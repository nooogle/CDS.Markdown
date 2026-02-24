using CDS.Markdown.MarkdownDocumentCore;

namespace CDS.Markdown;

/// <summary>
/// Provides a fluent API for constructing a Markdown document.
/// Each method returns <c>this</c> to allow chaining, and <see cref="ToMarkdown"/> finalises the output.
/// </summary>
public class FluentMarkdownDocument
{
    private readonly MarkdownDocument document = new();
    private readonly MarkdownWriter writer = new();

    /// <summary>
    /// Adds a level-1 heading to the document.
    /// </summary>
    /// <param name="text">The heading text.</param>
    public FluentMarkdownDocument AddHeading(string text) => AddHeading(text, 1);

    /// <summary>
    /// Adds a heading to the document at the specified level.
    /// </summary>
    /// <param name="text">The heading text.</param>
    /// <param name="level">The heading level (1–6).</param>
    public FluentMarkdownDocument AddHeading(string text, int level)
    {
        document.Add(new Heading(text, level));
        return this;
    }

    /// <summary>
    /// Adds a paragraph to the document.
    /// </summary>
    /// <param name="text">The paragraph text.</param>
    public FluentMarkdownDocument AddParagraph(string text)
    {
        document.Add(new Paragraph(text));
        return this;
    }

    /// <summary>
    /// Adds a fenced code block to the document without a language identifier.
    /// </summary>
    /// <param name="code">The code content.</param>
    public FluentMarkdownDocument AddCodeBlock(string code) => AddCodeBlock(code, null);

    /// <summary>
    /// Adds a fenced code block to the document with an optional language identifier.
    /// </summary>
    /// <param name="code">The code content.</param>
    /// <param name="language">The language identifier for syntax highlighting (e.g., "csharp").</param>
    public FluentMarkdownDocument AddCodeBlock(string code, string? language)
    {
        document.Add(new CodeBlock(code, language));
        return this;
    }

    /// <summary>
    /// Adds an unordered bullet list to the document.
    /// </summary>
    /// <param name="items">The list items.</param>
    public FluentMarkdownDocument AddBulletList(IReadOnlyList<string> items)
    {
        document.Add(new BulletList(items));
        return this;
    }

    /// <summary>
    /// Adds an image to the document.
    /// </summary>
    /// <param name="altText">The alternative text for the image.</param>
    /// <param name="path">The path or URL of the image.</param>
    public FluentMarkdownDocument AddImage(string altText, string path)
    {
        document.Add(new MarkdownDocumentCore.Image(altText, path));
        return this;
    }

    /// <summary>
    /// Adds a Mermaid diagram to the document as a fenced <c>mermaid</c> code block.
    /// The diagram is rendered as an SVG in the <see cref="MarkdownViewer"/> control.
    /// </summary>
    /// <param name="diagramDefinition">The Mermaid diagram source (e.g. a flowchart or sequence diagram).</param>
    public FluentMarkdownDocument AddMermaidDiagram(string diagramDefinition) =>
        AddCodeBlock(diagramDefinition, "mermaid");

    /// <summary>
    /// Adds a block of LaTeX-like mathematics to the document.
    /// </summary>
    /// <param name="math">The mathematical expression.</param>
    public FluentMarkdownDocument AddMathBlock(string math)
    {
        document.Add(new MathBlock(math));
        return this;
    }

    /// <summary>
    /// Renders the document and returns the complete Markdown string.
    /// </summary>
    public string ToMarkdown() => writer.Write(document);
}
