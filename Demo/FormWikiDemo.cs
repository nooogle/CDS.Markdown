namespace Demo;


/// <summary>
/// Demo form for displaying Markdown content.
/// </summary>
public partial class FormWikiDemo : Form
{
    /// <summary>
    /// Initialise, with the navigation toolbar shown.
    /// </summary>
    public FormWikiDemo() : this(showNavigationToolbar: true)
    {
    }

    /// <summary>
    /// Initialise.
    /// </summary>
    /// <param name="showNavigationToolbar">
    /// Whether the Home/Back/Forward toolbar is shown. Exposed here so UI-automation
    /// tests (see UiTests) can drive both states of <see cref="CDS.Markdown.MarkdownViewerOptions.ShowNavigationToolbar"/>
    /// via <c>Demo/Program.cs</c> command-line arguments without needing a UI to toggle it.
    /// </param>
    public FormWikiDemo(bool showNavigationToolbar)
    {
        InitializeComponent();
        markdownViewer.Options.ShowNavigationToolbar = showNavigationToolbar;
        markdownViewer.ContentHeightChanged += (_, height) =>
            labelContentHeight.Text = $"Content height: {height}px";
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
