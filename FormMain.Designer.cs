namespace DirectoryReporter
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.m_button_Cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.m_textBox_Depth = new System.Windows.Forms.TextBox();
            this.m_button_ChangeDirectory = new System.Windows.Forms.Button();
            this.m_textBox_Directory = new System.Windows.Forms.TextBox();
            this.m_button_Start = new System.Windows.Forms.Button();
            this.m_DirectoryReport = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.m_button_Cancel);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.m_textBox_Depth);
            this.splitContainer1.Panel1.Controls.Add(this.m_button_ChangeDirectory);
            this.splitContainer1.Panel1.Controls.Add(this.m_textBox_Directory);
            this.splitContainer1.Panel1.Controls.Add(this.m_button_Start);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.m_DirectoryReport);
            this.splitContainer1.Size = new System.Drawing.Size(828, 644);
            this.splitContainer1.SplitterDistance = 38;
            this.splitContainer1.TabIndex = 0;
            // 
            // m_button_Cancel
            // 
            this.m_button_Cancel.Location = new System.Drawing.Point(12, 13);
            this.m_button_Cancel.Name = "m_button_Cancel";
            this.m_button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.m_button_Cancel.TabIndex = 5;
            this.m_button_Cancel.Text = "Cancel";
            this.m_button_Cancel.UseVisualStyleBackColor = true;
            this.m_button_Cancel.Visible = false;
            this.m_button_Cancel.Click += new System.EventHandler(this.m_button_Cancel_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(716, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Depth";
            // 
            // m_textBox_Depth
            // 
            this.m_textBox_Depth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_textBox_Depth.Location = new System.Drawing.Point(758, 15);
            this.m_textBox_Depth.Name = "m_textBox_Depth";
            this.m_textBox_Depth.Size = new System.Drawing.Size(58, 20);
            this.m_textBox_Depth.TabIndex = 3;
            this.m_textBox_Depth.Text = "8";
            // 
            // m_button_ChangeDirectory
            // 
            this.m_button_ChangeDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_button_ChangeDirectory.Location = new System.Drawing.Point(644, 13);
            this.m_button_ChangeDirectory.Name = "m_button_ChangeDirectory";
            this.m_button_ChangeDirectory.Size = new System.Drawing.Size(27, 23);
            this.m_button_ChangeDirectory.TabIndex = 2;
            this.m_button_ChangeDirectory.Text = "...";
            this.m_button_ChangeDirectory.UseVisualStyleBackColor = true;
            this.m_button_ChangeDirectory.Click += new System.EventHandler(this.m_button_ChangeDirectory_Click);
            // 
            // m_textBox_Directory
            // 
            this.m_textBox_Directory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_textBox_Directory.Location = new System.Drawing.Point(93, 15);
            this.m_textBox_Directory.Name = "m_textBox_Directory";
            this.m_textBox_Directory.Size = new System.Drawing.Size(545, 20);
            this.m_textBox_Directory.TabIndex = 1;
            this.m_textBox_Directory.Text = "c:\\";
            // 
            // m_button_Start
            // 
            this.m_button_Start.Location = new System.Drawing.Point(12, 12);
            this.m_button_Start.Name = "m_button_Start";
            this.m_button_Start.Size = new System.Drawing.Size(75, 23);
            this.m_button_Start.TabIndex = 0;
            this.m_button_Start.Text = "Start";
            this.m_button_Start.UseVisualStyleBackColor = true;
            this.m_button_Start.Click += new System.EventHandler(this.m_button_Start_Click);
            // 
            // m_DirectoryReport
            // 
            this.m_DirectoryReport.ContextMenuStrip = this.contextMenuStrip1;
            this.m_DirectoryReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_DirectoryReport.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_DirectoryReport.Location = new System.Drawing.Point(0, 0);
            this.m_DirectoryReport.Name = "m_DirectoryReport";
            this.m_DirectoryReport.Size = new System.Drawing.Size(828, 602);
            this.m_DirectoryReport.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFolderToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(140, 26);
            // 
            // openFolderToolStripMenuItem
            // 
            this.openFolderToolStripMenuItem.Name = "openFolderToolStripMenuItem";
            this.openFolderToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openFolderToolStripMenuItem.Text = "Open Folder";
            this.openFolderToolStripMenuItem.Click += new System.EventHandler(this.openFolderToolStripMenuItem_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(828, 644);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FormMain";
            this.Text = "Directory Reporter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button m_button_ChangeDirectory;
        private System.Windows.Forms.TextBox m_textBox_Directory;
        private System.Windows.Forms.Button m_button_Start;
        private System.Windows.Forms.TreeView m_DirectoryReport;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox m_textBox_Depth;
        private System.Windows.Forms.Button m_button_Cancel;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openFolderToolStripMenuItem;
    }
}

