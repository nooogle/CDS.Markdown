namespace Demo;


/// <summary>
/// Demo form for displaying Markdown content.
/// </summary>
public partial class FormWikiDemo : Form
{
    /// <summary>
    /// Initialise
    /// </summary>
    public FormWikiDemo()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Load a markdown file when the form is shown.
    /// </summary>
    protected override async void OnShown(EventArgs e)
    {
        base.OnShown(e);

        try
        {
            await markdownViewer.LoadMarkdownAsync(@"wiki/index.md");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load markdown: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        labelInfo.Text = SystemInfoHelper.GetSystemInfo();
    }
}
