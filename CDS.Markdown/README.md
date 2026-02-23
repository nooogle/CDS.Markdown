# CDS.Markdown

A simple .NET WinForms control for rendering Markdown using Markdig and WebView2.

**Compatible with:**
- .NET 8
- .NET 10

## Features
- Render Markdown files in your WinForms app
- Uses [Markdig](https://github.com/lunet-io/markdig) for Markdown parsing
- Uses [WebView2](https://learn.microsoft.com/en-us/microsoft-edge/webview2/) for HTML rendering
- Supports advanced Markdown extensions

## Getting Started
1. Install the NuGet package:
   ```shell
   dotnet add package CDS.Markdown
   ```
2. Add the `MarkdownViewer` control to your form:
   ```csharp
   using CDS.Markdown;
   // ...
   var viewer = new MarkdownViewer();
   viewer.LoadMarkdown("README.md");
   this.Controls.Add(viewer);
   ```

## License
MIT

## Contributing
Pull requests are welcome!

## Repository
https://github.com/nooogle/CDS.Markdown


## Attributions
<a href="https://www.flaticon.com/free-icons/markdown" title="markdown icons">Markdown icons created by brajaomar_j - Flaticon</a>
<a href="https://www.flaticon.com/free-icons/right-arrow" title="right arrow icons">Right arrow icons created by hqrloveq - Flaticon</a>
