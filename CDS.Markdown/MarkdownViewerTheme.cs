namespace CDS.Markdown;

/// <summary>
/// Specifies the theme used for rendering Markdown content.
/// </summary>
public enum MarkdownViewerTheme
{
    /// <summary>
    /// The default theme, determined by the system preferences.
    /// </summary>
    System,

    /// <summary>
    /// Explicitly forces a light theme.
    /// </summary>
    Light,

    /// <summary>
    /// Explicitly forces a dark theme.
    /// </summary>
    Dark
}