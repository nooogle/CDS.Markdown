using CDS.Markdown;

namespace Demo
{
    public partial class Form1 : Form
    {
        private MarkdownViewer markdownViewer;
        public Form1()
        {
            InitializeComponent();
            markdownViewer = new MarkdownViewer
            {
                Dock = DockStyle.Fill
            };
            Controls.Add(markdownViewer);
            // Example usage: load index.md from the app directory
            markdownViewer.LoadMarkdown("index.md");
        }
    }
}
