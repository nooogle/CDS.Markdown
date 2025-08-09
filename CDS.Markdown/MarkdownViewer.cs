using Microsoft.Web.WebView2.Core;

namespace CDS.Markdown;

/// <summary>
/// A WinForms UserControl for rendering Markdown files using Markdig and WebView2.
/// </summary>
public partial class MarkdownViewer : UserControl
{
    /// <summary>Tracks all temp HTML files created for cleanup on dispose.</summary>
    private readonly TempHtmlFileManager tempHtmlFileManager = new();

    /// <summary>
    /// The Markdown viewer session, managing state and navigation.
    /// </summary>
    private readonly MarkdownViewerSession session = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="MarkdownViewer"/> class.
    /// </summary>
    public MarkdownViewer()
    {
        InitializeComponent();
        session.HtmlReady += OnHtmlReadyAsync;
    }

    /// <summary>
    /// Loads and renders a Markdown file asynchronously. Stores the path for 'Home' navigation.
    /// </summary>
    /// <param name="filePath">The path to the Markdown file.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the file does not exist.</exception>
    public async Task LoadMarkdownAsync(string filePath)
    {
        await session.NavigateToAsync(filePath, setHome: true);
    }

    /// <summary>
    /// Handles the HtmlReady event from the MarkdownViewerSession.
    /// </summary>
    private async Task OnHtmlReadyAsync(string html)
    {
        await EnsureWebView2ReadyAsync();
        string tempHtmlPath = tempHtmlFileManager.CreateTempHtmlFile(html);
        webView.Source = new Uri(tempHtmlPath);
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
        _ = session.HandleMarkdownLinkAsync(href);
    }

    /// <summary>
    /// Handles navigation events in WebView2 to intercept .md file navigation.
    /// </summary>
    private void NavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
    {
        _ = session.HandleNavigationUriAsync(e.Uri, () => e.Cancel = true);
    }

    /// <summary>
    /// Cleans up all temporary HTML files created by this control.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (components != null)
            {
                components.Dispose();
            }
            tempHtmlFileManager.Dispose();
        }
        base.Dispose(disposing);
    }


    /// <summary>
    /// User clicks the "Home" button to load the Markdown file specified in <see cref="LoadMarkdownAsync(string)"/>
    /// </summary>
    private void btnHome_Click(object sender, EventArgs e)
    {
        _ = session.GoHomeAsync();
    }

    /// <summary>
    /// User clicks the "Back" button to navigate to the previous page in the WebView2 history.
    /// </summary>
    private void btnBack_Click(object sender, EventArgs e)
    {
        webView.GoBack();
    }

    /// <summary>
    /// User clicks the "Forward" button to navigate to the next page in the WebView2 history.
    /// </summary>
    private void btnForward_Click(object sender, EventArgs e)
    {
        webView.GoForward();
    }
}
