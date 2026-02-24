namespace Demo.CreationDemos;

public class BuilderAPI
{
    public static string Run()
    {
        var builder = new CDS.Markdown.MarkdownDocumentBuilder();
        
        builder.AddHeading(":hammer_and_wrench: MarkdownDocumentBuilder", 1);
        builder.AddParagraph(
            "Build **structured Markdown** programmatically using a traditional builder API. " +
            "No string wrangling, no formatting bugs — just pure :fire: code.");
        
        builder.AddHeading(":sparkles: Features", 2);
        builder.AddBulletList([
            ":pencil: **Headings** at any level (H1–H6)",
            ":speech_balloon: **Paragraphs** with inline *Markdown* formatting",
            ":clipboard: **Bullet lists** — just like this one",
            ":computer: **Code blocks** with syntax highlighting",
            ":framed_picture: **Images** with alt text",
            ":smiley: **Emoji** via `:shortcodes:`",
        ]);
        
        builder.AddHeading("Quick Start", 3);
        builder.AddParagraph(
            "Call the `Add*` methods to build your document, then call `Build()` to get the final output. :bulb:");
        
        builder.AddCodeBlock(
            """
            var builder = new MarkdownDocumentBuilder();
            builder.AddHeading(":wave: Hello, World!");
            builder.AddParagraph("This is a **bold** statement and this is *italic*.");
            builder.AddBulletList(["First item", "Second item", "Third item"]);
            builder.AddCodeBlock("Console.WriteLine(\"Hello!\");", "csharp");
            var markdown = builder.Build();
            """, "csharp");
            
        builder.AddHeading(":triangular_ruler: Heading Levels", 2);
        builder.AddParagraph("All six Markdown heading levels are supported:");
        builder.AddHeading("H2 — Section title", 2);
        builder.AddHeading("H3 — Subsection", 3);
        builder.AddHeading("H4 — Detail heading", 4);
        builder.AddParagraph(
            "*Tip:* Use **H2** and **H3** for most document structure. " +
            "Reserve **H1** for the document title only.");
            
        builder.AddHeading(":white_check_mark: Project Status", 2);
        builder.AddBulletList([
            ":white_check_mark: Stable builder API",
            ":white_check_mark: Targets .NET 8, .NET 10",
            ":white_check_mark: Mermaid diagram rendering",
            ":construction: Ordered lists — *coming soon*",
            ":construction: Tables — *coming soon*",
        ]);
        
        builder.AddHeading(":bar_chart: Mermaid Diagram Support", 2);
        builder.AddParagraph(
            "Mermaid diagrams render as **inline SVG** directly in the viewer — " +
            "the `mermaid.js` bundle is embedded in the assembly so **no internet connection is required**.");
            
        builder.AddHeading("Sequence Diagram", 3);
        builder.AddCodeBlock(
            """
            sequenceDiagram
                participant App
                participant MarkdownDocumentBuilder
                participant MarkdownRenderer
                participant MarkdownViewer

                App->>MarkdownDocumentBuilder: AddHeading(), AddCodeBlock()...
                App->>MarkdownDocumentBuilder: Build()
                MarkdownDocumentBuilder-->>App: markdown string
                App->>MarkdownViewer: LoadMarkdownAsync(path)
                MarkdownViewer->>MarkdownRenderer: RenderToHtml(markdown)
                MarkdownRenderer-->>MarkdownViewer: html (with mermaid div)
                MarkdownViewer-->>App: diagram rendered as SVG
            """, "mermaid");
            
        builder.AddHeading("Flowchart", 3);
        builder.AddCodeBlock(
            """
            flowchart LR
                A([Markdown string]) --> B[MarkdownRenderer]
                B --> C[HTML + mermaid div]
                C --> D[MarkdownHtmlDocumentBuilder]
                D --> E[Full HTML page\n+ mermaid.js bundle]
                E --> F([WebView2 renders SVG])
            """, "mermaid");

        builder.AddHeading(":heavy_division_sign: Mathematics Support", 2);
        builder.AddParagraph("LaTeX-like math rendering is supported via Markdig's mathematics extension. Note that to render this in the browser, you may need to include a library like KaTeX or MathJax in your HTML template.");
        builder.AddMathBlock(@"\frac{n!}{k!(n-k)!} = \binom{n}{k}");

        return builder.Build();
    }
}
