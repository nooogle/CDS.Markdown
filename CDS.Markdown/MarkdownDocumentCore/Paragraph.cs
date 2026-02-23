namespace CDS.Markdown.MarkdownDocumentCore;

/// <summary>
/// Represents a Markdown paragraph element containing plain text.
/// </summary>
/// <param name="Text">The paragraph text.</param>
public record Paragraph(string Text) : IMarkdownElement;
