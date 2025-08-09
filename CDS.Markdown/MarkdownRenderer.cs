using Markdig;

namespace CDS.Markdown;

/// <summary>
/// Converts Markdown text to HTML using the Markdig pipeline.
/// </summary>
public class MarkdownRenderer
{
    /// <summary>
    /// The Markdig pipeline used for Markdown to HTML conversion.
    /// </summary>
    private readonly MarkdownPipeline pipeline;

    /// <summary>
    /// Initializes a new instance of the <see cref="MarkdownRenderer"/> class with advanced extensions enabled.
    /// </summary>
    public MarkdownRenderer()
    {
        pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .UseEmojiAndSmiley()
            .Build();
    }

    /// <summary>
    /// Converts the specified Markdown text to HTML.
    /// </summary>
    /// <param name="markdown">The Markdown text to convert.</param>
    /// <returns>The resulting HTML string.</returns>
    public string RenderToHtml(string markdown)
    {
        return Markdig.Markdown.ToHtml(markdown, pipeline);
    }
}
