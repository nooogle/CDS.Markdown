# CDS.Markdown

A comprehensive .NET library for rendering and generating Markdown in WinForms applications.

**Compatible with:**
- .NET 8
- .NET 10

## Features
- **WinForms Viewer Control**: Render Markdown files using [Markdig](https://github.com/lunet-io/markdig) and [WebView2](https://learn.microsoft.com/en-us/microsoft-edge/webview2/).
- **Programmatic Creation**: Generate Markdown dynamically using a clean **Fluent API** or a traditional **Builder API**.
- **Offline-First**: Embedded resources for GitHub-style CSS, Mermaid.js diagrams, and MathJax (LaTeX math) mean no internet connection is required.

## Getting Started

### 1. Install the NuGet package:
```shell
dotnet add package CDS.Markdown
```

### 2. Viewing Markdown
Add the `MarkdownViewer` control to your form:
```csharp
using CDS.Markdown;
// ...
var viewer = new MarkdownViewer();
await viewer.LoadMarkdownAsync("README.md");
this.Controls.Add(viewer);
```

### 3. Generating Markdown
Create structured Markdown programmatically:
```csharp
using CDS.Markdown;

var markdown = new FluentMarkdownDocument()
    .AddHeading("Hello, World!")
    .AddParagraph("This is a **bold** statement.")
    .AddBulletList(["First item", "Second item"])
    .ToMarkdown();
```

## Documentation
For full documentation, including advanced rendering features and the Builder API, please visit the [GitHub Repository](https://github.com/nooogle/CDS.Markdown).

## License
MIT

## Contributing
Pull requests are welcome!

## Repository
https://github.com/nooogle/CDS.Markdown

## Attributions
<a href="https://www.flaticon.com/free-icons/markdown" title="markdown icons">Markdown icons created by brajaomar_j - Flaticon</a>
<a href="https://www.flaticon.com/free-icons/right-arrow" title="right arrow icons">Right arrow icons created by hqrloveq - Flaticon</a>
