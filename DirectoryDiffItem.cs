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
        FileMissing
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
            Path1 = path1;
            Path2 = path2;
        }

        private DirectoryDiffItem(string path1, string path2, DiffType type)
        {
            Path1 = path1;
            Path2 = path2;
            Type = type;
        }

        public string DirectoryName
        {
            get { return Path1.Substring(Path1.LastIndexOf("\\") + 1); }
        }

        public string PathValue
        {
            get
            {
                return Path1;
            }
        }

        public string Info
        {
            get
            {
                var label = Type == DiffType.FileMissing || Type == DiffType.FolderMissing ? " (+)" : "";
                return $"{Path.GetFileName(PathValue)}{label}";
            }
        }

        public bool IsDirectory
        {
            get
            {
                return Type == DiffType.FolderMissing || Type == DiffType.FolderDifferent;
            }
        }

        public Color Color
        {
            get
            {
                return Type == DiffType.FolderDifferent || Type == DiffType.FolderMissing ? Color.Blue : Color.Black;
            }
        }

        private void InitChildren()
        {
            if (Children == null)
            {
                Children = new List<IDirectoryInfo>();
            }
        }

        public long Populate()
        {
            if (Activity.Cancel == true)
            {
                return 0;
            }

            Activity.CurrentDirectory = null;
            Activity.DiffCount = 0;
            Activity.Cancel = false;

            DiffCount = 0;

            if (!Directory.Exists(Path2))
            {
                DiffCount++;
                Type = DiffType.FolderMissing;
                return DiffCount;
            }

            var path1Files = Directory.GetFiles(Path1, "*.*");

            var extensionFilter = new string[] { ".log", ".pdb" }.ToList();

            foreach (var path1File in path1Files)
            {
                var path2File = Utils.TranslatePath(path1File, Path2);

                if (extensionFilter.IndexOf(Path.GetExtension(path1File)) >= 0)
                    continue;

                try
                {
                    if (!File.Exists(path2File))
                    {
                        DiffCount++;
                        InitChildren();
                        Children.Add(new DirectoryDiffItem(path1File, path2File, DiffType.FileMissing));
                    }
                    else
                    {
                        var info1 = new FileInfo(path1File);
                        var info2 = new FileInfo(path2File);
                        
                        if (info1.Length != info2.Length)
                        {
                            DiffCount++;
                            InitChildren();
                            Children.Add(new DirectoryDiffItem(path1File, path2File, DiffType.FileDifferent));
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }

            var subDirectories = Directory.GetDirectories(Path1);

            foreach (var subDirectory1 in subDirectories)
            {
                var dirName = Path.GetFileName(subDirectory1);
                var subDirectory2 = Path.Combine(Path2, dirName);

                if (dirName.StartsWith("$") || dirName.StartsWith(".git") || dirName.StartsWith(".cache") || dirName == "Debug" || dirName == "Release")
                {
                    continue;
                }

                if (Activity.Cancel == true)
                {
                    return DiffCount;
                }

                Activity.CurrentDirectory = subDirectory1;

                if (!Directory.Exists(subDirectory2))
                {
                    InitChildren();
                    DiffCount++;
                    Children.Add(new DirectoryDiffItem(subDirectory1, subDirectory2, DiffType.FolderMissing));
                    continue;
                }

                var childInfo = new DirectoryDiffItem(subDirectory1, subDirectory2);

                var subDirDiffCount = childInfo.Populate();

                if (subDirDiffCount > 0)
                {
                    if (Children == null)
                        Children = new List<IDirectoryInfo>();

                    Children.Add(childInfo);
                    DiffCount += subDirDiffCount;
                }
            }

            return DiffCount;
        }
    }
}
