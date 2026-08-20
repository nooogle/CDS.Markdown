namespace CDS.Markdown.MarkdownDocumentCore;

/// <summary>
/// Represents a Markdown document as an ordered collection of <see cref="IMarkdownElement"/> instances.
/// </summary>
public class MarkdownDocument
{
    private readonly List<IMarkdownElement> elements = [];

    /// <summary>
    /// Gets the elements contained in this document, in the order they were added.
    /// </summary>
    public IReadOnlyList<IMarkdownElement> Elements => elements;

    /// <summary>
    /// Adds an element to the end of the document.
    /// </summary>
    /// <param name="element">The element to add.</param>
    public void Add(IMarkdownElement element) => elements.Add(element);
}
