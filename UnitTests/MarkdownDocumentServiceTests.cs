using CDS.Markdown;
using FluentAssertions;

namespace UnitTests;

[TestClass]
public class MarkdownDocumentServiceTests
{
    private string? tempFilePath;

    [TestInitialize]
    public void Setup()
    {
        tempFilePath = Path.GetTempFileName();
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (File.Exists(tempFilePath))
        {
            File.Delete(tempFilePath);
        }
    }

    [TestMethod]
    public async Task BuildHtmlFromMarkdownFileAsync_ShouldReturnHtmlDocument()
    {
        if(tempFilePath == null)
        {
            throw new InvalidOperationException("Temporary file path is not initialized.");
        }

        // Arrange
        var markdown = "# Test";
        await TestFileHelper.WriteAllTextAsync(tempFilePath, markdown);
        var service = new MarkdownDocumentService();
        var baseHref = "<base href=\"/\">";

        // Act
        var html = await service.BuildHtmlFromMarkdownFileAsync(tempFilePath, baseHref);

        // Assert
        html.Should().Contain("<h1").And.Contain("Test").And.Contain(baseHref);
    }

    [TestMethod]
    public async Task BuildHtmlFromMarkdownFileAsync_ShouldThrowIfFileMissing()
    {
        // Arrange
        var service = new MarkdownDocumentService();
        var missingFile = tempFilePath + ".missing";
        var baseHref = "<base href=\"/\">";

        // Act
        Func<Task> act = async () => await service.BuildHtmlFromMarkdownFileAsync(missingFile, baseHref);

        // Assert
        await act.Should().ThrowAsync<FileNotFoundException>();
    }
}
