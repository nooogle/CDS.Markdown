using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;
using Markdig;

namespace CDS.Markdown;

public partial class MarkdownViewer : UserControl
{
    private string? currentDirectory;

    public MarkdownViewer()
    {
        InitializeComponent();
    }


    public async void LoadMarkdown(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("Markdown file not found", filePath);

        currentDirectory = Path.GetDirectoryName(Path.GetFullPath(filePath))!;

        await EnsureWebView2ReadyAsync();

        string markdown = await File.ReadAllTextAsync(filePath);

        var pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .Build();

        string htmlBody = Markdig.Markdown.ToHtml(markdown, pipeline);

        string baseHref = $"<base href=\"file:///{currentDirectory.Replace("\\", "/")}/\">";
        string linkInterceptScript = @"
<script>
document.addEventListener('DOMContentLoaded', function() {
  document.body.addEventListener('click', function(e) {
    let t = e.target;
    while (t && t.tagName !== 'A') t = t.parentElement;
    if (t && t.tagName === 'A') {
      let href = t.getAttribute('href');
      if (href && href.match(/\.md($|[#?])/i)) {
        e.preventDefault();
        window.chrome.webview.postMessage(href);
      }
    }
  }, true);
});
</script>";

        string html = $@"
<!DOCTYPE html>
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

        string tempHtmlPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".html");
        File.WriteAllText(tempHtmlPath, html);
        webView.Source = new Uri(tempHtmlPath);
    }

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

        webView.CoreWebView2.WebMessageReceived -= WebMessageReceived;
        webView.CoreWebView2.WebMessageReceived += WebMessageReceived;

        webView.CoreWebView2.NavigationStarting -= NavigationStarting;
        webView.CoreWebView2.NavigationStarting += NavigationStarting;
    }

    private void WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
    {
        string? href = e.TryGetWebMessageAsString();
        if (string.IsNullOrEmpty(href) || currentDirectory == null)
            return;

        string fileOnly = href.Split('?', '#')[0];
        string targetPath = Path.Combine(currentDirectory, fileOnly);
        if (File.Exists(targetPath))
        {
            LoadMarkdown(targetPath);
        }
    }

    private void NavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
    {
        if (string.IsNullOrEmpty(e.Uri) || currentDirectory == null)
            return;

        if (e.Uri.StartsWith("file://", StringComparison.OrdinalIgnoreCase) &&
            e.Uri.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
        {
            e.Cancel = true;

            string localPath = Uri.UnescapeDataString(new Uri(e.Uri).LocalPath);
            string fullPath = Path.GetFullPath(localPath);

            if (File.Exists(fullPath))
            {
                LoadMarkdown(fullPath);
            }
        }
    }

    private void btnBack_Click(object sender, EventArgs e)
    {
        webView.GoBack();
    }

    private void btnForward_Click(object sender, EventArgs e)
    {
        webView.GoForward();
    }
}
