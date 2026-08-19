namespace Demo;

/// <summary>
/// Demo form for the lightweight, non-WebView2 <see cref="CDS.Markdown.MarkdownTextBox"/> control.
/// </summary>
public partial class FormMarkdownTextBoxDemo : Form
{
    /// <summary>A short chat-reply-sized sample: prose, a table, and a list.</summary>
    private const string SampleMarkdown = """
        Here is a short paragraph of prose representing a typical AI chat turn, with **bold**,
        *italic*, and `inline code` mixed in for realism.

        | Metric | Value |
        |---|---|
        | Latency | 120ms |
        | Tokens | 342 |
        | Status | OK |

        - first point
        - second point
          - nested point
        - third point
        """;

    /// <summary>
    /// Initialise
    /// </summary>
    public FormMarkdownTextBoxDemo()
    {
        InitializeComponent();
    }

    private void FormMarkdownTextBoxDemo_Load(object sender, EventArgs e)
    {
        labelInfo.Text = SystemInfoHelper.GetSystemInfo();
        markdownTextBox.SetMarkdown(SampleMarkdown);
        labelPreferredHeight.Text = $"Preferred content height: {markdownTextBox.GetPreferredContentHeight()}px";
    }
}
