namespace Demo.CreationDemos;

public class FluentAPI
{
    public static string Run()
    {
        return new CDS.Markdown.FluentMarkdownDocument()
            .AddHeading(":rocket: FluentMarkdownDocument", 1)
            .AddParagraph(
                "Build **structured Markdown** programmatically using a clean, chainable API. " +
                "No string wrangling, no formatting bugs — just pure :fire: code.")
            .AddHeading(":sparkles: Features", 2)
            .AddBulletList([
                ":pencil: **Headings** at any level (H1–H6)",
                ":speech_balloon: **Paragraphs** with inline *Markdown* formatting",
                ":clipboard: **Bullet lists** — just like this one",
                ":computer: **Code blocks** with syntax highlighting",
                ":framed_picture: **Images** with alt text",
                ":smiley: **Emoji** via `:shortcodes:`",
            ])
            .AddHeading("Quick Start", 3)
            .AddParagraph(
                "Chain as many calls as you like, then call `ToMarkdown()` to get the final output. :bulb:")
            .AddCodeBlock(
                """
                var markdown = new FluentMarkdownDocument()
                    .AddHeading(":wave: Hello, World!")
                    .AddParagraph("This is a **bold** statement and this is *italic*.")
                    .AddBulletList(["First item", "Second item", "Third item"])
                    .AddCodeBlock("Console.WriteLine(\"Hello!\");", "csharp")
                    .ToMarkdown();
                """, "csharp")
            .AddHeading(":triangular_ruler: Heading Levels", 2)
            .AddParagraph("All six Markdown heading levels are supported:")
            .AddHeading("H2 — Section title", 2)
            .AddHeading("H3 — Subsection", 3)
            .AddHeading("H4 — Detail heading", 4)
            .AddParagraph(
                "*Tip:* Use **H2** and **H3** for most document structure. " +
                "Reserve **H1** for the document title only.")
            .AddHeading(":white_check_mark: Project Status", 2)
            .AddBulletList([
                ":white_check_mark: Stable fluent API",
                ":white_check_mark: Targets .NET 8, .NET 10",
                ":white_check_mark: Mermaid diagram rendering",
                ":construction: Ordered lists — *coming soon*",
                ":construction: Tables — *coming soon*",
            ])
            .AddHeading(":bar_chart: Mermaid Diagram Support", 2)
            .AddParagraph(
                "Mermaid diagrams render as **inline SVG** directly in the viewer — " +
                "the `mermaid.js` bundle is embedded in the assembly so **no internet connection is required**.")
            .AddHeading("Sequence Diagram", 3)
            .AddMermaidDiagram(
                """
                sequenceDiagram
                    participant App
                    participant FluentMarkdownDocument
                    participant MarkdownRenderer
                    participant MarkdownViewer

                    App->>FluentMarkdownDocument: AddHeading(), AddMermaidDiagram()...
                    App->>FluentMarkdownDocument: ToMarkdown()
                    FluentMarkdownDocument-->>App: markdown string
                    App->>MarkdownViewer: LoadMarkdownAsync(path)
                    MarkdownViewer->>MarkdownRenderer: RenderToHtml(markdown)
                    MarkdownRenderer-->>MarkdownViewer: html (with mermaid div)
                    MarkdownViewer-->>App: diagram rendered as SVG
                """)
            .AddHeading("Flowchart", 3)
            .AddMermaidDiagram(
                """
                flowchart LR
                    A([Markdown string]) --> B[MarkdownRenderer]
                    B --> C[HTML + mermaid div]
                    C --> D[MarkdownHtmlDocumentBuilder]
                    D --> E[Full HTML page\n+ mermaid.js bundle]
                    E --> F([WebView2 renders SVG])
                """)
            .ToMarkdown();
    }
}
