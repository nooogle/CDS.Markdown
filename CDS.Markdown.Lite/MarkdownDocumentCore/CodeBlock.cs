namespace CDS.Markdown.MarkdownDocumentCore;

/// <summary>
/// Represents a fenced Markdown code block with optional syntax highlighting language.
/// </summary>
/// <param name="Code">The code content.</param>
/// <param name="Language">The optional language identifier for syntax highlighting (e.g., "csharp", "json").</param>
public record CodeBlock(string Code, string? Language) : IMarkdownElement;
