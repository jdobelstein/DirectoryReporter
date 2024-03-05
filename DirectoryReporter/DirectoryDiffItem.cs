using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;

namespace DirectoryReporter
{
    public enum DiffType
    {
        FolderDifferent,
        FolderMissing,
        FileDifferent,
        FileMissing,
    }

    public class Difference
    {
        public string PathValue { get; set; }

        public DiffType Type { get; set; }
    }

    public class DirectoryDiffItem : IDirectoryInfo
    {
        public string Path1 { get; private set; }

        public string Path2 { get; private set; }

        public long DiffCount { get; private set; }

        public DiffType Type { get; set; }

        public List<IDirectoryInfo> Children { get; private set; }

        public static class Activity
        {
            public static string CurrentDirectory { get; set; }

            public static int DiffCount { get; set; }

            public static bool Cancel { get; set; }
        }

        public DirectoryDiffItem(string path1, string path2)
        {
            this.Path1 = path1;
            this.Path2 = path2;
        }

        private DirectoryDiffItem(string path1, string path2, DiffType type)
        {
            this.Path1 = path1;
            this.Path2 = path2;
            this.Type = type;
        }

        public string DirectoryName
        {
            get { return this.Path1.Substring(this.Path1.LastIndexOf("\\") + 1); }
        }

        public string PathValue
        {
            get
            {
                return this.Path1;
            }
        }

        public string Info
        {
            get
            {
                var label = this.Type == DiffType.FileMissing || this.Type == DiffType.FolderMissing ? " (+)" : "";
                return $"{Path.GetFileName(this.PathValue)}{label}";
            }
        }

        public bool IsDirectory
        {
            get
            {
                return this.Type == DiffType.FolderMissing || this.Type == DiffType.FolderDifferent;
            }
        }

        public Color Color
        {
            get
            {
                return this.Type == DiffType.FolderDifferent || this.Type == DiffType.FolderMissing ? Color.Blue : Color.Black;
            }
        }

        private void InitChildren()
        {
            if (this.Children == null)
            {
                this.Children = new List<IDirectoryInfo>();
            }
        }

        public long Populate()
        {
            if (Activity.Cancel)
            {
                return 0;
            }

            Activity.CurrentDirectory = null;
            Activity.DiffCount = 0;
            Activity.Cancel = false;

            this.DiffCount = 0;

            if (!Directory.Exists(this.Path2))
            {
                this.DiffCount++;
                this.Type = DiffType.FolderMissing;
                return this.DiffCount;
            }

            var path1Files = Directory.GetFiles(this.Path1, "*.*");

            var extensionFilter = new string[] { ".log", ".pdb" }.ToList();

            foreach (var path1File in path1Files)
            {
                var path2File = Utils.TranslatePath(path1File, this.Path2);

                if (extensionFilter.IndexOf(Path.GetExtension(path1File)) >= 0)
                {
                    continue;
                }

                try
                {
                    if (!File.Exists(path2File))
                    {
                        this.DiffCount++;
                        this.InitChildren();
                        this.Children.Add(new DirectoryDiffItem(path1File, path2File, DiffType.FileMissing));
                    }
                    else
                    {
                        var info1 = new FileInfo(path1File);
                        var info2 = new FileInfo(path2File);

                        if (info1.Length != info2.Length)
                        {
                            this.DiffCount++;
                            this.InitChildren();
                            this.Children.Add(new DirectoryDiffItem(path1File, path2File, DiffType.FileDifferent));
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }

            var subDirectories = Directory.GetDirectories(this.Path1);

            foreach (var subDirectory1 in subDirectories)
            {
                var dirName = Path.GetFileName(subDirectory1);
                var subDirectory2 = Path.Combine(this.Path2, dirName);

                if (dirName.StartsWith("$") || dirName.StartsWith(".git") || dirName.StartsWith(".cache") || dirName == "Debug" || dirName == "Release")
                {
                    continue;
                }

                if (Activity.Cancel)
                {
                    return this.DiffCount;
                }

                Activity.CurrentDirectory = subDirectory1;

                if (!Directory.Exists(subDirectory2))
                {
                    this.InitChildren();
                    this.DiffCount++;
                    this.Children.Add(new DirectoryDiffItem(subDirectory1, subDirectory2, DiffType.FolderMissing));
                    continue;
                }

                var childInfo = new DirectoryDiffItem(subDirectory1, subDirectory2);

                var subDirDiffCount = childInfo.Populate();

                if (subDirDiffCount > 0)
                {
                    if (this.Children == null)
                    {
                        this.Children = new List<IDirectoryInfo>();
                    }

                    this.Children.Add(childInfo);
                    this.DiffCount += subDirDiffCount;
                }
            }

            return this.DiffCount;
        }
    }
}
