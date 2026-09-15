using System.Reflection;
using System.Windows.Forms;
using CDS.Markdown;
using AwesomeAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests;

[TestClass]
public class MarkdownViewerTests
{
    [TestMethod]
    public void Constructor_DefaultOptions_ShowsNavigationToolbar()
    {
        // Arrange & Act
        using var viewer = new MarkdownViewer();

        // Assert
        viewer.Options.ShowNavigationToolbar.Should().BeTrue();
        GetToolbarPanel(viewer).Visible.Should().BeTrue();
    }

    [TestMethod]
    public void ApplyToolbarVisibility_ShowNavigationToolbarFalse_HidesToolbarPanel()
    {
        // Arrange
        using var viewer = new MarkdownViewer();
        viewer.Options.ShowNavigationToolbar = false;

        // Act
        InvokeApplyToolbarVisibility(viewer);

        // Assert
        GetToolbarPanel(viewer).Visible.Should().BeFalse();
    }

    /// <summary>
    /// The toolbar panel is a designer-generated private field with no public accessor,
    /// so tests reach it via reflection rather than exposing new public surface purely
    /// for testability.
    /// </summary>
    private static Panel GetToolbarPanel(MarkdownViewer viewer)
    {
        var field = typeof(MarkdownViewer).GetField("panel1", BindingFlags.NonPublic | BindingFlags.Instance);
        field.Should().NotBeNull();
        return (Panel)field!.GetValue(viewer)!;
    }

    private static void InvokeApplyToolbarVisibility(MarkdownViewer viewer)
    {
        var method = typeof(MarkdownViewer).GetMethod("ApplyToolbarVisibility", BindingFlags.NonPublic | BindingFlags.Instance);
        method.Should().NotBeNull();
        method!.Invoke(viewer, null);
    }
}
