using System.Text;

namespace CDS.Markdown.MarkdownDocumentCore;

/// <summary>
/// Renders a <see cref="MarkdownDocument"/> to a Markdown string by visiting each element in order.
/// </summary>
public class MarkdownWriter
{
    /// <summary>
    /// Renders the specified <see cref="MarkdownDocument"/> to a Markdown string.
    /// </summary>
    /// <param name="document">The document to render.</param>
    /// <returns>A string containing the complete Markdown representation of the document.</returns>
    public string Write(MarkdownDocument document)
    {
        var sb = new StringBuilder();

        foreach (var element in document.Elements)
        {
            switch (element)
            {
                case Heading heading:
                    WriteHeading(sb, heading);
                    break;
                case Paragraph paragraph:
                    WriteParagraph(sb, paragraph);
                    break;
                case CodeBlock codeBlock:
                    WriteCodeBlock(sb, codeBlock);
                    break;
                case BulletList bulletList:
                    WriteBulletList(sb, bulletList);
                    break;
                case Image image:
                    WriteImage(sb, image);
                    break;
                case MathBlock mathBlock:
                    WriteMathBlock(sb, mathBlock);
                    break;
            }
        }

        return sb.ToString();
    }

    private static void WriteHeading(StringBuilder sb, Heading heading)
    {
        var level = Math.Clamp(heading.Level, 1, 6);
        sb.Append(new string('#', level));
        sb.Append(' ');
        sb.AppendLine(heading.Text);
        sb.AppendLine();
    }

    private static void WriteParagraph(StringBuilder sb, Paragraph paragraph)
    {
        sb.AppendLine(paragraph.Text);
        sb.AppendLine();
    }

    private static void WriteCodeBlock(StringBuilder sb, CodeBlock codeBlock)
    {
        sb.Append("```");
        if (!string.IsNullOrEmpty(codeBlock.Language))
        {
            sb.Append(codeBlock.Language);
        }

        sb.AppendLine();
        sb.AppendLine(codeBlock.Code);
        sb.AppendLine("```");
        sb.AppendLine();
    }

    private static void WriteBulletList(StringBuilder sb, BulletList bulletList)
    {
        foreach (var item in bulletList.Items)
        {
            sb.Append("- ");
            sb.AppendLine(item);
        }

        sb.AppendLine();
    }

    private static void WriteImage(StringBuilder sb, Image image)
    {
        sb.Append("![");
        sb.Append(image.AltText);
        sb.Append("](");
        sb.Append(image.Path);
        sb.AppendLine(")");
        sb.AppendLine();
    }

    private static void WriteMathBlock(StringBuilder sb, MathBlock mathBlock)
    {
        sb.AppendLine("$$");
        sb.AppendLine(mathBlock.Math);
        sb.AppendLine("$$");
        sb.AppendLine();
    }
}
