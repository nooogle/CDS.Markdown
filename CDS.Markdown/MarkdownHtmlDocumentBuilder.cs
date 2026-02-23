namespace CDS.Markdown;

/// <summary>
/// Builds a complete HTML document for rendered Markdown content, including CSS, base href, and scripts.
/// </summary>
public class MarkdownHtmlDocumentBuilder
{
    /// <summary>The GitHub Markdown CSS to embed in the HTML document.</summary>
    private readonly string githubCss;

    /// <summary>The default CSS to embed in the HTML document.</summary>
    private readonly string defaultCss;

    /// <summary>The script to intercept .md link clicks in the HTML document.</summary>
    private readonly string linkInterceptScript;

    /// <summary>
    /// The minified mermaid.js bundle to inline, or <c>null</c> if diagram
    /// rendering is not required.
    /// </summary>
    private readonly string? mermaidBundle;

    /// <summary>
    /// The Mermaid initialisation script that rewrites Markdig code blocks into
    /// mermaid divs and calls <c>mermaid.run()</c>, or <c>null</c> when mermaid
    /// is not included.
    /// </summary>
    private readonly string? mermaidInitScript;

    /// <summary>
    /// Initializes a new instance of the <see cref="MarkdownHtmlDocumentBuilder"/> class.
    /// </summary>
    /// <param name="githubCss">The GitHub Markdown CSS to embed.</param>
    /// <param name="defaultCss">The default CSS to embed.</param>
    /// <param name="linkInterceptScript">The script to intercept .md link clicks.</param>
    /// <param name="mermaidBundle">
    /// The minified mermaid.js source to inline. When provided, Mermaid fenced
    /// code blocks are rendered as diagrams. Pass <c>null</c> to omit.
    /// </param>
    /// <param name="mermaidInitScript">
    /// The initialisation script that wires Mermaid to the rendered HTML.
    /// Required when <paramref name="mermaidBundle"/> is supplied.
    /// </param>
    public MarkdownHtmlDocumentBuilder(
        string githubCss,
        string defaultCss,
        string linkInterceptScript,
        string? mermaidBundle = null,
        string? mermaidInitScript = null)
    {
        this.githubCss = githubCss;
        this.defaultCss = defaultCss;
        this.linkInterceptScript = linkInterceptScript;
        this.mermaidBundle = mermaidBundle;
        this.mermaidInitScript = mermaidInitScript;
    }

    /// <summary>
    /// Builds the full HTML document for the given HTML body and base href.
    /// </summary>
    /// <param name="htmlBody">The HTML body content (rendered from Markdown).</param>
    /// <param name="baseHref">The base href for resolving relative links.</param>
    /// <returns>The complete HTML document as a string.</returns>
    public string Build(string htmlBody, string baseHref)
    {
        var mermaidScripts = mermaidBundle is not null
            ? $"  <script>{mermaidBundle}</script>\n  {mermaidInitScript}\n"
            : string.Empty;

        return $"<!DOCTYPE html>\n" +
               "<html>\n" +
               "<head>\n" +
               "  <meta charset='utf-8'>\n" +
               $"  {baseHref}\n" +
               "  <style>\n" +
               $"    {defaultCss}\n" +
               $"    {githubCss}\n" +
               "  </style>\n" +
               $"  {linkInterceptScript}\n" +
               mermaidScripts +
               "</head>\n" +
               "<body>\n" +
               "<div class=\"markdown-body\">\n" +
               $"{htmlBody}\n" +
               "</div>\n" +
               "</body>\n" +
               "</html>";
    }
}
