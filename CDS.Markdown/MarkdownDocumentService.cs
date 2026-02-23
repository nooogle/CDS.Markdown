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
        var mermaidBundle = LoadEmbeddedResource("CDS.Markdown.Resources.mermaid.min.js");
        renderer = new MarkdownRenderer();
        htmlBuilder = new MarkdownHtmlDocumentBuilder(
            githubCss,
            MarkdownViewerResources.DefaultCss,
            MarkdownViewerResources.LinkInterceptScript,
            mermaidBundle,
            MarkdownViewerResources.MermaidInitScript
        );
    }

    /// <summary>
    /// Renders a Markdown string to HTML and builds the full HTML document.
    /// </summary>
    /// <param name="markdown">The Markdown text to render.</param>
    /// <param name="baseHref">The base href for resolving relative links.</param>
    /// <returns>The complete HTML document as a string.</returns>
    public Task<string> BuildHtmlFromMarkdownAsync(string markdown, string baseHref)
    {
        var htmlBody = renderer.RenderToHtml(markdown);
        return Task.FromResult(htmlBuilder.Build(htmlBody, baseHref));
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

        string markdown = await File.ReadAllTextAsync(filePath).ConfigureAwait(false);
        string htmlBody = renderer.RenderToHtml(markdown);
        return htmlBuilder.Build(htmlBody, baseHref);
    }

    /// <summary>
    /// Loads the GitHub Markdown CSS from embedded resources.
    /// </summary>
    private static string LoadGithubMarkdownCss() =>
        LoadEmbeddedResource("CDS.Markdown.Resources.github-markdown.css");

    /// <summary>
    /// Loads an embedded resource by its fully-qualified manifest resource name.
    /// Returns an empty string if the resource is not found.
    /// </summary>
    /// <param name="resourceName">The fully-qualified manifest resource name.</param>
    private static string LoadEmbeddedResource(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream is null)
        {
            return string.Empty;
        }
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
