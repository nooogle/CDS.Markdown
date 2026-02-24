# Markdown Creation Guide

`CDS.Markdown` provides two programmatic ways to generate Markdown strings: a **Fluent API** and a **Builder API**. Both approaches eliminate manual string concatenation and formatting bugs.

## Fluent API

The `FluentMarkdownDocument` allows you to chain method calls to build a document. This is ideal for generating static or linear documents.

```csharp
using CDS.Markdown;

var markdown = new FluentMarkdownDocument()
    .AddHeading("Project Status", 1)
    .AddParagraph("Here is the current status of the project:")
    .AddBulletList([
        "Core features implemented",
        "Unit tests passing",
        "Documentation updated"
    ])
    .AddHeading("Architecture", 2)
    .AddMermaidDiagram(@"
        flowchart LR
            A[App] --> B[MarkdownViewer]
            B --> C[WebView2]
    ")
    .AddHeading("Mathematics", 2)
    .AddMathBlock(@"\frac{n!}{k!(n-k)!} = \binom{n}{k}")
    .ToMarkdown();
```

## Builder API

The `MarkdownDocumentBuilder` provides a traditional, statement-based approach. This is useful if you need to conditionally add elements using loops or `if` statements.

```csharp
using CDS.Markdown;

var builder = new MarkdownDocumentBuilder();

builder.AddHeading("System Report", 1);

if (hasErrors)
{
    builder.AddHeading("Errors Found", 2);
    builder.AddCodeBlock(errorLog, "log");
}
else
{
    builder.AddParagraph("All systems operational. :white_check_mark:");
}

var markdown = builder.Build();
```

## Supported Elements

Both APIs support adding:
- **Headings** (`AddHeading`)
- **Paragraphs** (`AddParagraph`)
- **Bullet Lists** (`AddBulletList`)
- **Code Blocks** (`AddCodeBlock`)
- **Images** (`AddImage`)
- **Mermaid Diagrams** (`AddMermaidDiagram` - Fluent only, or via `AddCodeBlock("...", "mermaid")`)
- **Math Blocks** (`AddMathBlock`)
