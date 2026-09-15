using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.Core.Tools;
using AwesomeAssertions;

namespace UiTests;

/// <summary>
/// Drives the real <c>Demo.exe</c> (via FlaUI) to confirm <see cref="CDS.Markdown.MarkdownViewer"/>
/// behaves correctly end-to-end: real WinForms layout, a real WebView2, real rendered content.
/// Complements the CDS.Markdown/UnitTests project, which covers the same features at the
/// class level (HTML string assertions, mocked message plumbing) without a running UI.
/// </summary>
[TestClass]
public class MarkdownViewerUiTests
{
    private DemoAppLauncher? launcher;

    [TestCleanup]
    public void Cleanup() => launcher?.Dispose();

    [TestMethod]
    public void WikiDemo_DefaultOptions_ShowsNavigationToolbarButtons()
    {
        // Arrange & Act
        launcher = DemoAppLauncher.Launch("--wiki");
        var window = launcher.GetMainWindow();

        // Assert
        window.Title.Should().Be("CDS.Markdown demo");
        FindButton(window, "btnHome").Should().NotBeNull();
        FindButton(window, "btnBack").Should().NotBeNull();
        FindButton(window, "btnForward").Should().NotBeNull();
    }

    [TestMethod]
    public void WikiDemo_ShowNavigationToolbarFalse_HidesToolbarButtons()
    {
        // Arrange & Act
        launcher = DemoAppLauncher.Launch("--wiki-no-toolbar");
        var window = launcher.GetMainWindow();

        // Assert: panel1 (and its buttons) is excluded from WinForms dock layout while
        // Visible=false, and FlaUI/UIA does not surface not-visible WinForms controls either.
        FindButton(window, "btnHome").Should().BeNull();
        FindButton(window, "btnBack").Should().BeNull();
        FindButton(window, "btnForward").Should().BeNull();
    }

    [TestMethod]
    public void WikiDemo_AfterLoad_ReportsPositiveContentHeight()
    {
        // Arrange
        launcher = DemoAppLauncher.Launch("--wiki");
        var window = launcher.GetMainWindow();

        // Act: labelContentHeight's text is updated from MarkdownViewer.ContentHeightChanged
        // (see FormWikiDemo), which only fires once the WebView2 has actually rendered content.
        // Timeout matches DemoAppLauncher.GetMainWindow's - see its remarks on CI cold-start cost.
        Retry.WhileTrue(
            () => GetContentHeightLabelText(window).Contains("(pending)"),
            timeout: TimeSpan.FromSeconds(45),
            interval: TimeSpan.FromMilliseconds(250));
        var labelText = GetContentHeightLabelText(window);

        // Assert
        var heightText = labelText
            .Replace("Content height:", string.Empty)
            .Replace("px", string.Empty)
            .Trim();
        int.Parse(heightText).Should().BePositive();
    }

    private static string GetContentHeightLabelText(Window window)
    {
        var label = window.FindFirstDescendant(cf => cf.ByAutomationId("labelContentHeight"))?.AsLabel();
        label.Should().NotBeNull(because: "FormWikiDemo always creates labelContentHeight");
        return label!.Text;
    }

    private static AutomationElement? FindButton(Window window, string automationId) =>
        window.FindFirstDescendant(cf => cf.ByAutomationId(automationId).And(cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button)));
}

/// <summary>
/// Drives the real <c>Demo.exe</c> to confirm <see cref="CDS.Markdown.MarkdownTextBox"/> renders
/// content and reports a real preferred height in a real, painted window - the lightweight
/// alternative to <see cref="CDS.Markdown.MarkdownViewer"/>.
/// </summary>
[TestClass]
public class MarkdownTextBoxUiTests
{
    private DemoAppLauncher? launcher;

    [TestCleanup]
    public void Cleanup() => launcher?.Dispose();

    [TestMethod]
    public void TextBoxDemo_AfterLoad_RendersMarkdownAndReportsPositiveHeight()
    {
        // Arrange & Act
        launcher = DemoAppLauncher.Launch("--textbox");
        var window = launcher.GetMainWindow();

        // Assert: window loads synchronously (no WebView2/async render to wait for), so the
        // content and the preferred-height label are both already set by the time it appears.
        window.Title.Should().Be("CDS.Markdown - MarkdownTextBox demo");

        var textBox = window.FindFirstDescendant(cf => cf.ByAutomationId("markdownTextBox"));
        textBox.Should().NotBeNull();
        var text = textBox!.Patterns.Text.PatternOrDefault?.DocumentRange.GetText(int.MaxValue)
            ?? textBox.Patterns.Value.PatternOrDefault?.Value.ValueOrDefault;
        text.Should().NotBeNullOrWhiteSpace();
        text.Should().Contain("Latency").And.Contain("nested point");

        var heightLabel = window.FindFirstDescendant(cf => cf.ByAutomationId("labelPreferredHeight"))?.AsLabel();
        heightLabel.Should().NotBeNull();
        var heightText = heightLabel!.Text.Replace("Preferred content height:", string.Empty).Replace("px", string.Empty).Trim();
        int.Parse(heightText).Should().BePositive();
    }
}
