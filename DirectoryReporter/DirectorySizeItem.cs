using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace DirectoryReporter
{
    public interface IDirectoryInfo
    {
        string PathValue { get; }

        string Info { get; }

        Color Color { get; }

        bool IsDirectory { get; }

        List<IDirectoryInfo> Children { get; }

        long Populate();
    }

    public class DirectorySizeItem : IDirectoryInfo
    {
        public string PathValue { get; private set; }

        public long FilesSize { get; private set; }

        public long TotalSize { get; private set; }

        public List<IDirectoryInfo> Children { get; private set; }

        public bool IsDirectory
        {
            get
            {
                return true;
            }
        }

        public string Info
        {
            get
            {
                return string.Format("{0}({1})", System.IO.Path.GetFileName(this.PathValue).PadRight(20), this.TotalSize.ToFileSize());
            }
        }

        public Color Color
        {
            get
            {
                return this.TotalSize > 1000000000 ? Color.Red : Color.Black;
            }
        }

        public static class Activity
        {
            public static string CurrentDirectory { get; set; }

            public static bool Cancel { get; set; }
        }

        public DirectorySizeItem(string path)
        {
            this.PathValue = path;

            this.FilesSize = 0;
            this.TotalSize = 0;

            this.Children = new List<IDirectoryInfo>();
        }

        public long Populate()
        {
            if (Activity.Cancel)
            {
                return 0;
            }

            // Activity.CurrentDirectory = null;
            // Activity.Cancel = false;
            this.FilesSize = GetDirectorySize(this.PathValue);
            this.TotalSize = this.FilesSize;

            foreach (var subDirectory in Directory.GetDirectories(this.PathValue))
            {
                var dirName = System.IO.Path.GetFileName(subDirectory);
                if (dirName.StartsWith("$"))
                {
                    continue;
                }

                if (Activity.Cancel)
                {
                    return this.TotalSize;
                }

                Activity.CurrentDirectory = subDirectory;

                var subDirectoryInfo = new DirectorySizeItem(subDirectory);

                try
                {
                    subDirectoryInfo.Populate();
                    this.TotalSize += subDirectoryInfo.TotalSize;
                    this.Children.Add(subDirectoryInfo);
                }
                catch (Exception e)
                {
                    var err = e.Message;
                }
            }

            return this.TotalSize;
        }

        static long GetDirectorySize(string baseDirectory)
        {
            var fileObj = Directory.GetFiles(baseDirectory, "*.*");
            long totalSize = 0;
            foreach (var name in fileObj)
            {
                var info = new FileInfo(name);
                totalSize += info.Length;
            }

            return totalSize;
        }
    }

    public static class ExtensionMethods
    {
        public static string ToFileSize(this long l)
        {
            return String.Format(new FileSizeFormatProvider(), "{0:fs}", l);
        }
    }

    public class FileSizeFormatProvider : IFormatProvider, ICustomFormatter
    {
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
            {
                return this;
            }

            return null;
        }

        private const string fileSizeFormat = "fs";
        private const Decimal OneKiloByte = 1024M;
        private const Decimal OneMegaByte = OneKiloByte * 1024M;
        private const Decimal OneGigaByte = OneMegaByte * 1024M;

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (format == null || !format.StartsWith(fileSizeFormat))
            {
                return defaultFormat(format, arg, formatProvider);
            }

            if (arg is string)
            {
                return defaultFormat(format, arg, formatProvider);
            }

            Decimal size;

            try
            {
                size = Convert.ToDecimal(arg);
            }
            catch (InvalidCastException)
            {
                return defaultFormat(format, arg, formatProvider);
            }

            string suffix;
            if (size > OneGigaByte)
            {
                size /= OneGigaByte;
                suffix = "GB";
            }
            else if (size > OneMegaByte)
            {
                size /= OneMegaByte;
                suffix = "MB";
            }
            else if (size > OneKiloByte)
            {
                size /= OneKiloByte;
                suffix = "kB";
            }
            else
            {
                suffix = " B";
            }

            string precision = format.Substring(2);
            if (String.IsNullOrEmpty(precision))
            {
                precision = "2";
            }

            return String.Format("{0:N" + precision + "}{1}", size, suffix);

        }

        private static string defaultFormat(string format, object arg, IFormatProvider formatProvider)
        {
            IFormattable formattableArg = arg as IFormattable;
            if (formattableArg != null)
            {
                return formattableArg.ToString(format, formatProvider);
            }

            return arg.ToString();
        }

    }
}
