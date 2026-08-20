# CDS.Markdown Solution

[![CI](https://github.com/nooogle/CDS.Markdown/actions/workflows/ci.yml/badge.svg)](https://github.com/nooogle/CDS.Markdown/actions/workflows/ci.yml)
[![CodeQL](https://github.com/nooogle/CDS.Markdown/actions/workflows/codeql.yml/badge.svg)](https://github.com/nooogle/CDS.Markdown/actions/workflows/codeql.yml)
[![OpenSSF Scorecard](https://api.securityscorecards.dev/projects/github.com/nooogle/CDS.Markdown/badge)](https://securityscorecards.dev/viewer/?uri=github.com/nooogle/CDS.Markdown)
[![NuGet](https://img.shields.io/nuget/v/CDS.Markdown.svg)](https://www.nuget.org/packages/CDS.Markdown/)
[![License](https://img.shields.io/github/license/nooogle/CDS.Markdown)](LICENSE.txt)

**CDS.Markdown** is a comprehensive .NET library for both **rendering** and **generating** Markdown. It is designed for easy integration into your .NET 8 and .NET 10 applications.

## Two packages

| | [`CDS.Markdown.Lite`](https://www.nuget.org/packages/CDS.Markdown.Lite/) | [`CDS.Markdown`](https://www.nuget.org/packages/CDS.Markdown/) |
|---|---|---|
| Gives you | `MarkdownTextBox` + the Fluent/Builder generation APIs | Everything in `CDS.Markdown.Lite`, plus `MarkdownViewer` |
| Depends on | `Markdig` only | `Markdig`, `Microsoft.Web.WebView2`, and `CDS.Markdown.Lite` |
| Pick this when | You only need prose-sized rendering or Markdown generation, and want to keep WebView2 out of your dependency tree entirely | You need `MarkdownViewer`'s full document fidelity (Mermaid, MathJax, real tables, images, links) |

`CDS.Markdown` depends on `CDS.Markdown.Lite`, so installing `CDS.Markdown` still gets you `MarkdownTextBox` and the generation APIs too — nothing is lost by installing the bigger package. Install `CDS.Markdown.Lite` on its own only when you specifically want to avoid the WebView2 dependency.

## Features

- 🖥️ **WinForms Viewer Control** (`CDS.Markdown`): A drop-in `MarkdownViewer` control powered by [WebView2](https://learn.microsoft.com/en-us/microsoft-edge/webview2/) and [Markdig](https://github.com/lunet-io/markdig). Includes Light and Dark mode theme support.
- ⚡ **Lightweight Text Box Control** (`CDS.Markdown.Lite`): `MarkdownTextBox`, a `RichTextBox`-based control for prose-sized Markdown (headings, formatting, lists, tables) with no WebView2 process and a synchronous preferred-height read — a cheaper option when you're rendering many small fragments (e.g. chat bubbles) rather than a full document.
- 📝 **Programmatic Creation** (`CDS.Markdown.Lite`): Generate Markdown dynamically using a clean **Fluent API** or a traditional **Builder API**.
- 🔌 **Offline-First**: Embedded resources for GitHub-style CSS, Mermaid.js diagrams, and MathJax (LaTeX math) mean **no internet connection is required** to render advanced Markdown.
- 🧪 **Fully Tested**: Comprehensive unit test coverage ensuring reliable HTML generation and Markdown building.

## Quick Start

### 1. Installation
Install whichever package matches what you need (see the table above):
```bash
dotnet add package CDS.Markdown.Lite
# or, for the full WebView2 viewer too:
dotnet add package CDS.Markdown
```

### 2. Viewing Markdown (WinForms)
Drop the `MarkdownViewer` control onto your form and load a file:
```csharp
await markdownViewer1.LoadMarkdownAsync("readme.md");
```
👉 [Read the full Viewer Documentation](docs/viewer.md)

Or, for many small fragments rather than a full document, drop a `MarkdownTextBox` control onto your form instead — this one only needs `CDS.Markdown.Lite`:
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

- **`CDS.Markdown.Lite`**: The lightweight library — `MarkdownTextBox` and the Markdown generation APIs. Depends on Markdig only.
- **`CDS.Markdown`**: The WebView2-based viewer control and its HTML-building pipeline. Depends on `CDS.Markdown.Lite`.
- **`Demo`**: A sample WinForms application demonstrating the viewer, the text box, and the creation APIs.
- **`UnitTests`**: MSTest project covering both packages — HTML rendering, session management, `MarkdownTextBox` rendering, and Markdown generation.
- **`UiTests`**: FlaUI project that drives the real `Demo.exe` end-to-end.

## Documentation

- [Markdown Viewer Guide](docs/viewer.md)
- [Markdown Creation Guide](docs/creation.md)
- [Demo Wiki](Demo/wiki/index.md)

## Attributions

- [github-markdown-css](https://github.com/sindresorhus/github-markdown-css) (MIT License)
- [Markdig](https://github.com/lunet-io/markdig) (BSD-2-Clause)
- [Mermaid.js](https://mermaid.js.org/) (MIT License)
- [MathJax](https://www.mathjax.org/) (Apache-2.0)
