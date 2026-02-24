# Markdown Viewer Guide

The `MarkdownViewer` is a WinForms UserControl that renders Markdown using Markdig and WebView2.

## Setup Instructions

1. Use the NuGet package manager to install the `CDS.Markdown` package in your WinForms project.
2. Add a `MarkdownViewer` control to your form.

![Toolbox](../readme_images/toolbox.png){width=300px}

3. Create a Markdown file in your project directory, for example `readme.md`.

![Sln Exp Readme](../readme_images/sln_exp_readme.png){width=300px}

4. Set the Markdown file's **Copy to Output Directory** to `Copy if newer`.

![Readme Props](../readme_images/readme_props.png){width=300px}

![Form](../readme_images/form.png){width=400px}

5. In your form's code, call the `LoadMarkdownAsync()` method on the `MarkdownViewer` control, passing the path to your Markdown file:

```csharp
protected async override void OnShown(EventArgs e)
{
    base.OnShown(e);
    await markdownViewer1.LoadMarkdownAsync("readme.md");
}
```

## Tips

> **Tip 1:** The viewer expects any linked Markdown files to be in the same directory as the executable, or a subdirectory of it. See the `Demo` project for an example of using subdirectories.

> **Tip 2:** The project uses the WebView2 control which causes compile-time warnings around version conflicts with the `WindowsBase` assembly. The following property can be added to your `.csproj` file to suppress these warnings:
```xml
<PropertyGroup>
    <NoWarn>$(NoWarn);MSB3277</NoWarn>
</PropertyGroup>
```

## Advanced Rendering

The viewer automatically supports:
- **GitHub Flavored Markdown**: Styled using embedded `github-markdown-css`.
- **Mermaid Diagrams**: Rendered offline using an embedded `mermaid.min.js`.
- **Mathematics**: LaTeX-style math blocks (`$$`) rendered offline using an embedded MathJax SVG engine.
