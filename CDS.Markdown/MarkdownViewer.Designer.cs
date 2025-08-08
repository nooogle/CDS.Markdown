namespace CDS.Markdown
{
    partial class MarkdownViewer
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        private void InitializeComponent()
        {
            webView = new Microsoft.Web.WebView2.WinForms.WebView2();
            panel1 = new Panel();
            btnHome = new Button();
            btnForward = new Button();
            btnBack = new Button();
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
            panel1.Controls.Add(btnHome);
            panel1.Controls.Add(btnForward);
            panel1.Controls.Add(btnBack);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(3);
            panel1.Size = new Size(438, 49);
            panel1.TabIndex = 1;
            // 
            // btnHome
            // 
            btnHome.FlatAppearance.BorderSize = 0;
            btnHome.FlatStyle = FlatStyle.Flat;
            btnHome.Font = new Font("Segoe UI Symbol", 12F);
            btnHome.Image = Properties.Resources.home_button_2;
            btnHome.Location = new Point(6, 6);
            btnHome.Name = "btnHome";
            btnHome.Size = new Size(54, 37);
            btnHome.TabIndex = 2;
            btnHome.UseVisualStyleBackColor = true;
            btnHome.Click += btnHome_Click;
            // 
            // btnForward
            // 
            btnForward.FlatAppearance.BorderSize = 0;
            btnForward.FlatStyle = FlatStyle.Flat;
            btnForward.Font = new Font("Segoe UI Symbol", 12F);
            btnForward.Image = Properties.Resources.right_arrow;
            btnForward.Location = new Point(126, 6);
            btnForward.Name = "btnForward";
            btnForward.Size = new Size(54, 37);
            btnForward.TabIndex = 1;
            btnForward.UseVisualStyleBackColor = true;
            btnForward.Click += btnForward_Click;
            // 
            // btnBack
            // 
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Font = new Font("Segoe UI Symbol", 12F);
            btnBack.Image = Properties.Resources.left_align;
            btnBack.Location = new Point(66, 6);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(54, 37);
            btnBack.TabIndex = 0;
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
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
        private Button btnHome;
    }
}
