using System;
using System.IO;
using System.Threading.Tasks;
using CDS.Markdown;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests;

[TestClass]
public class MarkdownViewerSessionTests
{
    private string? tempFilePath;
    private MarkdownViewerSession? session;
    private string? lastHtml;

    [TestInitialize]
    public void Setup()
    {
        tempFilePath = Path.GetTempFileName();
        session = new MarkdownViewerSession();
        lastHtml = null;
        session.HtmlReady += html => { lastHtml = html; return Task.CompletedTask; };
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (File.Exists(tempFilePath!))
        {
            File.Delete(tempFilePath);
        }
    }

    [TestMethod]
    public async Task NavigateToAsync_ShouldSetCurrentMarkdownPathAndRaiseHtmlReady()
    {
        // Arrange
        var markdown = "# Session Test";
        await TestFileHelper.WriteAllTextAsync(tempFilePath!, markdown);

        // Act
        await session!.NavigateToAsync(tempFilePath!);

        // Assert
        session.CurrentMarkdownPath.Should().Be(tempFilePath);
        lastHtml.Should().Contain("Session Test");
    }

    [TestMethod]
    public async Task GoHomeAsync_ShouldNavigateToHomeMarkdown()
    {
        // Arrange
        var markdown = "# Home Test";
        await TestFileHelper.WriteAllTextAsync(tempFilePath!, markdown);
        await session!.NavigateToAsync(tempFilePath!, setHome: true);
        lastHtml = null;

        // Act
        await session.GoHomeAsync();

        // Assert
        lastHtml.Should().Contain("Home Test");
    }

    [TestMethod]
    public async Task HandleMarkdownLinkAsync_ShouldNavigateToLinkedFile()
    {
        // Arrange
        var markdown = "# Link Test";
        await TestFileHelper.WriteAllTextAsync(tempFilePath!, markdown);
        await session!.NavigateToAsync(tempFilePath!, setHome: true);
        var linkedFile = Path.Combine(Path.GetDirectoryName(tempFilePath!)!, "linked.md");
        await TestFileHelper.WriteAllTextAsync(linkedFile, "# Linked");
        lastHtml = null;

        try
        {
            // Act
            await session.HandleMarkdownLinkAsync("linked.md");

            // Assert
            session.CurrentMarkdownPath.Should().Be(linkedFile);
            lastHtml.Should().Contain("Linked");
        }
        finally
        {
            if (File.Exists(linkedFile)) File.Delete(linkedFile);
        }
    }

    [TestMethod]
    public async Task HandleNavigationUriAsync_ShouldInterceptMdFileAndNavigate()
    {
        // Arrange
        var markdown = "# Nav Test";
        await TestFileHelper.WriteAllTextAsync(tempFilePath!, markdown);
        await session!.NavigateToAsync(tempFilePath!, setHome: true);
        var linkedFile = Path.Combine(Path.GetDirectoryName(tempFilePath!)!, "nav.md");
        await TestFileHelper.WriteAllTextAsync(linkedFile, "# NavLinked");
        lastHtml = null;
        bool cancelled = false;
        var uri = new Uri(linkedFile).AbsoluteUri;

        try
        {
            // Act
            await session.HandleNavigationUriAsync(uri, () => cancelled = true);

            // Assert
            cancelled.Should().BeTrue();
            session.CurrentMarkdownPath.Should().Be(linkedFile);
            lastHtml.Should().Contain("NavLinked");
        }
        finally
        {
            if (File.Exists(linkedFile)) File.Delete(linkedFile);
        }
    }
}
