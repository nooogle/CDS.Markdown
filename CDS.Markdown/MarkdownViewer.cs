using System.Text.Json;
using System.Text.Json.Serialization;
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
    /// Raised whenever the rendered content's height changes: once after the initial
    /// render, and again if it changes afterwards (web fonts finishing, Mermaid diagrams
    /// or MathJax reflowing content, etc.). The value is the content height in CSS pixels.
    /// Useful for a host that wants to size its own container to the content rather than
    /// fixing a height up front.
    /// </summary>
    public event EventHandler<int>? ContentHeightChanged;

    /// <summary>
    /// Initializes a new instance of the <see cref="MarkdownViewer"/> class.
    /// </summary>
    public MarkdownViewer()
    {
        InitializeComponent();
        session.HtmlReady += OnHtmlReadyAsync;
        ApplyToolbarVisibility();
    }

    /// <summary>
    /// Renders a Markdown string directly without requiring a file on disk.
    /// Home navigation is not available when using this method.
    /// </summary>
    /// <param name="markdown">The Markdown text to render.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task LoadMarkdownFromStringAsync(string markdown)
    {
        ApplyThemeToWebView();
        ApplyToolbarVisibility();
        session.Theme = Options.Theme;
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
        ApplyThemeToWebView();
        ApplyToolbarVisibility();
        session.Theme = Options.Theme;
        await session.NavigateToAsync(filePath, setHome: true);
    }

    /// <summary>
    /// Reads the current rendered content height on demand.
    /// Prefer <see cref="ContentHeightChanged"/> for hosts that want to react to height
    /// changes as they happen; use this when a one-off, up-to-date reading is enough.
    /// </summary>
    /// <returns>The content height in CSS pixels, or 0 if nothing has been rendered yet.</returns>
    public async Task<int> GetContentHeightAsync()
    {
        await EnsureWebView2ReadyAsync();
        var result = await webView.CoreWebView2!.ExecuteScriptAsync("document.body.scrollHeight");
        return int.TryParse(result, out var height) ? height : 0;
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

            // Ensure control handle exists and we�re on the UI thread.
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
            ApplyThemeToWebView();
        }
        finally
        {
            initGate.Release();
        }
    }

    /// <summary>
    /// Applies the selected theme to the WebView2 profile.
    /// </summary>
    private void ApplyThemeToWebView()
    {
        if (webView.CoreWebView2?.Profile == null)
            return;

        webView.CoreWebView2.Profile.PreferredColorScheme = Options.Theme switch
        {
            MarkdownViewerTheme.Light => CoreWebView2PreferredColorScheme.Light,
            MarkdownViewerTheme.Dark => CoreWebView2PreferredColorScheme.Dark,
            _ => CoreWebView2PreferredColorScheme.Auto
        };
    }

    /// <summary>
    /// Shows or hides the Home/Back/Forward navigation toolbar based on <see cref="MarkdownViewerOptions.ShowNavigationToolbar"/>.
    /// When hidden, the WebView2 control docks to fill the entire control since <c>panel1</c> is
    /// excluded from Windows Forms' dock layout while invisible.
    /// </summary>
    private void ApplyToolbarVisibility()
    {
        panel1.Visible = Options.ShowNavigationToolbar;
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
    /// Handles messages from the WebView2 (e.g., a .md link clicked in the HTML, or a
    /// content-height report). Messages are JSON objects with a <c>type</c> discriminator
    /// so multiple message shapes can share the one WebView2 messaging channel;
    /// see <see cref="MarkdownViewerResources.LinkInterceptScript"/> and
    /// <see cref="MarkdownViewerResources.ContentHeightScript"/> for the senders.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
    {
        WebViewMessage? message;
        try
        {
            message = JsonSerializer.Deserialize<WebViewMessage>(e.WebMessageAsJson);
        }
        catch (JsonException)
        {
            return;
        }

        switch (message?.Type)
        {
            case "linkClick":
                _ = session.HandleMarkdownLinkAsync(message.Href);
                break;

            case "contentHeight" when message.Height is int height:
                ContentHeightChanged?.Invoke(this, height);
                break;
        }
    }

    /// <summary>
    /// The shape of messages posted from the rendered HTML over the WebView2 messaging
    /// channel. <see cref="Href"/> is populated for <c>linkClick</c> messages,
    /// <see cref="Height"/> for <c>contentHeight</c> messages.
    /// </summary>
    private sealed record WebViewMessage(
        [property: JsonPropertyName("type")] string? Type,
        [property: JsonPropertyName("href")] string? Href,
        [property: JsonPropertyName("height")] int? Height);

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
