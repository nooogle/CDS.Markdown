using System;
using CDS.Markdown;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests;

[TestClass]
public class MarkdownDocumentBuilderTests
{
    [TestMethod]
    public void AddHeading_ShouldAddHeadingWithCorrectLevel()
    {
        // Arrange
        var builder = new MarkdownDocumentBuilder();

        // Act
        builder.AddHeading("Title");
        builder.AddHeading("Subtitle", 2);
        var markdown = builder.Build();

        // Assert
        markdown.Should().Contain("# Title");
        markdown.Should().Contain("## Subtitle");
    }

    [TestMethod]
    public void AddParagraph_ShouldAddParagraph()
    {
        // Arrange
        var builder = new MarkdownDocumentBuilder();

        // Act
        builder.AddParagraph("This is a paragraph.");
        var markdown = builder.Build();

        // Assert
        markdown.Should().Contain("This is a paragraph.");
    }

    [TestMethod]
    public void AddCodeBlock_ShouldAddCodeBlockWithLanguage()
    {
        // Arrange
        var builder = new MarkdownDocumentBuilder();

        // Act
        builder.AddCodeBlock("var x = 1;", "csharp");
        var markdown = builder.Build();

        // Assert
        markdown.Should().Contain("```csharp");
        markdown.Should().Contain("var x = 1;");
        markdown.Should().Contain("```");
    }

    [TestMethod]
    public void AddBulletList_ShouldAddBulletList()
    {
        // Arrange
        var builder = new MarkdownDocumentBuilder();

        // Act
        builder.AddBulletList(["Item 1", "Item 2"]);
        var markdown = builder.Build();

        // Assert
        markdown.Should().Contain("- Item 1");
        markdown.Should().Contain("- Item 2");
    }

    [TestMethod]
    public void AddImage_ShouldAddImage()
    {
        // Arrange
        var builder = new MarkdownDocumentBuilder();

        // Act
        builder.AddImage("Alt text", "image.png");
        var markdown = builder.Build();

        // Assert
        markdown.Should().Contain("![Alt text](image.png)");
    }

    [TestMethod]
    public void Build_ShouldBuildCompleteDocument()
    {
        // Arrange
        var builder = new MarkdownDocumentBuilder();

        // Act
        builder.AddHeading("Main Title");
        builder.AddParagraph("Intro");
        builder.AddBulletList(["A", "B"]);
        var markdown = builder.Build();

        // Assert
        markdown.Should().Contain("# Main Title");
        markdown.Should().Contain("Intro");
        markdown.Should().Contain("- A");
        markdown.Should().Contain("- B");
    }
}
