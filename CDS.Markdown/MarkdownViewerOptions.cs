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

    /// <summary>
    /// Gets or sets the theme used for rendering.
    /// Defaults to System.
    /// </summary>
    public MarkdownViewerTheme Theme { get; set; } = MarkdownViewerTheme.System;

    /// <summary>
    /// Gets or sets whether the Home/Back/Forward navigation toolbar is shown above the
    /// rendered content. Defaults to <c>true</c>, matching the viewer's original behaviour.
    /// Set to <c>false</c> when embedding the viewer as a fragment inside a larger UI (e.g. a
    /// chat message bubble) that has no "home" document and nowhere to navigate to/from.
    /// </summary>
    public bool ShowNavigationToolbar { get; set; } = true;
}
