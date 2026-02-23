namespace CDS.Markdown.MarkdownDocumentCore;

/// <summary>
/// Represents a Markdown image element with alt text and a file path or URL.
/// </summary>
/// <param name="AltText">The alternative text for the image.</param>
/// <param name="Path">The path or URL of the image.</param>
public record Image(string AltText, string Path) : IMarkdownElement;
