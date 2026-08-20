namespace CDS.Markdown.MarkdownDocumentCore;

/// <summary>
/// Represents a Markdown heading element with a text label and heading level (1–6).
/// </summary>
/// <param name="Text">The heading text.</param>
/// <param name="Level">The heading level, from 1 (largest) to 6 (smallest).</param>
public record Heading(string Text, int Level) : IMarkdownElement;
