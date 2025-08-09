namespace CDS.Markdown;

/// <summary>
/// Encapsulates the navigation/session state for a Markdown viewing session, including navigation, path management, and file loading.
/// </summary>
public class MarkdownViewerSession
{
    private readonly MarkdownDocumentService markdownService;
    private string? currentDirectory;
    private string? homeMarkdownPath;
    private string? currentMarkdownPath;

    /// <summary>
    /// Raised when navigation occurs and new HTML is ready to be displayed.
    /// </summary>
    public event Func<string, Task>? HtmlReady;

    /// <summary>
    /// Gets the current directory for resolving relative links.
    /// </summary>
    public string? CurrentDirectory => currentDirectory;

    /// <summary>
    /// Gets the path to the current markdown file.
    /// </summary>
    public string? CurrentMarkdownPath => currentMarkdownPath;

    /// <summary>
    /// Gets the path to the home markdown file.
    /// </summary>
    public string? HomeMarkdownPath => homeMarkdownPath;

    public MarkdownViewerSession(MarkdownDocumentService? service = null)
    {
        markdownService = service ?? new MarkdownDocumentService();
    }

    /// <summary>
    /// Loads and navigates to a Markdown file, raising HtmlReady when done.
    /// </summary>
    public async Task NavigateToAsync(string filePath, bool setHome = false)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Markdown file not found", filePath);
        }
        currentMarkdownPath = filePath;
        currentDirectory = Path.GetDirectoryName(Path.GetFullPath(filePath));
        if (setHome || string.IsNullOrEmpty(homeMarkdownPath))
        {
            homeMarkdownPath = filePath;
        }
        var baseHref = $"<base href=\"file:///{currentDirectory!.Replace("\\", "/")}/\">";
        string html = await markdownService.BuildHtmlFromMarkdownFileAsync(filePath, baseHref);
        if (HtmlReady != null)
        {
            await HtmlReady.Invoke(html);
        }
    }

    /// <summary>
    /// Navigates to the home markdown file, if set.
    /// </summary>
    public async Task GoHomeAsync()
    {
        if (!string.IsNullOrEmpty(homeMarkdownPath))
        {
            await NavigateToAsync(homeMarkdownPath!);
        }
    }

    /// <summary>
    /// Handles a markdown link click (e.g., from WebView2 message), resolves and navigates if valid.
    /// </summary>
    public async Task HandleMarkdownLinkAsync(string? href)
    {
        if (string.IsNullOrEmpty(href) || string.IsNullOrEmpty(currentDirectory))
        {
            return;
        }
        string fileOnly = href.Split('?', '#')[0];
        string targetPath = Path.Combine(currentDirectory, fileOnly);
        if (File.Exists(targetPath))
        {
            await NavigateToAsync(targetPath);
        }
    }

    /// <summary>
    /// Handles navigation to a URI (e.g., from WebView2 navigation), intercepts .md files and navigates if valid.
    /// </summary>
    public async Task HandleNavigationUriAsync(string? uri, Action cancelNavigation)
    {
        if (string.IsNullOrEmpty(uri) || string.IsNullOrEmpty(currentDirectory))
        {
            return;
        }
        if (uri.StartsWith("file://", StringComparison.OrdinalIgnoreCase) &&
            uri.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
        {
            cancelNavigation();
            string localPath = Uri.UnescapeDataString(new Uri(uri).LocalPath);
            string fullPath = Path.GetFullPath(localPath);
            if (File.Exists(fullPath))
            {
                await NavigateToAsync(fullPath);
            }
        }
    }
}
