namespace CDS.Markdown;

/// <summary>
/// Provides static resources (CSS, scripts) used by the MarkdownViewer and related classes.
/// </summary>
public static class MarkdownViewerResources
{
    /// <summary>
    /// The default CSS for markdown rendering.
    /// This provides basic table and body styling to supplement GitHub's CSS.
    /// </summary>
    public static string DefaultCss => @"
    body { font-family: sans-serif; padding: 20px; }
    table {
      border-collapse: collapse;
      width: auto;
      max-width: 100%;
      margin-bottom: 1em;
      word-break: break-word;
      overflow-x: auto;
      display: block;
    }
    th, td {
      border: 1px solid #888;
      padding: 6px 10px;
      text-align: left;
      vertical-align: top;
      word-break: break-word;
    }
    th { background: #f0f0f0; }
    ";

    /// <summary>
    /// The script to intercept .md link clicks and route them through the viewer.
    /// - Listens for click events on anchor tags.
    /// - If the link points to a .md file, prevents default navigation and sends the href to the host app via WebView2 messaging.
    /// This allows seamless in-app navigation between Markdown files.
    /// </summary>
    public static string LinkInterceptScript => @"<script>
document.addEventListener('DOMContentLoaded', function() {
  document.body.addEventListener('click', function(e) {
    let t = e.target;
    while (t && t.tagName !== 'A') t = t.parentElement;
    if (t && t.tagName === 'A') {
      let href = t.getAttribute('href');
      if (href && href.match(/\\.md($|[#?])/i)) {
        e.preventDefault();
        window.chrome.webview.postMessage(href);
      }
    }
  }, true);
});
</script>";
}
