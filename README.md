# CDS.Markdown Solution

[![Build & Test](https://github.com/nooogle/CDS.Markdown/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/nooogle/CDS.Markdown/actions/workflows/build-and-test.yml)
[![NuGet](https://img.shields.io/nuget/v/CDS.Markdown.svg)](https://www.nuget.org/packages/CDS.Markdown/)
[![codecov](https://codecov.io/gh/nooogle/CDS.Markdown/graph/badge.svg?token=YOUR_TOKEN)](https://codecov.io/gh/nooogle/CDS.Markdown)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)

**CDS.Markdown** is a comprehensive .NET library for both **rendering** and **generating** Markdown. It is designed for easy integration into your .NET 8 and .NET 10 applications.

## Features

- 🖥️ **WinForms Viewer Control**: A drop-in `MarkdownViewer` control powered by [WebView2](https://learn.microsoft.com/en-us/microsoft-edge/webview2/) and [Markdig](https://github.com/lunet-io/markdig). Includes Light and Dark mode theme support.
- 📝 **Programmatic Creation**: Generate Markdown dynamically using a clean **Fluent API** or a traditional **Builder API**.
- 🔌 **Offline-First**: Embedded resources for GitHub-style CSS, Mermaid.js diagrams, and MathJax (LaTeX math) mean **no internet connection is required** to render advanced Markdown.
- 🧪 **Fully Tested**: Comprehensive unit test coverage ensuring reliable HTML generation and Markdown building.

## Quick Start

### 1. Installation
Install the package via NuGet:
```bash
dotnet add package CDS.Markdown
```

### 2. Viewing Markdown (WinForms)
Drop the `MarkdownViewer` control onto your form and load a file:
```csharp
await markdownViewer1.LoadMarkdownAsync("readme.md");
```
👉 [Read the full Viewer Documentation](docs/viewer.md)

### 3. Generating Markdown (Fluent API)
Create structured Markdown programmatically without string wrangling:
```csharp
var markdown = new FluentMarkdownDocument()
    .AddHeading("Hello, World!")
    .AddParagraph("This is a **bold** statement.")
    .AddBulletList(["First item", "Second item"])
    .AddMermaidDiagram("graph TD;\nA-->B;")
    .ToMarkdown();
```
👉 [Read the full Creation Documentation](docs/creation.md)

## Project Structure

- **`CDS.Markdown`**: The core library containing the viewer control, HTML builder, and Markdown generation APIs.
- **`Demo`**: A sample WinForms application demonstrating both the viewer and the creation APIs.
- **`UnitTests`**: MSTest project covering HTML rendering, session management, and Markdown generation.

## Documentation

- [Markdown Viewer Guide](docs/viewer.md)
- [Markdown Creation Guide](docs/creation.md)
- [Demo Wiki](Demo/wiki/index.md)

## Attributions

- [github-markdown-css](https://github.com/sindresorhus/github-markdown-css) (MIT License)
- [Markdig](https://github.com/lunet-io/markdig) (BSD-2-Clause)
- [Mermaid.js](https://mermaid.js.org/) (MIT License)
- [MathJax](https://www.mathjax.org/) (Apache-2.0)
