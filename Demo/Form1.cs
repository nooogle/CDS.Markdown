using System.Runtime.InteropServices;

namespace Demo;


/// <summary>
/// Demo form for displaying Markdown content.
/// </summary>
public partial class Form1 : Form
{
    /// <summary>
    /// Initialise
    /// </summary>
    public Form1()
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
        labelInfo.Text = GenerateSystemInfo();
    }


    /// <summary>
    /// Gets a string containing information about the system and application.
    /// </summary>
    private static string GenerateSystemInfo()
    {
        string appName = Application.ProductName!;
        string appVersion = Application.ProductVersion.Split('+')[0]; // Remove hash if present

        string appBitDepth = Environment.Is64BitProcess ? "64-bit" : "32-bit";
        string appArchitecture = RuntimeInformation.ProcessArchitecture.ToString();
        string appFramework = RuntimeInformation.FrameworkDescription;

        string osVersion = Environment.OSVersion.VersionString;
        string osBitDepth = Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit";
        string osArchitecture = RuntimeInformation.OSArchitecture.ToString();

        return
            $"Application: {appName} [{appVersion}] " +
            $"running as {appBitDepth} {appArchitecture} " +
            $"using {appFramework} " +
            $"on {osVersion} {osBitDepth} and {osArchitecture} processor";
    }
}
