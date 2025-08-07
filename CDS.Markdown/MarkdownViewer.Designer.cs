namespace CDS.Markdown
{
    partial class MarkdownViewer
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

        #region Component Designer generated code

        private void InitializeComponent()
        {
            webView = new Microsoft.Web.WebView2.WinForms.WebView2();
            panel1 = new Panel();
            btnBack = new Button();
            btnForward = new Button();
            ((System.ComponentModel.ISupportInitialize)webView).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // webView
            // 
            webView.AllowExternalDrop = true;
            webView.CreationProperties = null;
            webView.DefaultBackgroundColor = Color.White;
            webView.Dock = DockStyle.Fill;
            webView.Location = new Point(0, 49);
            webView.Name = "webView";
            webView.Size = new Size(438, 323);
            webView.TabIndex = 0;
            webView.ZoomFactor = 1D;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnForward);
            panel1.Controls.Add(btnBack);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(12);
            panel1.Size = new Size(438, 49);
            panel1.TabIndex = 1;
            // 
            // btnBack
            // 
            btnBack.Location = new Point(15, 15);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(75, 23);
            btnBack.TabIndex = 0;
            btnBack.Text = "Back";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // btnForward
            // 
            btnForward.Location = new Point(96, 15);
            btnForward.Name = "btnForward";
            btnForward.Size = new Size(75, 23);
            btnForward.TabIndex = 1;
            btnForward.Text = "Forward";
            btnForward.UseVisualStyleBackColor = true;
            btnForward.Click += btnForward_Click;
            // 
            // MarkdownViewer
            // 
            Controls.Add(webView);
            Controls.Add(panel1);
            Name = "MarkdownViewer";
            Size = new Size(438, 372);
            ((System.ComponentModel.ISupportInitialize)webView).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 webView;
        private Panel panel1;
        private Button btnForward;
        private Button btnBack;
    }
}
