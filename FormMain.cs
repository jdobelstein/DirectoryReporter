using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using Delimon.Win32.IO;
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

        private IDirectoryInfo m_DirectoryInfo = null;

        public FormMain()
        {
            InitializeComponent();

            //m_comboBox_Mode.Text = "Differences";

            UpdateForm();
        }

        private void UpdateForm()
        {
            switch (m_comboBox_Mode.Text)
            {
                case "Size Report":
                    m_textBox_Directory2.Enabled = false;
                    m_button_ChangeDirectory2.Enabled = false;
                    m_button_SyncItem.Enabled = false;
                    break;

                case "Differences":
                    m_textBox_Directory2.Enabled = true;
                    m_button_ChangeDirectory2.Enabled = true;
                    m_button_SyncItem.Enabled = true;
                    break;
            }
        }
        private string PromptForFolder()
        {
            var dirForm = new FolderBrowserDialog();

            dirForm.ShowNewFolderButton = false;

            var result = dirForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                return dirForm.SelectedPath;
            }

            return "";
        }
        private void m_button_ChangeDirectory_Click(object sender, EventArgs e)
        {
            m_textBox_Directory.Text = PromptForFolder();
        }

        private void m_button_ChangeDirectory2_Click(object sender, EventArgs e)
        {
            m_textBox_Directory2.Text = PromptForFolder();
        }

        private void m_button_Start_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(m_textBox_Directory.Text))
            {
                MessageBox.Show("Directory not found", "Error");
                return;
            }

            if (m_textBox_Directory2.Enabled && !Directory.Exists(m_textBox_Directory2.Text))
            {
                MessageBox.Show("Directory 2 not found", "Error");
                return;
            }

            m_button_Start.Visible = false;
            m_button_Cancel.Visible = true;

            m_DirectoryReport.Nodes.Clear();

            switch (m_comboBox_Mode.Text)
            {
                case "Size Report":
                    PopulateDirectoryInfo(m_textBox_Directory.Text);
                    break;

                case "Differences":
                    PopulateDifferencesInfo(m_textBox_Directory.Text, m_textBox_Directory2.Text);
                    break;
            }


            m_button_Start.Visible = true;
            m_button_Cancel.Visible = false;

        }

        private void m_button_Cancel_Click(object sender, EventArgs e)
        {
            DirectorySizeItem.Activity.Cancel = true;
        }

        private void PopulateDirectoryInfo(string rootDirectory)
        {
            m_DirectoryInfo = new DirectorySizeItem(rootDirectory);

            var m = new MethodInvoker(DoPopulateDirectoryInfo);

            m_bBusy = true;

            var tag = m.BeginInvoke(null, null);

            while (m_bBusy)
            {
                Application.DoEvents();
                Thread.Sleep(50);

                this.Text = "Directory Size Reporter - " + DirectorySizeItem.Activity.CurrentDirectory;

                if (m_bClosing)
                {
                    this.Text.Replace("Directory Size Reporter - ", "Directory Reporter - (CLOSING) - ");
                }
            }

            m.EndInvoke(tag);

            if (!m_bClosing)
                UpdateTree(int.Parse(m_textBox_Depth.Text));

            this.Text = "Directory Reporter - ";
        }

        private void PopulateDifferencesInfo(string directory1, string directory2)
        {
            m_DirectoryInfo = new DirectoryDiffItem(directory1, directory2);

            var m = new MethodInvoker(DoPopulateDirectoryDiffInfo);

            m_bBusy = true;

            var tag = m.BeginInvoke(null, null);

            while (m_bBusy)
            {
                Application.DoEvents();
                Thread.Sleep(50);

                this.Text = $"Directory Differences - ({DirectoryDiffItem.Activity.DiffCount}) - " + DirectoryDiffItem.Activity.CurrentDirectory;

                if (m_bClosing)
                {
                    this.Text.Replace("Directory Differences - ", "Directory Differences - (CLOSING) - ");
                }
            }

            m.EndInvoke(tag);

            if (!m_bClosing)
                UpdateTree(int.Parse(m_textBox_Depth.Text));

            this.Text = "Directory Differences";
        }

        private void DoPopulateDirectoryInfo()
        {
            if (!Directory.Exists(m_DirectoryInfo.Path))
            {
                return;
            }

            DirectorySizeItem.Activity.Cancel = false;

            m_DirectoryInfo.Populate();

            m_bBusy = false;
        }

        private void DoPopulateDirectoryDiffInfo()
        {
            if (!Directory.Exists(m_DirectoryInfo.Path))
            {
                return;
            }

            DirectoryDiffItem.Activity.Cancel = false;

            m_DirectoryInfo.Populate();

            m_bBusy = false;
        }


        private void UpdateTree(int depth)
        {
            m_DirectoryReport.Nodes.Clear();

            var rootNode = m_DirectoryReport.Nodes.Add(m_DirectoryInfo.Path, m_DirectoryInfo.Info);
            rootNode.Tag = m_DirectoryInfo;
            
            PopulateTreeNode(rootNode, m_DirectoryInfo, depth);

            rootNode.Expand();
        }

        private void PopulateTreeNode(TreeNode treeNode, IDirectoryInfo directoryInfo, int depth)
        {
            if (directoryInfo == null || directoryInfo.Children ==  null)
                return;

            foreach (var child in directoryInfo.Children)
            {
                var dirNode = treeNode.Nodes.Add(child.Path, child.Info);
                dirNode.Tag = child;
                dirNode.ForeColor = child.Color;

                /*
                if (subDirectory.TotalSize > 1000000000)
                {
                    dirNode.ForeColor = Color.Red;
                }
                */
                if (depth > 0)
                {
                    PopulateTreeNode(dirNode, child, depth - 1);

                    if (m_comboBox_Mode.Text != "Size Report")
                        dirNode.Expand();
                }
            }
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
            var dirInfo = m_DirectoryReport.SelectedNode.Tag as IDirectoryInfo;

            if (dirInfo != null && dirInfo.IsDirectory)
                OpenFolder(dirInfo.Path);
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_bClosing = true;
            DirectorySizeItem.Activity.Cancel = true;
            while (m_bBusy)
            {
                Application.DoEvents();
                Thread.Sleep(50);
            }

        }

        private void m_comboBox_Mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void syncItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dirInfo = m_DirectoryReport.SelectedNode.Tag as IDirectoryInfo;

            var obj2 = dirInfo.Path.Replace(m_textBox_Directory.Text, m_textBox_Directory2.Text);

            if (dirInfo != null && dirInfo.IsDirectory)
            {
                Utils.DirectoryCopy(dirInfo.Path, obj2, true);
            }
            else
            {
                File.Copy(dirInfo.Path, obj2, true);
            }
        }
    }
}
