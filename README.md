# CDS.Markdown Solution

[![CI](https://github.com/nooogle/CDS.Markdown/actions/workflows/ci.yml/badge.svg)](https://github.com/nooogle/CDS.Markdown/actions/workflows/ci.yml)
[![CodeQL](https://github.com/nooogle/CDS.Markdown/actions/workflows/codeql.yml/badge.svg)](https://github.com/nooogle/CDS.Markdown/actions/workflows/codeql.yml)
[![OpenSSF Scorecard](https://api.securityscorecards.dev/projects/github.com/nooogle/CDS.Markdown/badge)](https://securityscorecards.dev/viewer/?uri=github.com/nooogle/CDS.Markdown)
[![NuGet](https://img.shields.io/nuget/v/CDS.Markdown.svg)](https://www.nuget.org/packages/CDS.Markdown/)
[![License](https://img.shields.io/github/license/nooogle/CDS.Markdown)](LICENSE.txt)

**CDS.Markdown** is a comprehensive .NET library for both **rendering** and **generating** Markdown. It is designed for easy integration into your .NET 8 and .NET 10 applications.

## Features

- 🖥️ **WinForms Viewer Control**: A drop-in `MarkdownViewer` control powered by [WebView2](https://learn.microsoft.com/en-us/microsoft-edge/webview2/) and [Markdig](https://github.com/lunet-io/markdig). Includes Light and Dark mode theme support.
- ⚡ **Lightweight Text Box Control**: `MarkdownTextBox`, a `RichTextBox`-based control for prose-sized Markdown (headings, formatting, lists, tables) with no WebView2 process and a synchronous preferred-height read — a cheaper option when you're rendering many small fragments (e.g. chat bubbles) rather than a full document.
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

Or, for many small fragments rather than a full document, drop a `MarkdownTextBox` control onto your form instead:
```csharp
markdownTextBox1.SetMarkdown("**Hello**, world!");
```
See the "Embedding as a Fragment" section of the [Viewer Documentation](docs/viewer.md) for the tradeoff between the two controls.

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

- **`CDS.Markdown`**: The core library containing the viewer control, the lightweight text box control, HTML builder, and Markdown generation APIs.
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
