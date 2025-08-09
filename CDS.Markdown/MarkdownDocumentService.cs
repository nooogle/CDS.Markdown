using System.Reflection;

namespace CDS.Markdown;

/// <summary>
/// Service for loading, rendering, and building HTML from Markdown files using the same pipeline and resources as the MarkdownViewer.
/// </summary>
public class MarkdownDocumentService
{
    private readonly MarkdownRenderer renderer;
    private readonly MarkdownHtmlDocumentBuilder htmlBuilder;
    private readonly string githubCss;

    /// <summary>
    /// Initializes a new instance of the <see cref="MarkdownDocumentService"/> class.
    /// </summary>
    public MarkdownDocumentService()
    {
        githubCss = LoadGithubMarkdownCss();
        renderer = new MarkdownRenderer();
        htmlBuilder = new MarkdownHtmlDocumentBuilder(
            githubCss,
            MarkdownViewerResources.DefaultCss,
            MarkdownViewerResources.LinkInterceptScript
        );
    }

    /// <summary>
    /// Loads a Markdown file, renders it to HTML, and builds the full HTML document.
    /// </summary>
    /// <param name="filePath">The path to the Markdown file.</param>
    /// <param name="baseHref">The base href for resolving relative links.</param>
    /// <returns>The complete HTML document as a string.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the file does not exist.</exception>
    public async Task<string> BuildHtmlFromMarkdownFileAsync(string filePath, string baseHref)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Markdown file not found", filePath);
        }
#if NET48
        string markdown = File.ReadAllText(filePath);
        await Task.Yield(); // Simulate async for API consistency
#else
        string markdown = await File.ReadAllTextAsync(filePath).ConfigureAwait(false);
#endif
        string htmlBody = renderer.RenderToHtml(markdown);
        return htmlBuilder.Build(htmlBody, baseHref);
    }

    /// <summary>
    /// Loads the GitHub Markdown CSS from embedded resources.
    /// </summary>
    private static string LoadGithubMarkdownCss()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "CDS.Markdown.Resources.github-markdown.css";
        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream != null)
        {
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
        return string.Empty;
    }
}
