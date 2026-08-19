namespace Demo
{
    partial class FormMarkdownTextBoxDemo
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
            markdownTextBox = new CDS.Markdown.MarkdownTextBox();
            labelInfo = new Label();
            labelPreferredHeight = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            //
            // markdownTextBox
            //
            markdownTextBox.BorderStyle = BorderStyle.FixedSingle;
            markdownTextBox.Dock = DockStyle.Fill;
            markdownTextBox.Location = new Point(3, 44);
            markdownTextBox.Name = "markdownTextBox";
            markdownTextBox.Size = new Size(748, 646);
            markdownTextBox.TabIndex = 0;
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
            // labelPreferredHeight
            //
            labelPreferredHeight.Dock = DockStyle.Bottom;
            labelPreferredHeight.Location = new Point(12, 715);
            labelPreferredHeight.Name = "labelPreferredHeight";
            labelPreferredHeight.Size = new Size(754, 23);
            labelPreferredHeight.TabIndex = 1;
            labelPreferredHeight.Text = "Preferred content height: (pending)";
            labelPreferredHeight.TextAlign = ContentAlignment.MiddleCenter;
            //
            // tableLayoutPanel1
            //
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(labelInfo, 0, 0);
            tableLayoutPanel1.Controls.Add(markdownTextBox, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(12, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 41F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(754, 714);
            tableLayoutPanel1.TabIndex = 2;
            //
            // FormMarkdownTextBoxDemo
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(778, 738);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(labelPreferredHeight);
            Name = "FormMarkdownTextBoxDemo";
            Padding = new Padding(12);
            Text = "CDS.Markdown - MarkdownTextBox demo";
            Load += FormMarkdownTextBoxDemo_Load;
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private CDS.Markdown.MarkdownTextBox markdownTextBox;
        private Label labelInfo;
        private Label labelPreferredHeight;
        private TableLayoutPanel tableLayoutPanel1;
    }
}
