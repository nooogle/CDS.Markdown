using System.Text;

using Markdig;
using Markdig.Extensions.Tables;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace CDS.Markdown;

/// <summary>
/// Renders a Markdown string as formatted text directly in a <see cref="RichTextBox"/>: bold,
/// italic, inline and fenced code, headings, lists (including nesting), block quotes, thematic
/// breaks, and a best-effort monospaced rendering of tables. There is no browser involved, so a
/// page of prose costs what any other <see cref="RichTextBox"/> costs — no WebView2 process, no
/// navigation chrome, and its rendered height is readable straight back from
/// <see cref="GetPreferredContentHeight"/> for a host that wants to size itself to the content.
/// </summary>
/// <remarks>
/// This is a prose-formatting control, not a document viewer: Mermaid diagrams, MathJax, real HTML
/// tables, images (beyond an italic placeholder), and clickable links are deliberately out of
/// scope. Reach for <c>MarkdownViewer</c> (in the CDS.Markdown package) instead when you need
/// those, or when the content isn't reliably small — see docs/viewer.md's "Embedding as a
/// Fragment" section for the tradeoff between the two.
/// </remarks>
public class MarkdownTextBox : RichTextBox
{
    private const string MonospaceFontFamily = "Cascadia Mono";
    private const int ListIndentPerLevel = 20;
    private const int ContentBottomPadding = 4;

    private static readonly MarkdownPipeline s_pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
    private static readonly Color s_codeBackColour = Color.FromArgb(240, 240, 240);
    private static readonly Color s_linkColour = Color.FromArgb(0, 102, 204);
    private static readonly Color s_mutedColour = SystemColors.GrayText;

    private readonly Stack<TextRunStyle> _styles = new();

    /// <summary>Initialises a new instance of the <see cref="MarkdownTextBox"/> class.</summary>
    public MarkdownTextBox()
    {
        BorderStyle = BorderStyle.None;
        BackColor = SystemColors.Window;
        ReadOnly = true;
        WordWrap = true;
        ScrollBars = RichTextBoxScrollBars.None;
        DetectUrls = false;
    }

    /// <summary>
    /// Replaces the control's content with the rendered form of <paramref name="markdown"/>.
    /// </summary>
    /// <param name="markdown">
    /// The Markdown source to render, or <see langword="null"/>/blank to clear the control.
    /// </param>
    public void SetMarkdown(string? markdown)
    {
        Clear();
        RenderMarkdown(markdown, separateFromExistingContent: false);

        SelectionStart = 0;
        SelectionLength = 0;
    }

    /// <summary>
    /// Renders <paramref name="markdown"/> and adds it after the control's existing content,
    /// separated from it by a blank line — for a host building up a transcript or log rather
    /// than showing one document at a time. Leaves existing content, including anything added
    /// via <see cref="AppendPlainText"/>, untouched.
    /// </summary>
    /// <param name="markdown">
    /// The Markdown source to render and append. Does nothing for <see langword="null"/> or
    /// blank input — unlike <see cref="SetMarkdown"/>, appending nothing must not clear
    /// existing content.
    /// </param>
    public void AppendMarkdown(string? markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown))
        {
            return;
        }

        RenderMarkdown(markdown, separateFromExistingContent: true);

        SelectionStart = TextLength;
        SelectionLength = 0;
        ScrollToCaret();
    }

    /// <summary>
    /// Appends one line of plain, unparsed text after the control's existing content, in a
    /// monospaced font with an optional background colour — for a host that wants to interleave
    /// pre-formatted content (e.g. a diff) with rendered Markdown, without it being parsed as
    /// Markdown itself.
    /// </summary>
    /// <param name="text">The line's text. A trailing newline is added; do not include one.</param>
    /// <param name="backColor">
    /// The line's background colour, or <see langword="null"/> to use the control's own
    /// <see cref="Control.BackColor"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="text"/> is <see langword="null"/>.</exception>
    public void AppendPlainText(string text, Color? backColor = null)
    {
        ArgumentNullException.ThrowIfNull(text);

        var style = DefaultStyle() with { FontFamily = MonospaceFontFamily, BackColor = backColor ?? BackColor };
        AppendLiteral(text + "\n", style);

        SelectionStart = TextLength;
        SelectionLength = 0;
        ScrollToCaret();
    }

    /// <summary>
    /// Parses and renders <paramref name="markdown"/> onto the control's existing content,
    /// optionally preceding it with a blank line when that content is non-empty.
    /// </summary>
    private void RenderMarkdown(string? markdown, bool separateFromExistingContent)
    {
        if (string.IsNullOrWhiteSpace(markdown))
        {
            return;
        }

        // Fully qualified: within the CDS.Markdown namespace, an unqualified "Markdown" binds to
        // the enclosing namespace itself rather than Markdig's Markdown class.
        var document = Markdig.Markdown.Parse(markdown, s_pipeline);

        _styles.Clear();
        _styles.Push(DefaultStyle());
        try
        {
            if (separateFromExistingContent && TextLength > 0)
            {
                AppendLiteral("\n\n", CurrentStyle);
            }

            RenderBlockSequence(document, blankLineBetween: true);
        }
        finally
        {
            _styles.Clear();
        }
    }

    /// <summary>
    /// Computes the height the control's content needs at its current width.
    /// </summary>
    /// <remarks>
    /// Deliberately not driven by <see cref="RichTextBox.ContentsResized"/>: that notification is
    /// unreliable for a <see cref="RichTextBox"/> that is still being laid out (e.g. sized while
    /// not yet parented into a shown window's control tree) - a common state for a control a host
    /// is about to place into a dynamically-built list. <c>GetPositionFromCharIndex</c> is a direct
    /// query against the native control instead of a notification that has to arrive, so it works
    /// regardless of parenting state.
    /// </remarks>
    public int GetPreferredContentHeight()
    {
        if (TextLength == 0)
        {
            return 0;
        }

        var lastIndex = TextLength - 1;
        var position = GetPositionFromCharIndex(lastIndex);

        SelectionStart = lastIndex;
        SelectionLength = 1;
        var lastLineHeight = SelectionFont?.Height ?? Font.Height;
        SelectionStart = TextLength;
        SelectionLength = 0;

        return position.Y + lastLineHeight + ContentBottomPadding;
    }

    private TextRunStyle DefaultStyle() => new(Font.FontFamily.Name, Font.SizeInPoints, FontStyle.Regular, ForeColor, BackColor);

    private TextRunStyle CurrentStyle => _styles.Peek();

    private void RenderBlockSequence(IEnumerable<Block> blocks, bool blankLineBetween)
    {
        var first = true;
        foreach (var block in blocks)
        {
            if (!first)
            {
                AppendLiteral(blankLineBetween ? "\n\n" : "\n", CurrentStyle);
            }

            RenderBlock(block);
            first = false;
        }
    }

    private void RenderBlock(Block block)
    {
        switch (block)
        {
            case HeadingBlock heading:
                RenderHeading(heading);
                break;

            case ParagraphBlock paragraph:
                RenderInlineContainer(paragraph.Inline);
                break;

            // FencedCodeBlock derives from CodeBlock, so this arm must come first.
            case FencedCodeBlock fenced:
                RenderCodeBlock(fenced.Lines.ToString());
                break;

            case CodeBlock code:
                RenderCodeBlock(code.Lines.ToString());
                break;

            case QuoteBlock quote:
                RenderQuote(quote);
                break;

            case ListBlock list:
                RenderList(list, depth: 0);
                break;

            case Table table:
                RenderTable(table);
                break;

            case ThematicBreakBlock:
                AppendLiteral(new string('-', 40), CurrentStyle with { Color = s_mutedColour });
                break;

            default:
                // HtmlBlock and anything else unrecognised is silently skipped rather than
                // dumped as raw markup into the text box.
                break;
        }
    }

    private void RenderHeading(HeadingBlock heading)
    {
        var sizeBoost = heading.Level switch
        {
            1 => 7f,
            2 => 5f,
            3 => 3f,
            4 => 2f,
            5 => 1f,
            _ => 0f,
        };

        _styles.Push(CurrentStyle with { FontStyle = FontStyle.Bold, FontSize = CurrentStyle.FontSize + sizeBoost });
        RenderInlineContainer(heading.Inline);
        _styles.Pop();
    }

    private void RenderCodeBlock(string code)
    {
        var text = code.Replace("\r\n", "\n").TrimEnd('\n');
        AppendLiteral(text, CurrentStyle with { FontFamily = MonospaceFontFamily, BackColor = s_codeBackColour });
    }

    private void RenderQuote(QuoteBlock quote)
    {
        var start = TextLength;

        _styles.Push(CurrentStyle with { FontStyle = CurrentStyle.FontStyle | FontStyle.Italic, Color = s_mutedColour });
        RenderBlockSequence(quote, blankLineBetween: true);
        _styles.Pop();

        SelectionStart = start;
        SelectionLength = TextLength - start;
        SelectionIndent = ListIndentPerLevel;
        SelectionStart = TextLength;
        SelectionLength = 0;
    }

    private void RenderList(ListBlock list, int depth)
    {
        var index = int.TryParse(list.OrderedStart, out var start) ? start : 1;
        var first = true;

        foreach (var block in list)
        {
            if (block is not ListItemBlock item)
            {
                continue;
            }

            if (!first)
            {
                AppendLiteral("\n", CurrentStyle);
            }

            RenderListItem(item, list.IsOrdered, index, depth);
            index++;
            first = false;
        }
    }

    private void RenderListItem(ListItemBlock item, bool ordered, int index, int depth)
    {
        var markerStart = TextLength;
        AppendLiteral(ordered ? $"{index}. " : "• ", CurrentStyle);

        // Tracks where this item's own text ends, before any nested sublist. A nested
        // RenderList call sets its own (deeper) SelectionIndent on its own paragraphs; if this
        // item's indent were applied across the full range including that nested text, it would
        // overwrite the nested paragraphs' indent with this shallower one, since RichTextBox
        // applies SelectionIndent to every paragraph the selection touches.
        var ownContentEnd = TextLength;

        var first = true;
        foreach (var block in item)
        {
            if (!first)
            {
                AppendLiteral("\n", CurrentStyle);
            }

            if (block is ListBlock nested)
            {
                RenderList(nested, depth + 1);
            }
            else
            {
                RenderBlock(block);
                ownContentEnd = TextLength;
            }

            first = false;
        }

        SelectionStart = markerStart;
        SelectionLength = ownContentEnd - markerStart;
        SelectionIndent = depth * ListIndentPerLevel;
        SelectionHangingIndent = 14;
        SelectionStart = TextLength;
        SelectionLength = 0;
    }

    private void RenderTable(Table table)
    {
        var rows = new List<(string[] Cells, bool IsHeader)>();

        foreach (var block in table)
        {
            if (block is not TableRow row)
            {
                continue;
            }

            var cells = row
                .Select(cellBlock => cellBlock is TableCell cell ? GetCellPlainText(cell) : string.Empty)
                .ToArray();
            rows.Add((cells, row.IsHeader));
        }

        if (rows.Count == 0)
        {
            return;
        }

        var columnCount = rows.Max(r => r.Cells.Length);
        var widths = new int[columnCount];
        for (var column = 0; column < columnCount; column++)
        {
            widths[column] = rows.Max(r => column < r.Cells.Length ? r.Cells[column].Length : 0);
        }

        var style = CurrentStyle with { FontFamily = MonospaceFontFamily };
        for (var i = 0; i < rows.Count; i++)
        {
            if (i > 0)
            {
                AppendLiteral("\n", style);
            }

            AppendLiteral(FormatRow(rows[i].Cells, widths), style);

            if (rows[i].IsHeader)
            {
                AppendLiteral("\n" + FormatSeparator(widths), style);
            }
        }
    }

    private static string FormatRow(string[] cells, int[] widths)
    {
        var parts = new string[widths.Length];
        for (var i = 0; i < widths.Length; i++)
        {
            var text = i < cells.Length ? cells[i] : string.Empty;
            parts[i] = text.PadRight(widths[i]);
        }

        return string.Join("  ", parts);
    }

    private static string FormatSeparator(int[] widths) =>
        string.Join("  ", widths.Select(w => new string('-', w)));

    private static string GetCellPlainText(TableCell cell)
    {
        var text = new StringBuilder();
        foreach (var block in cell)
        {
            if (block is ParagraphBlock { Inline: not null } paragraph)
            {
                text.Append(GetPlainText(paragraph.Inline));
            }
        }

        return text.ToString();
    }

    private void RenderInlineContainer(ContainerInline? container)
    {
        for (var inline = container?.FirstChild; inline is not null; inline = inline.NextSibling)
        {
            RenderInline(inline);
        }
    }

    private void RenderInline(Inline inline)
    {
        switch (inline)
        {
            case LiteralInline literal:
                AppendLiteral(literal.Content.ToString(), CurrentStyle);
                break;

            case CodeInline code:
                AppendLiteral(code.Content, CurrentStyle with { FontFamily = MonospaceFontFamily, BackColor = s_codeBackColour });
                break;

            case EmphasisInline emphasis:
                var style = CurrentStyle;
                style = emphasis.DelimiterCount >= 2
                    ? style with { FontStyle = style.FontStyle | FontStyle.Bold }
                    : style with { FontStyle = style.FontStyle | FontStyle.Italic };
                _styles.Push(style);
                RenderInlineContainer(emphasis);
                _styles.Pop();
                break;

            case LineBreakInline lineBreak:
                AppendLiteral(lineBreak.IsHard ? "\n" : " ", CurrentStyle);
                break;

            case LinkInline { IsImage: true } image:
                AppendLiteral(GetPlainText(image), CurrentStyle with { FontStyle = CurrentStyle.FontStyle | FontStyle.Italic, Color = s_mutedColour });
                break;

            case LinkInline link:
                _styles.Push(CurrentStyle with { FontStyle = CurrentStyle.FontStyle | FontStyle.Underline, Color = s_linkColour });
                RenderInlineContainer(link);
                _styles.Pop();
                break;

            case ContainerInline container:
                RenderInlineContainer(container);
                break;

            default:
                break;
        }
    }

    private static string GetPlainText(ContainerInline container)
    {
        var text = new StringBuilder();
        for (var inline = container.FirstChild; inline is not null; inline = inline.NextSibling)
        {
            switch (inline)
            {
                case LiteralInline literal:
                    text.Append(literal.Content.ToString());
                    break;
                case CodeInline code:
                    text.Append(code.Content);
                    break;
                case ContainerInline nested:
                    text.Append(GetPlainText(nested));
                    break;
            }
        }

        return text.ToString();
    }

    private void AppendLiteral(string text, TextRunStyle style)
    {
        if (text.Length == 0)
        {
            return;
        }

        var start = TextLength;
        AppendText(text);

        SelectionStart = start;
        SelectionLength = text.Length;
        using (var font = new Font(style.FontFamily, style.FontSize, style.FontStyle))
        {
            SelectionFont = font;
        }
        SelectionColor = style.Color;
        SelectionBackColor = style.BackColor;

        SelectionStart = TextLength;
        SelectionLength = 0;
    }

    private readonly record struct TextRunStyle(string FontFamily, float FontSize, FontStyle FontStyle, Color Color, Color BackColor);
}
