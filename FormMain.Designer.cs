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
            this.m_comboBox_Mode = new System.Windows.Forms.ComboBox();
            this.m_button_ChangeDirectory2 = new System.Windows.Forms.Button();
            this.m_textBox_Directory2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.m_textBox_Depth = new System.Windows.Forms.TextBox();
            this.m_button_ChangeDirectory = new System.Windows.Forms.Button();
            this.m_textBox_Directory = new System.Windows.Forms.TextBox();
            this.m_button_Start = new System.Windows.Forms.Button();
            this.m_button_Cancel = new System.Windows.Forms.Button();
            this.m_DirectoryReport = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_button_SyncItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.m_comboBox_Mode);
            this.splitContainer1.Panel1.Controls.Add(this.m_button_ChangeDirectory2);
            this.splitContainer1.Panel1.Controls.Add(this.m_textBox_Directory2);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.m_textBox_Depth);
            this.splitContainer1.Panel1.Controls.Add(this.m_button_ChangeDirectory);
            this.splitContainer1.Panel1.Controls.Add(this.m_textBox_Directory);
            this.splitContainer1.Panel1.Controls.Add(this.m_button_Start);
            this.splitContainer1.Panel1.Controls.Add(this.m_button_Cancel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.m_DirectoryReport);
            this.splitContainer1.Size = new System.Drawing.Size(828, 644);
            this.splitContainer1.SplitterDistance = 63;
            this.splitContainer1.TabIndex = 0;
            // 
            // m_comboBox_Mode
            // 
            this.m_comboBox_Mode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_comboBox_Mode.FormattingEnabled = true;
            this.m_comboBox_Mode.Items.AddRange(new object[] {
            "Size Report",
            "Differences"});
            this.m_comboBox_Mode.Location = new System.Drawing.Point(695, 37);
            this.m_comboBox_Mode.Name = "m_comboBox_Mode";
            this.m_comboBox_Mode.Size = new System.Drawing.Size(121, 21);
            this.m_comboBox_Mode.TabIndex = 8;
            this.m_comboBox_Mode.Text = "Size Report";
            this.m_comboBox_Mode.SelectedIndexChanged += new System.EventHandler(this.m_comboBox_Mode_SelectedIndexChanged);
            // 
            // m_button_ChangeDirectory2
            // 
            this.m_button_ChangeDirectory2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_button_ChangeDirectory2.Enabled = false;
            this.m_button_ChangeDirectory2.Location = new System.Drawing.Point(644, 35);
            this.m_button_ChangeDirectory2.Name = "m_button_ChangeDirectory2";
            this.m_button_ChangeDirectory2.Size = new System.Drawing.Size(27, 23);
            this.m_button_ChangeDirectory2.TabIndex = 7;
            this.m_button_ChangeDirectory2.Text = "...";
            this.m_button_ChangeDirectory2.UseVisualStyleBackColor = true;
            this.m_button_ChangeDirectory2.Click += new System.EventHandler(this.m_button_ChangeDirectory2_Click);
            // 
            // m_textBox_Directory2
            // 
            this.m_textBox_Directory2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_textBox_Directory2.Enabled = false;
            this.m_textBox_Directory2.Location = new System.Drawing.Point(93, 37);
            this.m_textBox_Directory2.Name = "m_textBox_Directory2";
            this.m_textBox_Directory2.Size = new System.Drawing.Size(545, 20);
            this.m_textBox_Directory2.TabIndex = 6;
            this.m_textBox_Directory2.Text = "c:\\";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(716, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Depth";
            // 
            // m_textBox_Depth
            // 
            this.m_textBox_Depth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_textBox_Depth.Location = new System.Drawing.Point(758, 11);
            this.m_textBox_Depth.Name = "m_textBox_Depth";
            this.m_textBox_Depth.Size = new System.Drawing.Size(58, 20);
            this.m_textBox_Depth.TabIndex = 3;
            this.m_textBox_Depth.Text = "99";
            // 
            // m_button_ChangeDirectory
            // 
            this.m_button_ChangeDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_button_ChangeDirectory.Location = new System.Drawing.Point(644, 9);
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
            this.m_textBox_Directory.Location = new System.Drawing.Point(93, 11);
            this.m_textBox_Directory.Name = "m_textBox_Directory";
            this.m_textBox_Directory.Size = new System.Drawing.Size(545, 20);
            this.m_textBox_Directory.TabIndex = 1;
            this.m_textBox_Directory.Text = "c:\\";
            // 
            // m_button_Start
            // 
            this.m_button_Start.Location = new System.Drawing.Point(12, 10);
            this.m_button_Start.Name = "m_button_Start";
            this.m_button_Start.Size = new System.Drawing.Size(75, 23);
            this.m_button_Start.TabIndex = 0;
            this.m_button_Start.Text = "Start";
            this.m_button_Start.UseVisualStyleBackColor = true;
            this.m_button_Start.Click += new System.EventHandler(this.m_button_Start_Click);
            // 
            // m_button_Cancel
            // 
            this.m_button_Cancel.Location = new System.Drawing.Point(12, 9);
            this.m_button_Cancel.Name = "m_button_Cancel";
            this.m_button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.m_button_Cancel.TabIndex = 5;
            this.m_button_Cancel.Text = "Cancel";
            this.m_button_Cancel.UseVisualStyleBackColor = true;
            this.m_button_Cancel.Visible = false;
            this.m_button_Cancel.Click += new System.EventHandler(this.m_button_Cancel_Click);
            // 
            // m_DirectoryReport
            // 
            this.m_DirectoryReport.ContextMenuStrip = this.contextMenuStrip1;
            this.m_DirectoryReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_DirectoryReport.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_DirectoryReport.Location = new System.Drawing.Point(0, 0);
            this.m_DirectoryReport.Name = "m_DirectoryReport";
            this.m_DirectoryReport.Size = new System.Drawing.Size(828, 577);
            this.m_DirectoryReport.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFolderToolStripMenuItem,
            this.m_button_SyncItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(140, 48);
            // 
            // openFolderToolStripMenuItem
            // 
            this.openFolderToolStripMenuItem.Name = "openFolderToolStripMenuItem";
            this.openFolderToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.openFolderToolStripMenuItem.Text = "Open Folder";
            this.openFolderToolStripMenuItem.Click += new System.EventHandler(this.openFolderToolStripMenuItem_Click);
            // 
            // m_button_SyncItem
            // 
            this.m_button_SyncItem.Enabled = false;
            this.m_button_SyncItem.Name = "m_button_SyncItem";
            this.m_button_SyncItem.Size = new System.Drawing.Size(139, 22);
            this.m_button_SyncItem.Text = "Sync Item";
            this.m_button_SyncItem.Click += new System.EventHandler(this.syncItemToolStripMenuItem_Click);
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
        private System.Windows.Forms.ComboBox m_comboBox_Mode;
        private System.Windows.Forms.Button m_button_ChangeDirectory2;
        private System.Windows.Forms.TextBox m_textBox_Directory2;
        private System.Windows.Forms.ToolStripMenuItem m_button_SyncItem;
    }
}

