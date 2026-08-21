using System.Drawing;

using CDS.Markdown;
using FluentAssertions;

namespace UnitTests;

/// <summary>
/// <see cref="RichTextBox"/> creates a native window on first use, and that native call is not
/// safe to make concurrently from multiple threads. With the assembly's
/// <c>[Parallelize(Scope = ExecutionScope.MethodLevel)]</c>, letting this class's tests run
/// alongside others crashed the test host (access violation inside RichTextBox.StreamIn) rather
/// than merely failing - <c>[DoNotParallelize]</c> keeps them serial.
/// </summary>
[TestClass]
[DoNotParallelize]
public class MarkdownTextBoxTests
{
    [TestMethod]
    public void SetMarkdown_Heading_RendersPlainText()
    {
        // Arrange
        using var textBox = new MarkdownTextBox();

        // Act
        textBox.SetMarkdown("# Title");

        // Assert
        textBox.Text.Should().Contain("Title");
    }

    [TestMethod]
    public void SetMarkdown_BoldAndItalic_RendersPlainText()
    {
        // Arrange
        using var textBox = new MarkdownTextBox();

        // Act
        textBox.SetMarkdown("This is **bold** and *italic* text.");

        // Assert
        textBox.Text.Should().Contain("This is bold and italic text.");
    }

    [TestMethod]
    public void SetMarkdown_NestedList_RendersAllItems()
    {
        // Arrange
        using var textBox = new MarkdownTextBox();

        // Act
        textBox.SetMarkdown("""
            - first
              - nested
            - second
            """);

        // Assert
        textBox.Text.Should().Contain("first").And.Contain("nested").And.Contain("second");
    }

    [TestMethod]
    public void SetMarkdown_NestedListItem_IsIndentedFurtherThanItsParent()
    {
        // Arrange
        using var textBox = new MarkdownTextBox();
        textBox.SetMarkdown("""
            - first point
            - second point
              - nested point
            - third point
            """);

        // Act
        textBox.SelectionStart = textBox.Text.IndexOf("second point", StringComparison.Ordinal);
        textBox.SelectionLength = 0;
        var parentIndent = textBox.SelectionIndent;

        textBox.SelectionStart = textBox.Text.IndexOf("nested point", StringComparison.Ordinal);
        textBox.SelectionLength = 0;
        var nestedIndent = textBox.SelectionIndent;

        // Assert: regression test for the nested item's SelectionIndent being applied and then
        // immediately overwritten by its parent item's own (shallower) indent, which used to
        // collapse every nested list back to zero indentation.
        nestedIndent.Should().BeGreaterThan(parentIndent);

        // Sibling items sharing the parent's depth must be unaffected by the fix.
        textBox.SelectionStart = textBox.Text.IndexOf("third point", StringComparison.Ordinal);
        textBox.SelectionLength = 0;
        textBox.SelectionIndent.Should().Be(parentIndent);
    }

    [TestMethod]
    public void SetMarkdown_Table_RendersCellsAsMonospacedGrid()
    {
        // Arrange
        using var textBox = new MarkdownTextBox();

        // Act
        textBox.SetMarkdown("""
            | Name | Value |
            |---|---|
            | Alpha | 1 |
            | Beta | 2 |
            """);

        // Assert
        textBox.Text.Should().Contain("Name").And.Contain("Value")
            .And.Contain("Alpha").And.Contain("1")
            .And.Contain("Beta").And.Contain("2");
    }

    [TestMethod]
    public void SetMarkdown_CodeBlock_RendersCodeContent()
    {
        // Arrange
        using var textBox = new MarkdownTextBox();

        // Act
        textBox.SetMarkdown("""
            ```csharp
            var x = 1;
            ```
            """);

        // Assert
        textBox.Text.Should().Contain("var x = 1;");
    }

    [TestMethod]
    public void SetMarkdown_NullOrWhitespace_ClearsControl()
    {
        // Arrange
        using var textBox = new MarkdownTextBox();
        textBox.SetMarkdown("# Something");

        // Act
        textBox.SetMarkdown(null);

        // Assert
        textBox.Text.Should().BeEmpty();
    }

    [TestMethod]
    public void SetMarkdown_ReplacesPreviousContent()
    {
        // Arrange
        using var textBox = new MarkdownTextBox();
        textBox.SetMarkdown("# First");

        // Act
        textBox.SetMarkdown("# Second");

        // Assert
        textBox.Text.Should().Contain("Second").And.NotContain("First");
    }

    [TestMethod]
    public void AppendMarkdown_ToEmptyControl_RendersContent()
    {
        // Arrange
        using var textBox = new MarkdownTextBox();

        // Act
        textBox.AppendMarkdown("# First");

        // Assert
        textBox.Text.Should().Contain("First");
    }

    [TestMethod]
    public void AppendMarkdown_AfterExistingContent_KeepsBothAndAddsSeparation()
    {
        // Arrange
        using var textBox = new MarkdownTextBox();
        textBox.SetMarkdown("First turn.");

        // Act
        textBox.AppendMarkdown("Second turn.");

        // Assert
        textBox.Text.Should().Contain("First turn.").And.Contain("Second turn.");
        textBox.Text.IndexOf("First turn.", StringComparison.Ordinal)
            .Should().BeLessThan(textBox.Text.IndexOf("Second turn.", StringComparison.Ordinal));
        textBox.Text.Should().Contain("First turn.\n\nSecond turn.");
    }

    [TestMethod]
    public void AppendMarkdown_NullOrWhitespace_LeavesExistingContentUnchanged()
    {
        // Arrange
        using var textBox = new MarkdownTextBox();
        textBox.SetMarkdown("First turn.");

        // Act
        textBox.AppendMarkdown(null);

        // Assert
        textBox.Text.Should().Be("First turn.");
    }

    [TestMethod]
    public void AppendMarkdown_AfterSetMarkdown_DoesNotReplacePreviousContent()
    {
        // Arrange: this is what SetMarkdown replacing content is explicitly for, so
        // AppendMarkdown must not fall back to that behaviour.
        using var textBox = new MarkdownTextBox();
        textBox.SetMarkdown("# First");

        // Act
        textBox.AppendMarkdown("# Second");

        // Assert
        textBox.Text.Should().Contain("First").And.Contain("Second");
    }

    [TestMethod]
    public void AppendPlainText_IsNotParsedAsMarkdown()
    {
        // Arrange
        using var textBox = new MarkdownTextBox();

        // Act
        textBox.AppendPlainText("**not bold**");

        // Assert
        textBox.Text.Should().Contain("**not bold**");
    }

    [TestMethod]
    public void AppendPlainText_AfterMarkdown_AppendsRatherThanReplaces()
    {
        // Arrange
        using var textBox = new MarkdownTextBox();
        textBox.SetMarkdown("# Diff for review");

        // Act
        textBox.AppendPlainText("+ added line");
        textBox.AppendPlainText("- removed line");

        // Assert
        textBox.Text.Should().Contain("Diff for review")
            .And.Contain("+ added line")
            .And.Contain("- removed line");
    }

    [TestMethod]
    public void AppendPlainText_SetsRequestedBackColor()
    {
        // Arrange
        using var textBox = new MarkdownTextBox();
        textBox.AppendPlainText("added line", Color.LightGreen);

        // Act
        textBox.SelectionStart = textBox.Text.IndexOf("added line", StringComparison.Ordinal);
        textBox.SelectionLength = 1;

        // Assert
        textBox.SelectionBackColor.Should().Be(Color.LightGreen);
    }

    [TestMethod]
    public void GetPreferredContentHeight_EmptyControl_ReturnsZero()
    {
        // Arrange
        using var textBox = new MarkdownTextBox();

        // Act
        var height = textBox.GetPreferredContentHeight();

        // Assert
        height.Should().Be(0);
    }

    [TestMethod]
    public void GetPreferredContentHeight_WithContent_ReturnsPositiveValue()
    {
        // Arrange
        using var textBox = new MarkdownTextBox();
        textBox.SetMarkdown("# Title\n\nSome body text.");

        // Act
        var height = textBox.GetPreferredContentHeight();

        // Assert
        height.Should().BePositive();
    }

    [TestMethod]
    public void GetPreferredContentHeight_MoreContent_IsTallerThanLessContent()
    {
        // Arrange
        using var shortBox = new MarkdownTextBox();
        shortBox.SetMarkdown("One line.");
        using var longBox = new MarkdownTextBox();
        longBox.SetMarkdown(string.Join("\n\n", Enumerable.Range(1, 10).Select(i => $"Paragraph {i}.")));

        // Act
        var shortHeight = shortBox.GetPreferredContentHeight();
        var longHeight = longBox.GetPreferredContentHeight();

        // Assert
        longHeight.Should().BeGreaterThan(shortHeight);
    }
}
