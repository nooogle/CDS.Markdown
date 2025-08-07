# CDS.MarkDown Solution

This repository contains:

- **CDS.Markdown**: A .NET 8 WinForms control for rendering Markdown using Markdig and WebView2. Designed for easy integration into your own WinForms applications.
- **Demo**: A sample WinForms application demonstrating how to use the CDS.Markdown control to display Markdown files.

## Projects Overview

### CDS.Markdown
- **Type**: .NET 8 WinForms Class Library
- **Purpose**: Provides a reusable `MarkdownViewer` control for rendering Markdown content in WinForms apps.
- **Key Features:**
  - Renders Markdown using [Markdig](https://github.com/lunet-io/markdig)
  - Displays HTML via [WebView2](https://learn.microsoft.com/en-us/microsoft-edge/webview2/)
  - Supports advanced Markdown extensions
  - Easy to use: just add the control and call `LoadMarkdown()`
- **NuGet**: Ready for packaging and public release

### Demo
- **Type**: .NET 8 WinForms Application
- **Purpose**: Shows how to use the `MarkdownViewer` control in a real application.
- **Usage**: Loads and displays Markdown files from the project directory.

## Getting Started

1. Build the solution. The NuGet package for CDS.Markdown will be generated in the output directory.
2. Reference the CDS.Markdown NuGet package or project in your own WinForms app.
3. Add the `MarkdownViewer` control to your form:
   ```csharp
   using CDS.Markdown;
   var viewer = new MarkdownViewer();
   viewer.LoadMarkdown("README.md");
   this.Controls.Add(viewer);
   ```

## Development
- .NET 8 is required.
- The solution follows modern C# and .NET best practices (see `.github/copilot-instructions.md`).
- Contributions are welcome! Please open issues or pull requests.

## License
MIT License. See [CDS.Markdown/LICENSE](CDS.Markdown/LICENSE).

## Repository
https://github.com/nooogle/CDS.Markdown
