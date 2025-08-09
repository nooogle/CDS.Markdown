using System;
using System.IO;
using CDS.Markdown;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests;

[TestClass]
public class TempHtmlFileManagerTests
{
    [TestMethod]
    public void CreateTempHtmlFile_ShouldCreateFileAndTrackIt()
    {
        // Arrange
        var manager = new TempHtmlFileManager();
        var html = "<html><body>test</body></html>";

        // Act
        var path = manager.CreateTempHtmlFile(html);

        // Assert
        File.Exists(path).Should().BeTrue();
        File.ReadAllText(path).Should().Be(html);

        // Cleanup
        manager.Dispose();
        File.Exists(path).Should().BeFalse();
    }

    [TestMethod]
    public void Cleanup_ShouldDeleteAllTrackedFiles()
    {
        // Arrange
        var manager = new TempHtmlFileManager();
        var path1 = manager.CreateTempHtmlFile("one");
        var path2 = manager.CreateTempHtmlFile("two");

        // Act
        manager.Cleanup();

        // Assert
        File.Exists(path1).Should().BeFalse();
        File.Exists(path2).Should().BeFalse();
    }
}
