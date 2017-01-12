using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DirectoryReporter
{
    public partial class FormMain : Form
    {
        private bool m_bBusy = false;
        private bool m_bClosing = false;

        private DirectoryInfo m_DirectoryInfo = null;

        public FormMain()
        {
            InitializeComponent();

            
        }

        private void m_button_ChangeDirectory_Click(object sender, EventArgs e)
        {
            var dirForm = new FolderBrowserDialog();

            dirForm.ShowNewFolderButton = false;

            var result = dirForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                m_textBox_Directory.Text = dirForm.SelectedPath;
            }
        }

        private void m_button_Start_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(m_textBox_Directory.Text))
            {
                MessageBox.Show("Directory not found", "Error");
                return;
            }

            m_button_Start.Visible = false;
            m_button_Cancel.Visible = true;

            m_DirectoryReport.Nodes.Clear();

            PopulateDirectoryInfo(m_textBox_Directory.Text);

            m_button_Start.Visible = true;
            m_button_Cancel.Visible = false;

        }

        private void m_button_Cancel_Click(object sender, EventArgs e)
        {
            DirectoryInfo.Activity.Cancel = true;
        }

        private void PopulateDirectoryInfo(string rootDirectory)
        {
            m_DirectoryInfo = new DirectoryInfo(rootDirectory);

            var m = new MethodInvoker(DoPopulateDirectoryInfo);

            m_bBusy = true;

            var tag = m.BeginInvoke(null, null);

            while (m_bBusy)
            {
                Application.DoEvents();
                Thread.Sleep(50);

                this.Text = "Directory Reporter - " + DirectoryInfo.Activity.CurrentDirectory;

                if (m_bClosing)
                {
                    this.Text.Replace("Directory Reporter - ", "Directory Reporter - (CLOSING) - ");
                }
            }

            m.EndInvoke(tag);

            if (!m_bClosing)
                UpdateTree(int.Parse(m_textBox_Depth.Text));

            this.Text = "Directory Reporter";
        }

        private void DoPopulateDirectoryInfo()
        {
            if (!Directory.Exists(m_DirectoryInfo.Path))
            {
                return;
            }

            DirectoryInfo.Activity.Cancel = false;

            m_DirectoryInfo.Populate();

            m_bBusy = false;
        }


        private void UpdateTree(int depth)
        {
            m_DirectoryReport.Nodes.Clear();

            var rootNode = m_DirectoryReport.Nodes.Add(m_DirectoryInfo.Path, FormatDirectoryInfo(m_DirectoryInfo));
            rootNode.Tag = m_DirectoryInfo;

            PopulateTreeNode(rootNode, m_DirectoryInfo, depth);

            rootNode.Expand();
        }

        private void PopulateTreeNode(TreeNode treeNode, DirectoryInfo directoryInfo, int depth)
        {
            foreach (var subDirectory in directoryInfo.SubDirectories)
            {
                var dirNode = treeNode.Nodes.Add(subDirectory.Path, FormatDirectoryInfo(subDirectory));
                dirNode.Tag = subDirectory;
                if (subDirectory.TotalSize > 1000000000)
                {
                    dirNode.ForeColor = Color.Red;
                }
                if (depth > 0)
                {
                    PopulateTreeNode(dirNode, subDirectory, depth - 1);
                }
            }
        }

        private static string FormatDirectoryInfo(DirectoryInfo directoryInfo)
        {
            return string.Format("{0}({1})", directoryInfo.DirectoryName.PadRight(20), directoryInfo.TotalSize.ToFileSize());
        }

        public static void OpenFolder(string sFolder)
        {
            var process = new Process();
            process.StartInfo.FileName = System.Environment.GetEnvironmentVariable("windir") + "\\explorer.exe";
            process.StartInfo.Arguments = "/n,/root," + sFolder;
            process.Start();
        }

        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dirInfo = m_DirectoryReport.SelectedNode.Tag as DirectoryInfo;

            if (dirInfo != null)
                OpenFolder(dirInfo.Path);
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_bClosing = true;
            DirectoryInfo.Activity.Cancel = true;
            while (m_bBusy)
            {
                Application.DoEvents();
                Thread.Sleep(50);
            }

        }
        
    }
}
