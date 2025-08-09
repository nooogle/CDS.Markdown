namespace CDS.Markdown;

/// <summary>
/// Manages the creation, tracking, and cleanup of temporary HTML files.
/// </summary>
public class TempHtmlFileManager : IDisposable
{
    private readonly List<string> tempFiles = new();
    private bool disposed;

    /// <summary>
    /// Creates a new temporary HTML file with the specified content and tracks it for cleanup.
    /// </summary>
    /// <param name="htmlContent">The HTML content to write to the file.</param>
    /// <returns>The full path to the created temp file.</returns>
    public string CreateTempHtmlFile(string htmlContent)
    {
        string tempHtmlPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".html");
        File.WriteAllText(tempHtmlPath, htmlContent);
        tempFiles.Add(tempHtmlPath);
        return tempHtmlPath;
    }

    /// <summary>
    /// Deletes all tracked temporary files.
    /// </summary>
    public void Cleanup()
    {
        foreach (var tempFile in tempFiles)
        {
            try
            {
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
            catch
            {
                // Ignore errors
            }
        }
        tempFiles.Clear();
    }

    /// <summary>
    /// Disposes the manager and cleans up all tracked temp files.
    /// </summary>
    public void Dispose()
    {
        if (!disposed)
        {
            Cleanup();
            disposed = true;
        }
    }
}
