using Microsoft.Web.WebView2.Core;

namespace CDS.Markdown;

/// <summary>
/// A WinForms UserControl for rendering Markdown files using Markdig and WebView2.
/// </summary>
public partial class MarkdownViewer : UserControl
{
    private readonly SemaphoreSlim initGate = new(initialCount: 1, maxCount: 1);

    /// <summary>
    /// Tracks all temp HTML files created for cleanup on dispose.
    /// </summary>
    private readonly TempHtmlFileManager tempHtmlFileManager = new();

    /// <summary>
    /// The Markdown viewer session, managing state and navigation.
    /// </summary>
    private readonly MarkdownViewerSession session = new();

    /// <summary>
    /// Gets the options for configuring the Markdown viewer.
    /// </summary>
    public MarkdownViewerOptions Options { get; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="MarkdownViewer"/> class.
    /// </summary>
    public MarkdownViewer()
    {
        InitializeComponent();
        session.HtmlReady += OnHtmlReadyAsync;
    }

    /// <summary>
    /// Renders a Markdown string directly without requiring a file on disk.
    /// Home navigation is not available when using this method.
    /// </summary>
    /// <param name="markdown">The Markdown text to render.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task LoadMarkdownFromStringAsync(string markdown)
    {
        await session.LoadMarkdownFromStringAsync(markdown);
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
    /// Renders the provided HTML in the WebView2 control.
    /// </summary>
    /// <param name="html">The HTML content to display.</param>
    private async Task OnHtmlReadyAsync(string html)
    {
        await EnsureWebView2ReadyAsync();
        var tempHtmlPath = tempHtmlFileManager.CreateTempHtmlFile(html);
        webView.Source = new Uri(tempHtmlPath);
    }

    /// <summary>
    /// Ensures the WebView2 control is initialized and event handlers are attached.
    /// Sets creation properties before initialization as required by WebView2.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if WebView2 fails to initialize.</exception>
    private async Task EnsureWebView2ReadyAsync()
    {
        await initGate.WaitAsync().ConfigureAwait(true);

        try
        {
            if (webView.CoreWebView2 != null)
            {
                return;
            }

            // Ensure control handle exists and we’re on the UI thread.
            var _ = Handle;

            // Determine user data folder for WebView2 profile.
            var userData = Options.UserDataFolder;
            if (string.IsNullOrWhiteSpace(userData))
            {
                var root = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                userData = Path.Combine(root, "CDS.MarkdownViewer", "WebView2", "UserData");
            }
            Directory.CreateDirectory(userData);

            // Set creation properties before initializing WebView2.
            webView.CreationProperties = new Microsoft.Web.WebView2.WinForms.CoreWebView2CreationProperties
            {
                UserDataFolder = userData,
                ProfileName = Options.ProfileName,
                BrowserExecutableFolder = Options.BrowserExecutableFolder
            };

            // Initialize WebView2.
            await webView.EnsureCoreWebView2Async();
            if (webView.CoreWebView2 == null)
            {
                throw new InvalidOperationException("WebView2 is not initialized.");
            }

            AttachWebView2EventHandlers();
        }
        finally
        {
            initGate.Release();
        }
    }

    /// <summary>
    /// Attaches event handlers to the WebView2 control, ensuring no duplicates.
    /// </summary>
    private void AttachWebView2EventHandlers()
    {
        webView.CoreWebView2.WebMessageReceived -= WebMessageReceived;
        webView.CoreWebView2.WebMessageReceived += WebMessageReceived;

        webView.CoreWebView2.NavigationStarting -= NavigationStarting;
        webView.CoreWebView2.NavigationStarting += NavigationStarting;
    }

    /// <summary>
    /// Handles messages from the WebView2 (e.g., when a .md link is clicked in the HTML).
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
    {
        var href = e.TryGetWebMessageAsString();
        _ = session.HandleMarkdownLinkAsync(href);
    }

    /// <summary>
    /// Handles navigation events in WebView2 to intercept .md file navigation.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void NavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
    {
        _ = session.HandleNavigationUriAsync(e.Uri, () => e.Cancel = true);
    }

    /// <summary>
    /// Cleans up all temporary HTML files created by this control and disposes resources.
    /// </summary>
    /// <param name="disposing">True if called from Dispose; false if called from finalizer.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
            tempHtmlFileManager.Dispose();
            initGate.Dispose();
        }
        base.Dispose(disposing);
    }

    /// <summary>
    /// User clicks the "Home" button to load the Markdown file specified in <see cref="LoadMarkdownAsync(string)"/>.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void btnHome_Click(object sender, EventArgs e)
    {
        _ = session.GoHomeAsync();
    }

    /// <summary>
    /// User clicks the "Back" button to navigate to the previous page in the WebView2 history.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void btnBack_Click(object sender, EventArgs e)
    {
        webView.GoBack();
    }

    /// <summary>
    /// User clicks the "Forward" button to navigate to the next page in the WebView2 history.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void btnForward_Click(object sender, EventArgs e)
    {
        webView.GoForward();
    }
}
