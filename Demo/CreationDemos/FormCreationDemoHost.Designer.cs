namespace Demo.CreationDemos
{
    partial class FormCreationDemoHost
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            markdownViewer = new CDS.Markdown.MarkdownViewer();
            SuspendLayout();
            // 
            // markdownViewer
            // 
            markdownViewer.BorderStyle = BorderStyle.FixedSingle;
            markdownViewer.Dock = DockStyle.Fill;
            markdownViewer.Location = new Point(0, 0);
            markdownViewer.Name = "markdownViewer";
            markdownViewer.Size = new Size(800, 450);
            markdownViewer.TabIndex = 1;
            // 
            // FormCreationDemoHost
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(markdownViewer);
            Name = "FormCreationDemoHost";
            Text = "FormCreationDemoHost";
            ResumeLayout(false);
        }

        #endregion

        private CDS.Markdown.MarkdownViewer markdownViewer;
    }
}