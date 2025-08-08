using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;
using Markdig;

namespace CDS.Markdown;

/// <summary>
/// A WinForms UserControl for rendering Markdown files using Markdig and WebView2.
/// </summary>
public partial class MarkdownViewer : UserControl
{
    // The directory of the currently loaded Markdown file, used for resolving relative links.
    private string? currentDirectory;
    // Tracks all temp HTML files created for cleanup on dispose.
    private readonly List<string> tempHtmlFiles = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="MarkdownViewer"/> class.
    /// </summary>
    public MarkdownViewer()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Loads and renders a Markdown file asynchronously.
    /// </summary>
    /// <param name="filePath">The path to the Markdown file.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the file does not exist.</exception>
    public async Task LoadMarkdownAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Markdown file not found", filePath);
        }

        // Set the current directory for resolving relative links.
        currentDirectory = Path.GetDirectoryName(Path.GetFullPath(filePath))!;

        await EnsureWebView2ReadyAsync();

        // Read the Markdown file content.
        string markdown = await File.ReadAllTextAsync(filePath);

        // Build the Markdig pipeline with advanced extensions.
        var pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .Build();

        // Convert Markdown to HTML.
        string htmlBody = Markdig.Markdown.ToHtml(markdown, pipeline);

        // Set base href for relative links and inject script to intercept .md links.
        var baseHref = $"<base href=\"file:///{currentDirectory.Replace("\\", "/")}/\">";
        var linkInterceptScript = @"<script>
document.addEventListener('DOMContentLoaded', function() {
  document.body.addEventListener('click', function(e) {
    let t = e.target;
    while (t && t.tagName !== 'A') t = t.parentElement;
    if (t && t.tagName === 'A') {
      let href = t.getAttribute('href');
      if (href && href.match(/\\.md($|[#?])/i)) {
        e.preventDefault();
        window.chrome.webview.postMessage(href);
      }
    }
  }, true);
});
</script>";

        // Compose the full HTML document.
        string html = $@"<!DOCTYPE html>
<html>
<head>
  <meta charset='utf-8'>
  {baseHref}
  <style>
    body {{ font-family: sans-serif; padding: 20px; }}
    table {{
      border-collapse: collapse;
      width: auto;
      max-width: 100%;
      margin-bottom: 1em;
      word-break: break-word;
      overflow-x: auto;
      display: block;
    }}
    th, td {{
      border: 1px solid #888;
      padding: 6px 10px;
      text-align: left;
      vertical-align: top;
      word-break: break-word;
    }}
    th {{ background: #f0f0f0; }}
  </style>
  {linkInterceptScript}
</head>
<body>
{htmlBody}
</body>
</html>";

        // Write the HTML to a temporary file and navigate the WebView to it.
        string tempHtmlPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".html");
        File.WriteAllText(tempHtmlPath, html);
        webView.Source = new Uri(tempHtmlPath);

        // Track the temp file for later cleanup
        tempHtmlFiles.Add(tempHtmlPath);
    }

    /// <summary>
    /// Ensures the WebView2 control is initialized and event handlers are attached.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if WebView2 fails to initialize.</exception>
    private async Task EnsureWebView2ReadyAsync()
    {
        if (webView.CoreWebView2 == null)
        {
            await webView.EnsureCoreWebView2Async();
        }

        if (webView.CoreWebView2 == null)
        {
            throw new InvalidOperationException("WebView2 is not initialized.");
        }

        // Attach event handlers (remove first to avoid duplicates).
        webView.CoreWebView2.WebMessageReceived -= WebMessageReceived;
        webView.CoreWebView2.WebMessageReceived += WebMessageReceived;

        webView.CoreWebView2.NavigationStarting -= NavigationStarting;
        webView.CoreWebView2.NavigationStarting += NavigationStarting;
    }

    /// <summary>
    /// Handles messages from the WebView2 (e.g., when a .md link is clicked in the HTML).
    /// </summary>
    private void WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
    {
        string? href = e.TryGetWebMessageAsString();
        if (string.IsNullOrEmpty(href) || currentDirectory == null)
        {
            return;
        }

        // Remove query/hash from href and resolve the target path.
        string fileOnly = href.Split('?', '#')[0];
        string targetPath = Path.Combine(currentDirectory, fileOnly);
        if (File.Exists(targetPath))
        {
            // Fire-and-forget: load the new Markdown file.
            _ = LoadMarkdownAsync(targetPath);
        }
    }

    /// <summary>
    /// Handles navigation events in WebView2 to intercept .md file navigation.
    /// </summary>
    private void NavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
    {
        if (string.IsNullOrEmpty(e.Uri) || currentDirectory == null)
        {
            return;
        }

        // Intercept navigation to local .md files and load them in the viewer instead.
        if (e.Uri.StartsWith("file://", StringComparison.OrdinalIgnoreCase) &&
            e.Uri.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
        {
            e.Cancel = true;

            string localPath = Uri.UnescapeDataString(new Uri(e.Uri).LocalPath);
            string fullPath = Path.GetFullPath(localPath);

            if (File.Exists(fullPath))
            {
                // Fire-and-forget: load the new Markdown file.
                _ = LoadMarkdownAsync(fullPath);
            }
        }
    }

    /// <summary>
    /// Navigates the WebView2 control back in its history.
    /// </summary>
    private void btnBack_Click(object sender, EventArgs e)
    {
        webView.GoBack();
    }

    /// <summary>
    /// Navigates the WebView2 control forward in its history.
    /// </summary>
    private void btnForward_Click(object sender, EventArgs e)
    {
        webView.GoForward();
    }

    /// <summary>
    /// Cleans up all temporary HTML files created by this control.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            foreach (var tempFile in tempHtmlFiles)
            {
                try { if (File.Exists(tempFile)) File.Delete(tempFile); } catch { /* Ignore errors */ }
            }
            tempHtmlFiles.Clear();
        }
        base.Dispose(disposing);
    }
}
