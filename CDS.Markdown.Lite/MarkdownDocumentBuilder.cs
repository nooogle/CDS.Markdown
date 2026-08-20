using CDS.Markdown.MarkdownDocumentCore;

namespace CDS.Markdown;

/// <summary>
/// Provides a traditional builder API for constructing a Markdown document using discrete <c>Add*</c> calls,
/// finalised with <see cref="Build"/>.
/// </summary>
public class MarkdownDocumentBuilder
{
    private readonly MarkdownDocument document = new();
    private readonly MarkdownWriter writer = new();

    /// <summary>
    /// Adds a level-1 heading to the document.
    /// </summary>
    /// <param name="text">The heading text.</param>
    public void AddHeading(string text) => AddHeading(text, 1);

    /// <summary>
    /// Adds a heading to the document at the specified level.
    /// </summary>
    /// <param name="text">The heading text.</param>
    /// <param name="level">The heading level (1–6).</param>
    public void AddHeading(string text, int level) => document.Add(new Heading(text, level));

    /// <summary>
    /// Adds a paragraph to the document.
    /// </summary>
    /// <param name="text">The paragraph text.</param>
    public void AddParagraph(string text) => document.Add(new Paragraph(text));

    /// <summary>
    /// Adds a fenced code block to the document without a language identifier.
    /// </summary>
    /// <param name="code">The code content.</param>
    public void AddCodeBlock(string code) => AddCodeBlock(code, null);

    /// <summary>
    /// Adds a fenced code block to the document with an optional language identifier.
    /// </summary>
    /// <param name="code">The code content.</param>
    /// <param name="language">The language identifier for syntax highlighting (e.g., "csharp").</param>
    public void AddCodeBlock(string code, string? language) => document.Add(new CodeBlock(code, language));

    /// <summary>
    /// Adds an unordered bullet list to the document.
    /// </summary>
    /// <param name="items">The list items.</param>
    public void AddBulletList(IReadOnlyList<string> items) => document.Add(new BulletList(items));

    /// <summary>
    /// Adds an image to the document.
    /// </summary>
    /// <param name="altText">The alternative text for the image.</param>
    /// <param name="path">The path or URL of the image.</param>
    public void AddImage(string altText, string path) => document.Add(new MarkdownDocumentCore.Image(altText, path));

    /// <summary>
    /// Adds a block of LaTeX-like mathematics to the document.
    /// </summary>
    /// <param name="math">The mathematical expression.</param>
    public void AddMathBlock(string math) => document.Add(new MathBlock(math));

    /// <summary>
    /// Renders the document and returns the complete Markdown string.
    /// </summary>
    public string Build() => writer.Write(document);
}
