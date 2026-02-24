using System;
using CDS.Markdown;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests;

[TestClass]
public class FluentMarkdownDocumentTests
{
    [TestMethod]
    public void AddHeading_ShouldAddHeadingWithCorrectLevel()
    {
        // Arrange
        var document = new FluentMarkdownDocument();

        // Act
        var markdown = document
            .AddHeading("Title")
            .AddHeading("Subtitle", 2)
            .ToMarkdown();

        // Assert
        markdown.Should().Contain("# Title");
        markdown.Should().Contain("## Subtitle");
    }

    [TestMethod]
    public void AddParagraph_ShouldAddParagraph()
    {
        // Arrange
        var document = new FluentMarkdownDocument();

        // Act
        var markdown = document
            .AddParagraph("This is a paragraph.")
            .ToMarkdown();

        // Assert
        markdown.Should().Contain("This is a paragraph.");
    }

    [TestMethod]
    public void AddCodeBlock_ShouldAddCodeBlockWithLanguage()
    {
        // Arrange
        var document = new FluentMarkdownDocument();

        // Act
        var markdown = document
            .AddCodeBlock("var x = 1;", "csharp")
            .ToMarkdown();

        // Assert
        markdown.Should().Contain("```csharp");
        markdown.Should().Contain("var x = 1;");
        markdown.Should().Contain("```");
    }

    [TestMethod]
    public void AddBulletList_ShouldAddBulletList()
    {
        // Arrange
        var document = new FluentMarkdownDocument();

        // Act
        var markdown = document
            .AddBulletList(["Item 1", "Item 2"])
            .ToMarkdown();

        // Assert
        markdown.Should().Contain("- Item 1");
        markdown.Should().Contain("- Item 2");
    }

    [TestMethod]
    public void AddImage_ShouldAddImage()
    {
        // Arrange
        var document = new FluentMarkdownDocument();

        // Act
        var markdown = document
            .AddImage("Alt text", "image.png")
            .ToMarkdown();

        // Assert
        markdown.Should().Contain("![Alt text](image.png)");
    }

    [TestMethod]
    public void AddMermaidDiagram_ShouldAddMermaidCodeBlock()
    {
        // Arrange
        var document = new FluentMarkdownDocument();

        // Act
        var markdown = document
            .AddMermaidDiagram("graph TD;\nA-->B;")
            .ToMarkdown();

        // Assert
        markdown.Should().Contain("```mermaid");
        markdown.Should().Contain("graph TD;\nA-->B;");
        markdown.Should().Contain("```");
    }

    [TestMethod]
    public void Chaining_ShouldBuildCompleteDocument()
    {
        // Arrange
        var document = new FluentMarkdownDocument();

        // Act
        var markdown = document
            .AddHeading("Main Title")
            .AddParagraph("Intro")
            .AddBulletList(["A", "B"])
            .ToMarkdown();

        // Assert
        markdown.Should().Contain("# Main Title");
        markdown.Should().Contain("Intro");
        markdown.Should().Contain("- A");
        markdown.Should().Contain("- B");
    }
}
