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
            this.InitializeComponent();

            // m_comboBox_Mode.Text = "Differences";
            this.UpdateForm();
        }

        private void UpdateForm()
        {
            this.m_textBox_Directory2.Visible = true;
            this.m_button_ChangeDirectory2.Visible = true;
            this.m_button_SyncItem.Visible = true;

            this.m_textBox_Directory2.Enabled = false;
            this.m_button_ChangeDirectory2.Enabled = false;
            this.m_button_SyncItem.Enabled = false;

            this.m_findLabel.Visible = false;
            this.m_findNameText.Visible = false;
            this.m_replaceWithLabel.Visible = false;
            this.m_replaceWithText.Visible = false;
            this.m_DirectoryReportText.Visible = false;

            switch (this.m_comboBox_Mode.Text)
            {
                case "Size Report":
                    break;

                case "Differences":
                    this.m_textBox_Directory2.Enabled = true;
                    this.m_button_ChangeDirectory2.Enabled = true;
                    this.m_button_SyncItem.Enabled = true;
                    break;

                case "Rename Files":
                    this.m_textBox_Directory2.Visible = false;
                    this.m_button_ChangeDirectory2.Visible = false;
                    this.m_button_SyncItem.Visible = false;
                    this.m_DirectoryReportText.Visible = false;

                    this.m_findLabel.Visible = true;
                    this.m_findNameText.Visible = true;
                    this.m_replaceWithLabel.Visible = true;
                    this.m_replaceWithText.Visible = true;
                    break;

                case "Folder List":
                    this.m_textBox_Directory2.Visible = false;
                    this.m_button_ChangeDirectory2.Visible = false;
                    this.m_button_SyncItem.Visible = false;
                    this.m_DirectoryReportText.Visible = true;

                    this.m_findLabel.Visible = false;
                    this.m_findNameText.Visible = false;
                    this.m_replaceWithLabel.Visible = false;
                    this.m_replaceWithText.Visible = false;
                    this.m_textBox_Depth.Text = "1";
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
            this.m_textBox_Directory.Text = this.PromptForFolder();
        }

        private void m_button_ChangeDirectory2_Click(object sender, EventArgs e)
        {
            this.m_textBox_Directory2.Text = this.PromptForFolder();
        }

        private void m_button_Start_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(this.m_textBox_Directory.Text))
            {
                MessageBox.Show("Directory not found", "Error");
                return;
            }

            if (this.m_textBox_Directory2.Enabled && !Directory.Exists(this.m_textBox_Directory2.Text))
            {
                MessageBox.Show("Directory 2 not found", "Error");
                return;
            }

            this.m_button_Start.Visible = false;
            this.m_button_Cancel.Visible = true;

            this.m_DirectoryReport.Nodes.Clear();

            switch (this.m_comboBox_Mode.Text)
            {
                case "Size Report":
                    this.PopulateDirectoryInfo(this.m_textBox_Directory.Text);
                    break;

                case "Differences":
                    this.PopulateDifferencesInfo(this.m_textBox_Directory.Text, this.m_textBox_Directory2.Text);
                    break;

                case "Rename Files":
                    this.RenameFiles(this.m_textBox_Directory.Text, this.m_findNameText.Text, this.m_replaceWithText.Text);
                    break;

                case "Folder List":
                    this.PopulateFolderList(this.m_textBox_Directory.Text);
                    break;
            }

            this.m_button_Start.Visible = true;
            this.m_button_Cancel.Visible = false;
        }

        private void m_button_Cancel_Click(object sender, EventArgs e)
        {
            DirectorySizeItem.Activity.Cancel = true;
        }

        private void PopulateDirectoryInfo(string rootDirectory)
        {
            this.m_DirectoryInfo = new DirectorySizeItem(rootDirectory);

            var m = new MethodInvoker(this.DoPopulateDirectoryInfo);

            this.m_bBusy = true;

            var tag = m.BeginInvoke(null, null);

            while (this.m_bBusy)
            {
                Application.DoEvents();
                Thread.Sleep(50);

                this.Text = "Directory Size Reporter - " + DirectorySizeItem.Activity.CurrentDirectory;

                if (this.m_bClosing)
                {
                    this.Text.Replace("Directory Size Reporter - ", "Directory Reporter - (CLOSING) - ");
                }
            }

            m.EndInvoke(tag);

            if (!this.m_bClosing)
            {
                this.UpdateTree(int.Parse(this.m_textBox_Depth.Text));
            }

            this.Text = "Directory Reporter - ";

        }

        private void PopulateDifferencesInfo(string directory1, string directory2)
        {
            this.m_DirectoryInfo = new DirectoryDiffItem(directory1, directory2);

            var m = new MethodInvoker(this.DoPopulateDirectoryDiffInfo);

            this.m_bBusy = true;

            var tag = m.BeginInvoke(null, null);

            while (this.m_bBusy)
            {
                Application.DoEvents();
                Thread.Sleep(50);

                this.Text = $"Directory Differences - ({DirectoryDiffItem.Activity.DiffCount}) - " + DirectoryDiffItem.Activity.CurrentDirectory;

                if (this.m_bClosing)
                {
                    this.Text.Replace("Directory Differences - ", "Directory Differences - (CLOSING) - ");
                }
            }

            m.EndInvoke(tag);

            if (!this.m_bClosing)
            {
                this.UpdateTree(int.Parse(this.m_textBox_Depth.Text));
            }

            this.Text = "Directory Differences";
        }

        private void DoPopulateDirectoryInfo()
        {
            if (!Directory.Exists(this.m_DirectoryInfo.PathValue))
            {
                return;
            }

            DirectorySizeItem.Activity.Cancel = false;

            this.m_DirectoryInfo.Populate();

            this.m_bBusy = false;
        }

        private void RenameFiles(string path, string findName, string replaceName)
        {
            try
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
                            Directory.Move(Path.Combine(path, dirName), Path.Combine(path, newName));
                        }
                        catch (Exception e)
                        {

                        }

                        this.RenameFiles(Path.Combine(path, newName), findName, replaceName);
                    }
                }

            }
            catch (Exception ex)
            {
                Trace.TraceError($"RenameFiles Exception getting directory: {path}, ex: {ex.Message}");
                return;
            }
        }

        private void PopulateFolderList(string folder)
        {
            this.m_DirectoryReportText.Text = string.Empty;

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

            foreach (var name in dirList.OrderBy(x => x))
            {
                this.m_DirectoryReportText.AppendText($"{name}{Environment.NewLine}");
            }

            Clipboard.SetText(this.m_DirectoryReportText.Text);
        }

        private void DoPopulateDirectoryDiffInfo()
        {
            if (!Directory.Exists(this.m_DirectoryInfo.PathValue))
            {
                return;
            }

            DirectoryDiffItem.Activity.Cancel = false;

            this.m_DirectoryInfo.Populate();

            this.m_bBusy = false;
        }

        private void UpdateTree(int depth)
        {
            this.m_DirectoryReport.Nodes.Clear();

            var rootNode = this.m_DirectoryReport.Nodes.Add(this.m_DirectoryInfo.PathValue, this.m_DirectoryInfo.Info);
            rootNode.Tag = this.m_DirectoryInfo;

            this.PopulateTreeNode(rootNode, this.m_DirectoryInfo, depth);

            rootNode.Expand();
        }

        private void PopulateTreeNode(TreeNode treeNode, IDirectoryInfo directoryInfo, int depth)
        {
            if (directoryInfo == null || directoryInfo.Children == null)
            {
                return;
            }

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
                    this.PopulateTreeNode(dirNode, child, depth - 1);

                    if (this.m_comboBox_Mode.Text != "Size Report")
                    {
                        dirNode.Expand();
                    }
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
            var dirInfo = this.m_DirectoryReport.SelectedNode.Tag as IDirectoryInfo;

            if (dirInfo != null && dirInfo.IsDirectory)
            {
                OpenFolder(dirInfo.PathValue);
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.m_bClosing = true;
            DirectorySizeItem.Activity.Cancel = true;
            while (this.m_bBusy)
            {
                Application.DoEvents();
                Thread.Sleep(50);
            }

        }

        private void m_comboBox_Mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateForm();
        }

        private void syncItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dirInfo = this.m_DirectoryReport.SelectedNode.Tag as IDirectoryInfo;

            var obj2 = dirInfo.PathValue.Replace(this.m_textBox_Directory.Text, this.m_textBox_Directory2.Text);

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
