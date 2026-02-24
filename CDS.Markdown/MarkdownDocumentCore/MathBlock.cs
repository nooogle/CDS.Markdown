namespace CDS.Markdown.MarkdownDocumentCore;

/// <summary>
/// Represents a block of LaTeX-like mathematics in a Markdown document.
/// </summary>
public class MathBlock : IMarkdownElement
{
    /// <summary>
    /// Gets the mathematical expression.
    /// </summary>
    public string Math { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MathBlock"/> class.
    /// </summary>
    /// <param name="math">The mathematical expression.</param>
    public MathBlock(string math)
    {
        Math = math;
    }
}
