using CDS.Markdown;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests;

[TestClass]
public class MarkdownHtmlDocumentBuilderTests
{
    [TestMethod]
    public void Build_ShouldWrapHtmlWithCssAndBaseHref()
    {
        // Arrange
        var githubCss = ".markdown-body { color: red; }";
        var defaultCss = "body { background: white; }";
        var script = "<script>console.log('test');</script>";
        var builder = new MarkdownHtmlDocumentBuilder(githubCss, defaultCss, script);
        var htmlBody = "<h1>Title</h1>";
        var baseHref = "<base href=\"file:///C:/docs/\">";

        // Act
        var html = builder.Build(htmlBody, baseHref);

        // Assert
        html.Should().Contain(githubCss)
            .And.Contain(defaultCss)
            .And.Contain(script)
            .And.Contain(baseHref)
            .And.Contain(htmlBody)
            .And.Contain("<div class=\"markdown-body\">");
    }

    [TestMethod]
    public void Build_ShouldHandleEmptyBody()
    {
        // Arrange
        var builder = new MarkdownHtmlDocumentBuilder("", "", "");
        var baseHref = "<base href=\"/\">";

        // Act
        var html = builder.Build(string.Empty, baseHref);

        // Assert
        html.Should().Contain(baseHref)
            .And.Contain("<div class=\"markdown-body\">");
    }

    [TestMethod]
    public void Build_ShouldIncludeMathJaxWhenProvided()
    {
        // Arrange
        var mathJaxBundle = "console.log('mathjax');";
        var mathJaxInit = "<script>initMathJax();</script>";
        var builder = new MarkdownHtmlDocumentBuilder("", "", "", null, null, mathJaxBundle, mathJaxInit);

        // Act
        var html = builder.Build("body", "<base href=\"/\">");

        // Assert
        html.Should().Contain(mathJaxBundle)
            .And.Contain(mathJaxInit);
    }
}
