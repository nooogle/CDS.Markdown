using System;
using CDS.Markdown;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests;

[TestClass]
public class MarkdownRendererTests
{
    [TestMethod]
    public void RenderToHtml_ShouldRenderBasicMarkdown()
    {
        // Arrange
        var renderer = new MarkdownRenderer();
        var markdown = "# Hello World";

        // Act
        var html = renderer.RenderToHtml(markdown);

        // Assert
        html.Should().Contain("<h1").And.Contain("Hello World");
    }

    [TestMethod]
    public void RenderToHtml_ShouldHandleEmptyString()
    {
        // Arrange
        var renderer = new MarkdownRenderer();
        var markdown = string.Empty;

        // Act
        var html = renderer.RenderToHtml(markdown);

        // Assert
        html.Should().BeEmpty();
    }

    [TestMethod]
    public void RenderToHtml_ShouldSupportAdvancedExtensions()
    {
        // Arrange
        var renderer = new MarkdownRenderer();
        var markdown = "- [x] Task done";

        // Act
        var html = renderer.RenderToHtml(markdown);

        // Assert
        html.Should().Contain("<input").And.Contain("\"checkbox\"");
    }
}
