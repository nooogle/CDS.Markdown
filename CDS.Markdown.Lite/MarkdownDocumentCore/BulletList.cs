namespace CDS.Markdown.MarkdownDocumentCore;

/// <summary>
/// Represents a Markdown unordered (bullet) list element.
/// </summary>
/// <param name="Items">The list of items to render as bullet points.</param>
public record BulletList(IReadOnlyList<string> Items) : IMarkdownElement;
