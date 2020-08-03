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

        private IDirectoryInfo m_DirectoryInfo = null;

        public FormMain()
        {
            InitializeComponent();

            //m_comboBox_Mode.Text = "Differences";

            UpdateForm();
        }

        private void UpdateForm()
        {
            m_textBox_Directory2.Visible = true;
            m_button_ChangeDirectory2.Visible = true;
            m_button_SyncItem.Visible = true;

            m_textBox_Directory2.Enabled = false;
            m_button_ChangeDirectory2.Enabled = false;
            m_button_SyncItem.Enabled = false;

            m_findLabel.Visible = false;
            m_findNameText.Visible = false;
            m_replaceWithLabel.Visible = false;
            m_replaceWithText.Visible = false;
            m_DirectoryReportText.Visible = false;

            switch (m_comboBox_Mode.Text)
            {
                case "Size Report":
                    break;

                case "Differences":
                    m_textBox_Directory2.Enabled = true;
                    m_button_ChangeDirectory2.Enabled = true;
                    m_button_SyncItem.Enabled = true;
                    break;

                case "Rename Files":
                    m_textBox_Directory2.Visible = false;
                    m_button_ChangeDirectory2.Visible = false;
                    m_button_SyncItem.Visible = false;
                    m_DirectoryReportText.Visible = false;

                    m_findLabel.Visible = true;
                    m_findNameText.Visible = true;
                    m_replaceWithLabel.Visible = true;
                    m_replaceWithText.Visible = true;
                    break;

                case "Folder List":
                    m_textBox_Directory2.Visible = false;
                    m_button_ChangeDirectory2.Visible = false;
                    m_button_SyncItem.Visible = false;
                    m_DirectoryReportText.Visible = true;

                    m_findLabel.Visible = false;
                    m_findNameText.Visible = false;
                    m_replaceWithLabel.Visible = false;
                    m_replaceWithText.Visible = false;
                    m_textBox_Depth.Text = "1";
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

                case "Rename Files":
                    RenameFiles(m_textBox_Directory.Text, m_findNameText.Text, m_replaceWithText.Text);
                    break;

                case "Folder List":
                    PopulateFolderList(m_textBox_Directory.Text);
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
            if (!Directory.Exists(m_DirectoryInfo.PathValue))
            {
                return;
            }

            DirectorySizeItem.Activity.Cancel = false;

            m_DirectoryInfo.Populate();

            m_bBusy = false;
        }

        private void RenameFiles(string path, string findName, string replaceName)
        {
            var files = Directory.GetFiles(path, $"*{findName}*.*");

            foreach (var name in files)
            {
                var newName = Path.GetFileName(name).Replace(findName, replaceName);

                try
                {
                    Directory.Move(name, Path.Combine(path, newName));
                }
                catch (Exception e)
                {

                }
            }

            foreach (var subDirectory in Directory.GetDirectories(path))
            {
                var dirName = Path.GetFileName(subDirectory);
                if (dirName.StartsWith("$") || dirName.ToLower() == "config" || dirName.ToLower() == "packages" || dirName.StartsWith("."))
                {
                    continue;
                }
                
                if (dirName.Contains(findName))
                {
                    var newName = dirName.Replace(findName, replaceName);

                    try
                    {
                        Directory.Move(Path.Combine(path, dirName), Path.Combine(path,newName));
                    }
                    catch (Exception e)
                    {

                    }

                    RenameFiles(Path.Combine(path, newName), findName, replaceName);
                }
            }
        }

        private void PopulateFolderList(string folder)
        {
            m_DirectoryReportText.Text = string.Empty;

            var directories = Directory.GetDirectories(folder);

            if (!directories.Any())
            {
                return;
            }

            var dirList = new List<string>();

            foreach (var directory in directories)
            {
                dirList.Add(Path.GetFileName(directory));
            }

            foreach(var name in dirList.OrderBy(x => x))
            {
                m_DirectoryReportText.AppendText($"{name}{Environment.NewLine}");
            }

            Clipboard.SetText(m_DirectoryReportText.Text);
        }

        private void DoPopulateDirectoryDiffInfo()
        {
            if (!Directory.Exists(m_DirectoryInfo.PathValue))
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

            var rootNode = m_DirectoryReport.Nodes.Add(m_DirectoryInfo.PathValue, m_DirectoryInfo.Info);
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
                if (DirectorySizeItem.Activity.Cancel)
                {
                    break;
                }
                var dirNode = treeNode.Nodes.Add(child.PathValue, child.Info);
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
                OpenFolder(dirInfo.PathValue);
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

            var obj2 = dirInfo.PathValue.Replace(m_textBox_Directory.Text, m_textBox_Directory2.Text);

            if (dirInfo != null && dirInfo.IsDirectory)
            {
                Utils.DirectoryCopy(dirInfo.PathValue, obj2, true);
            }
            else
            {
                File.Copy(dirInfo.PathValue, obj2, true);
            }
        }
    }
}
