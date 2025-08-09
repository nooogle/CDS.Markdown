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
  - Uses embedded [GitHub Markdown CSS](https://github.com/sindresorhus/github-markdown-css) for GitHub-style rendering
  - Easy to use: just add the control and call `LoadMarkdown()`
- **NuGet**: Ready for packaging and public release

### Demo
- **Type**: .NET 8 WinForms Application
- **Purpose**: Shows how to use the `MarkdownViewer` control in a real application.
- **Usage**: Loads and displays Markdown files from the project directory.


> Click here to view the demo wiki pages: [CDS.Markdown Demo Wiki](../Demo/wiki/index.md)


## GitHub Markdown CSS

This project embeds [github-markdown-css](https://github.com/sindresorhus/github-markdown-css) as an embedded resource to provide GitHub-style Markdown rendering.  
The CSS is included in the assembly, so no additional files or downloads are required when consuming the package.

### Attribution

The [github-markdown-css](https://github.com/sindresorhus/github-markdown-css) is licensed under the MIT License:
