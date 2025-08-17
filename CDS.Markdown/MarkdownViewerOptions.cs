namespace CDS.Markdown;

/// <summary>
/// Represents configuration options for the Markdown viewer, including user data storage,
/// profile management, and browser executable location.
/// </summary>
public sealed class MarkdownViewerOptions
{
    /// <summary>
    /// Gets or sets the path to the folder where user-specific data is stored.
    /// This may include cache, settings, or other persistent data.
    /// </summary>
    public string? UserDataFolder { get; set; }

    /// <summary>
    /// Gets or sets the name of the user profile to use for the Markdown viewer.
    /// Defaults to "Default" if not specified.
    /// </summary>
    public string? ProfileName { get; set; } = "Default";

    /// <summary>
    /// Gets or sets the path to the folder containing the browser executable.
    /// This is used if a custom browser location is required.
    /// </summary>
    public string? BrowserExecutableFolder { get; set; }
}
