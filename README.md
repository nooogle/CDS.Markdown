# CDS.Markdown

A simple .NET 8 WinForms control for rendering Markdown using Markdig and WebView2.

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