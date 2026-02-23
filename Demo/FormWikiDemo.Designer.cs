namespace Demo
{
    partial class FormWikiDemo
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            markdownViewer = new CDS.Markdown.MarkdownViewer();
            labelInfo = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // markdownViewer
            // 
            markdownViewer.BorderStyle = BorderStyle.FixedSingle;
            markdownViewer.Dock = DockStyle.Fill;
            markdownViewer.Location = new Point(3, 44);
            markdownViewer.Name = "markdownViewer";
            markdownViewer.Size = new Size(748, 667);
            markdownViewer.TabIndex = 0;
            // 
            // labelInfo
            // 
            labelInfo.Dock = DockStyle.Fill;
            labelInfo.Location = new Point(3, 0);
            labelInfo.Name = "labelInfo";
            labelInfo.Size = new Size(748, 41);
            labelInfo.TabIndex = 0;
            labelInfo.Text = "label1";
            labelInfo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(labelInfo, 0, 0);
            tableLayoutPanel1.Controls.Add(markdownViewer, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(12, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 41F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(754, 714);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(778, 738);
            Controls.Add(tableLayoutPanel1);
            Name = "Form1";
            Padding = new Padding(12);
            Text = "CDS.Markdown demo";
            Load += Form1_Load;
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private CDS.Markdown.MarkdownViewer markdownViewer;
        private Label labelInfo;
        private TableLayoutPanel tableLayoutPanel1;
    }
}
