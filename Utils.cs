using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryReporter
{
    public static class Utils
    {
        public static string GetSameEnding(string s1, string s2)
        {
            var s1Pos = s1.Length - 1;
            var s2Pos = s2.Length - 1;

            while (true)
            {
                if (s1Pos == 0 || s2Pos == 0 || s1[s1Pos] != s2[s2Pos])
                {
                    break;
                }

                s1Pos--; s2Pos--;
            }

            return s1.Substring(s1Pos);
        }

        public static string TranslatePath(string path1File, string path2Base)
        {
            var obj1Name = Path.GetFileName(path1File);
            return Path.Combine(path2Base, obj1Name);
        }

        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, bool overwrite = true)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, overwrite);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs, overwrite);
                }
            }
        }
    }

    internal static class NativeMethods
    {
        internal const int FILE_ATTRIBUTE_ARCHIVE = 0x20;
        internal const int INVALID_FILE_ATTRIBUTES = -1;

        internal const int FILE_READ_DATA = 0x0001;
        internal const int FILE_WRITE_DATA = 0x0002;
        internal const int FILE_APPEND_DATA = 0x0004;
        internal const int FILE_READ_EA = 0x0008;
        internal const int FILE_WRITE_EA = 0x0010;

        internal const int FILE_READ_ATTRIBUTES = 0x0080;
        internal const int FILE_WRITE_ATTRIBUTES = 0x0100;

        internal const int FILE_SHARE_NONE = 0x00000000;
        internal const int FILE_SHARE_READ = 0x00000001;

        internal const int FILE_ATTRIBUTE_DIRECTORY = 0x10;

        internal const long FILE_GENERIC_WRITE = STANDARD_RIGHTS_WRITE |
                                                    FILE_WRITE_DATA |
                                                    FILE_WRITE_ATTRIBUTES |
                                                    FILE_WRITE_EA |
                                                    FILE_APPEND_DATA |
                                                    SYNCHRONIZE;

        internal const long FILE_GENERIC_READ = STANDARD_RIGHTS_READ |
                                                FILE_READ_DATA |
                                                FILE_READ_ATTRIBUTES |
                                                FILE_READ_EA |
                                                SYNCHRONIZE;

        internal const long READ_CONTROL = 0x00020000L;
        internal const long STANDARD_RIGHTS_READ = READ_CONTROL;
        internal const long STANDARD_RIGHTS_WRITE = READ_CONTROL;

        internal const long SYNCHRONIZE = 0x00100000L;

        internal const int CREATE_NEW = 1;
        internal const int CREATE_ALWAYS = 2;
        internal const int OPEN_EXISTING = 3;

        internal const int MAX_PATH = 260;
        internal const int MAX_ALTERNATE = 14;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct WIN32_FIND_DATA
        {
            public System.IO.FileAttributes dwFileAttributes;
            public FILETIME ftCreationTime;
            public FILETIME ftLastAccessTime;
            public FILETIME ftLastWriteTime;
            public uint nFileSizeHigh; // changed all to uint, otherwise you run into unexpected overflow
            public uint nFileSizeLow;  // |
            public uint dwReserved0;   // |
            public uint dwReserved1;   // v
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public string cFileName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_ALTERNATE)]
            public string cAlternate;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WIN32_FILE_ATTRIBUTE_DATA
        {
            public FileAttributes dwFileAttributes;
            public FILETIME ftCreationTime;
            public FILETIME ftLastAccessTime;
            public FILETIME ftLastWriteTime;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
        }

        public enum GET_FILEEX_INFO_LEVELS
        {
            GetFileExInfoStandard,
            GetFileExMaxInfoLevel,
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern SafeFileHandle CreateFile(string lpFileName, int dwDesiredAccess, int dwShareMode, IntPtr lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool CopyFileW(string lpExistingFileName, string lpNewFileName, bool bFailIfExists);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int GetFileAttributesW(string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool GetFileAttributesEx(string lpFileName, GET_FILEEX_INFO_LEVELS fInfoLevelId, out WIN32_FILE_ATTRIBUTE_DATA fileData);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool DeleteFileW(string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool MoveFileW(string lpExistingFileName, string lpNewFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool SetFileTime(SafeFileHandle hFile, ref long lpCreationTime, ref long lpLastAccessTime, ref long lpLastWriteTime);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool GetFileTime(SafeFileHandle hFile, ref long lpCreationTime, ref long lpLastAccessTime, ref long lpLastWriteTime);

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr FindFirstFile(string lpFileName, out WIN32_FIND_DATA lpFindFileData);

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool FindNextFile(IntPtr hFindFile, out WIN32_FIND_DATA lpFindFileData);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool FindClose(IntPtr hFindFile);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool RemoveDirectory(string path);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool CreateDirectory(string lpPathName, IntPtr lpSecurityAttributes);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int SetFileAttributesW(string lpFileName, int fileAttributes);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]

        internal static extern int GetLongPathName([MarshalAs(UnmanagedType.LPTStr)] string path, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder longPath, int longPathLength);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetShortPathName([MarshalAs(UnmanagedType.LPTStr)] string path, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder shortPath, int shortPathLength);

    }

    public static class LongFile
    {
        public static string GetShortPathName(string path)
        {
            var holdPos = path.IndexOf(@"\\");
            StringBuilder shortPath = new StringBuilder(255);
            NativeMethods.GetShortPathName(path, shortPath, shortPath.Capacity);
            return shortPath.ToString();
        }

        private const int MAX_PATH = 260;

        public static bool Exists(string path)
        {
            if (path.Length < MAX_PATH)
            {
                return System.IO.File.Exists(path);
            }

            var attr = NativeMethods.GetFileAttributesW(GetWin32LongPath(path));
            return (attr != NativeMethods.INVALID_FILE_ATTRIBUTES && ((attr & NativeMethods.FILE_ATTRIBUTE_ARCHIVE) == NativeMethods.FILE_ATTRIBUTE_ARCHIVE));
        }

        public static long GetLength(string path)
        {
            if (path.Length < MAX_PATH)
            {
                var fileInfo = new FileInfo(path);
                return fileInfo.Length;
            }

            // return NativeMethods.fso.GetFile(path).Size;

            // Check path here
            NativeMethods.WIN32_FILE_ATTRIBUTE_DATA fileData;

            // Append special suffix \\?\ to allow path lengths up to 32767
            path = "\\\\?\\" + path;

            if (!NativeMethods.GetFileAttributesEx(path,
                    NativeMethods.GET_FILEEX_INFO_LEVELS.GetFileExInfoStandard, out fileData))
            {
                throw new Win32Exception();
            }

            return (long)(((ulong)fileData.nFileSizeHigh << 32) +
                           (ulong)fileData.nFileSizeLow);

        }

        public static void Delete(string path)
        {
            if (path.Length < MAX_PATH) System.IO.File.Delete(path);
            else
            {
                bool ok = NativeMethods.DeleteFileW(GetWin32LongPath(path));
                if (!ok)
                {
                    ThrowWin32Exception();
                }
            }
        }

        public static void AppendAllText(string path, string contents)
        {
            AppendAllText(path, contents, Encoding.Default);
        }

        public static void AppendAllText(string path, string contents, Encoding encoding)
        {
            if (path.Length < MAX_PATH)
            {
                System.IO.File.AppendAllText(path, contents, encoding);
            }
            else
            {
                var fileHandle = CreateFileForAppend(GetWin32LongPath(path));
                using (var fs = new System.IO.FileStream(fileHandle, System.IO.FileAccess.Write))
                {
                    var bytes = encoding.GetBytes(contents);
                    fs.Position = fs.Length;
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }

        public static void WriteAllText(string path, string contents)
        {
            WriteAllText(path, contents, Encoding.Default);
        }

        public static void WriteAllText(string path, string contents, Encoding encoding)
        {
            if (path.Length < MAX_PATH)
            {
                System.IO.File.WriteAllText(path, contents, encoding);
            }
            else
            {
                var fileHandle = CreateFileForWrite(GetWin32LongPath(path));

                using (var fs = new System.IO.FileStream(fileHandle, System.IO.FileAccess.Write))
                {
                    var bytes = encoding.GetBytes(contents);
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }

        public static void WriteAllBytes(string path, byte[] bytes)
        {
            if (path.Length < MAX_PATH)
            {
                System.IO.File.WriteAllBytes(path, bytes);
            }
            else
            {
                var fileHandle = CreateFileForWrite(GetWin32LongPath(path));

                using (var fs = new System.IO.FileStream(fileHandle, System.IO.FileAccess.Write))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }

        public static void Copy(string sourceFileName, string destFileName)
        {
            Copy(sourceFileName, destFileName, false);
        }

        public static void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            if (sourceFileName.Length < MAX_PATH && (destFileName.Length < MAX_PATH)) System.IO.File.Copy(sourceFileName, destFileName, overwrite);
            else
            {
                var ok = NativeMethods.CopyFileW(GetWin32LongPath(sourceFileName), GetWin32LongPath(destFileName), !overwrite);
                if (!ok)
                {
                    ThrowWin32Exception();
                }
            }
        }

        public static void Move(string sourceFileName, string destFileName)
        {
            if (sourceFileName.Length < MAX_PATH && (destFileName.Length < MAX_PATH)) System.IO.File.Move(sourceFileName, destFileName);
            else
            {
                var ok = NativeMethods.MoveFileW(GetWin32LongPath(sourceFileName), GetWin32LongPath(destFileName));
                if (!ok)
                {
                    ThrowWin32Exception();
                }
            }
        }

        public static string ReadAllText(string path)
        {
            return ReadAllText(path, Encoding.Default);
        }

        public static string ReadAllText(string path, Encoding encoding)
        {
            if (path.Length < MAX_PATH) { return System.IO.File.ReadAllText(path, encoding); }
            var fileHandle = GetFileHandle(GetWin32LongPath(path));

            using (var fs = new System.IO.FileStream(fileHandle, System.IO.FileAccess.Read))
            {
                var data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                return encoding.GetString(data);
            }
        }

        public static string[] ReadAllLines(string path)
        {
            return ReadAllLines(path, Encoding.Default);
        }

        public static string[] ReadAllLines(string path, Encoding encoding)
        {
            if (path.Length < MAX_PATH) { return System.IO.File.ReadAllLines(path, encoding); }
            var fileHandle = GetFileHandle(GetWin32LongPath(path));

            using (var fs = new System.IO.FileStream(fileHandle, System.IO.FileAccess.Read))
            {
                var data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                var str = encoding.GetString(data);
                if (str.Contains("\r"))
                {
                    return str.Split(new[] { "\r\n" }, StringSplitOptions.None);
                }

                return str.Split('\n');
            }
        }

        public static byte[] ReadAllBytes(string path)
        {
            if (path.Length < MAX_PATH)
            {
                return System.IO.File.ReadAllBytes(path);
            }

            var fileHandle = GetFileHandle(GetWin32LongPath(path));

            using (var fs = new System.IO.FileStream(fileHandle, System.IO.FileAccess.Read))
            {
                var data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                return data;
            }
        }

        public static void SetAttributes(string path, FileAttributes attributes)
        {
            if (path.Length < MAX_PATH)
            {
                System.IO.File.SetAttributes(path, attributes);
            }
            else
            {
                var longFilename = GetWin32LongPath(path);
                NativeMethods.SetFileAttributesW(longFilename, (int)attributes);
            }
        }

        #region Helper methods

        private static SafeFileHandle CreateFileForWrite(string filename)
        {
            if (filename.Length >= MAX_PATH)
            {
                filename = GetWin32LongPath(filename);
            }

            SafeFileHandle hfile = NativeMethods.CreateFile(filename, (int)NativeMethods.FILE_GENERIC_WRITE, NativeMethods.FILE_SHARE_NONE, IntPtr.Zero, NativeMethods.CREATE_ALWAYS, 0, IntPtr.Zero);
            if (hfile.IsInvalid)
            {
                ThrowWin32Exception();
            }

            return hfile;
        }

        private static SafeFileHandle CreateFileForAppend(string filename)
        {
            if (filename.Length >= MAX_PATH)
            {
                filename = GetWin32LongPath(filename);
            }

            SafeFileHandle hfile = NativeMethods.CreateFile(filename, (int)NativeMethods.FILE_GENERIC_WRITE, NativeMethods.FILE_SHARE_NONE, IntPtr.Zero, NativeMethods.CREATE_NEW, 0, IntPtr.Zero);
            if (hfile.IsInvalid)
            {
                hfile = NativeMethods.CreateFile(filename, (int)NativeMethods.FILE_GENERIC_WRITE, NativeMethods.FILE_SHARE_NONE, IntPtr.Zero, NativeMethods.OPEN_EXISTING, 0, IntPtr.Zero);
                if (hfile.IsInvalid)
                {
                    ThrowWin32Exception();
                }
            }

            return hfile;
        }

        internal static SafeFileHandle GetFileHandle(string filename)
        {
            if (filename.Length >= MAX_PATH)
            {
                filename = GetWin32LongPath(filename);
            }

            SafeFileHandle hfile = NativeMethods.CreateFile(filename, (int)NativeMethods.FILE_GENERIC_READ, NativeMethods.FILE_SHARE_READ, IntPtr.Zero, NativeMethods.OPEN_EXISTING, 0, IntPtr.Zero);
            if (hfile.IsInvalid)
            {
                ThrowWin32Exception();
            }

            return hfile;
        }

        internal static SafeFileHandle GetFileHandleWithWrite(string filename)
        {
            if (filename.Length >= MAX_PATH)
            {
                filename = GetWin32LongPath(filename);
            }

            SafeFileHandle hfile = NativeMethods.CreateFile(filename, (int)(NativeMethods.FILE_GENERIC_READ | NativeMethods.FILE_GENERIC_WRITE | NativeMethods.FILE_WRITE_ATTRIBUTES), NativeMethods.FILE_SHARE_NONE, IntPtr.Zero, NativeMethods.OPEN_EXISTING, 0, IntPtr.Zero);
            if (hfile.IsInvalid)
            {
                ThrowWin32Exception();
            }

            return hfile;
        }

        public static System.IO.FileStream GetFileStream(string filename, FileAccess access = FileAccess.Read)
        {
            var longFilename = GetWin32LongPath(filename);
            SafeFileHandle hfile;
            if (access == FileAccess.Write)
            {
                hfile = NativeMethods.CreateFile(longFilename, (int)(NativeMethods.FILE_GENERIC_READ | NativeMethods.FILE_GENERIC_WRITE | NativeMethods.FILE_WRITE_ATTRIBUTES), NativeMethods.FILE_SHARE_NONE, IntPtr.Zero, NativeMethods.OPEN_EXISTING, 0, IntPtr.Zero);
            }
            else
            {
                hfile = NativeMethods.CreateFile(longFilename, (int)NativeMethods.FILE_GENERIC_READ, NativeMethods.FILE_SHARE_READ, IntPtr.Zero, NativeMethods.OPEN_EXISTING, 0, IntPtr.Zero);
            }

            if (hfile.IsInvalid)
            {
                ThrowWin32Exception();
            }

            return new System.IO.FileStream(hfile, access);
        }

        [DebuggerStepThrough]
        public static void ThrowWin32Exception()
        {
            int code = Marshal.GetLastWin32Error();
            if (code != 0)
            {
                throw new System.ComponentModel.Win32Exception(code);
            }
        }

        public static string GetWin32LongPath(string path)
        {
            if (path.StartsWith(@"\\?\"))
            {
                return path;
            }

            if (path.StartsWith("\\"))
            {
                path = @"\\?\UNC\" + path.Substring(2);
            }
            else if (path.Contains(":"))
            {
                path = @"\\?\" + path;
            }
            else
            {
                var currdir = Environment.CurrentDirectory;
                path = Combine(currdir, path);
                while (path.Contains("\\.\\"))
                {
                    path = path.Replace("\\.\\", "\\");
                }

                path = @"\\?\" + path;
            }

            return path.TrimEnd('.'); ;
        }

        private static string Combine(string path1, string path2)
        {
            return path1.TrimEnd('\\') + "\\" + path2.TrimStart('\\').TrimEnd('.'); ;
        }

        #endregion

        public static void SetCreationTime(string path, DateTime creationTime)
        {
            long cTime = 0;
            long aTime = 0;
            long wTime = 0;

            using (var handle = GetFileHandleWithWrite(path))
            {
                NativeMethods.GetFileTime(handle, ref cTime, ref aTime, ref wTime);
                var fileTime = creationTime.ToFileTimeUtc();
                if (!NativeMethods.SetFileTime(handle, ref fileTime, ref aTime, ref wTime))
                {
                    throw new Win32Exception();
                }
            }
        }

        public static void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            long cTime = 0;
            long aTime = 0;
            long wTime = 0;

            using (var handle = GetFileHandleWithWrite(path))
            {
                NativeMethods.GetFileTime(handle, ref cTime, ref aTime, ref wTime);

                var fileTime = lastAccessTime.ToFileTimeUtc();
                if (!NativeMethods.SetFileTime(handle, ref cTime, ref fileTime, ref wTime))
                {
                    throw new Win32Exception();
                }
            }
        }

        public static void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            long cTime = 0;
            long aTime = 0;
            long wTime = 0;

            using (var handle = GetFileHandleWithWrite(path))
            {
                NativeMethods.GetFileTime(handle, ref cTime, ref aTime, ref wTime);

                var fileTime = lastWriteTime.ToFileTimeUtc();
                if (!NativeMethods.SetFileTime(handle, ref cTime, ref aTime, ref fileTime))
                {
                    throw new Win32Exception();
                }
            }
        }

        public static DateTime GetLastWriteTime(string path)
        {
            long cTime = 0;
            long aTime = 0;
            long wTime = 0;

            using (var handle = GetFileHandleWithWrite(path))
            {
                NativeMethods.GetFileTime(handle, ref cTime, ref aTime, ref wTime);

                return DateTime.FromFileTimeUtc(wTime);
            }
        }

    }

    public static class LongDirectory
    {
        private const int MAX_PATH = 260;

        public static void CreateDirectory(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            if (path.Length < MAX_PATH)
            {
                System.IO.Directory.CreateDirectory(path);
            }
            else
            {
                var paths = GetAllPathsFromPath(GetWin32LongPath(path));
                foreach (var item in paths)
                {
                    if (!LongExists(item))
                    {
                        var ok = NativeMethods.CreateDirectory(item, IntPtr.Zero);
                        if (!ok)
                        {
                            ThrowWin32Exception();
                        }
                    }
                }
            }
        }

        public static void Delete(string path)
        {
            Delete(path, false);
        }

        public static void Delete(string path, bool recursive)
        {
            if (path.Length < MAX_PATH && !recursive)
            {
                System.IO.Directory.Delete(path, false);
            }
            else
            {
                if (!recursive)
                {
                    bool ok = NativeMethods.RemoveDirectory(GetWin32LongPath(path));
                    if (!ok)
                    {
                        ThrowWin32Exception();
                    }
                }
                else
                {
                    DeleteDirectories(new string[] { GetWin32LongPath(path) });
                }
            }
        }

        private static void DeleteDirectories(string[] directories)
        {
            foreach (string directory in directories)
            {
                string[] files = LongDirectory.GetFiles(directory, null, System.IO.SearchOption.TopDirectoryOnly);
                foreach (string file in files)
                {
                    LongFile.Delete(file);
                }

                directories = LongDirectory.GetDirectories(directory, null, System.IO.SearchOption.TopDirectoryOnly);
                DeleteDirectories(directories);
                bool ok = NativeMethods.RemoveDirectory(GetWin32LongPath(directory));
                if (!ok)
                {
                    ThrowWin32Exception();
                }
            }
        }

        public static bool Exists(string path)
        {
            if (path.Length < MAX_PATH)
            {
                return System.IO.Directory.Exists(path);
            }

            return LongExists(GetWin32LongPath(path));
        }

        private static bool LongExists(string path)
        {
            var attr = NativeMethods.GetFileAttributesW(path);
            return (attr != NativeMethods.INVALID_FILE_ATTRIBUTES && ((attr & NativeMethods.FILE_ATTRIBUTE_DIRECTORY) == NativeMethods.FILE_ATTRIBUTE_DIRECTORY));
        }

        public static string[] GetDirectories(string path)
        {
            return GetDirectories(path, null, SearchOption.TopDirectoryOnly);
        }

        public static string[] GetDirectories(string path, string searchPattern)
        {
            return GetDirectories(path, searchPattern, SearchOption.TopDirectoryOnly);
        }

        public static string[] GetDirectories(string path, string searchPattern, System.IO.SearchOption searchOption)
        {
            searchPattern = searchPattern ?? "*";
            var dirs = new List<string>();
            InternalGetDirectories(path, searchPattern, searchOption, ref dirs);
            return dirs.ToArray();
        }

        private static void InternalGetDirectories(string path, string searchPattern, System.IO.SearchOption searchOption, ref List<string> dirs)
        {
            NativeMethods.WIN32_FIND_DATA findData;
            IntPtr findHandle = NativeMethods.FindFirstFile(System.IO.Path.Combine(GetWin32LongPath(path), searchPattern), out findData);

            try
            {
                if (findHandle != new IntPtr(-1))
                {

                    do
                    {
                        if ((findData.dwFileAttributes & System.IO.FileAttributes.Directory) != 0)
                        {
                            if (findData.cFileName != "." && findData.cFileName != "..")
                            {
                                string subdirectory = System.IO.Path.Combine(path, findData.cFileName);
                                dirs.Add(GetCleanPath(subdirectory));
                                if (searchOption == SearchOption.AllDirectories)
                                {
                                    InternalGetDirectories(subdirectory, searchPattern, searchOption, ref dirs);
                                }
                            }
                        }
                    } while (NativeMethods.FindNextFile(findHandle, out findData));
                    NativeMethods.FindClose(findHandle);
                }
                else
                {
                    ThrowWin32Exception();
                }
            }
            catch (Exception)
            {
                NativeMethods.FindClose(findHandle);
                throw;
            }
        }

        public static string[] GetFiles(string path)
        {
            return GetFiles(path, null, SearchOption.TopDirectoryOnly);
        }

        public static string[] GetFiles(string path, string searchPattern)
        {
            return GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly);
        }

        public static string[] GetFiles(string path, string searchPattern, System.IO.SearchOption searchOption)
        {
            searchPattern = searchPattern ?? "*";

            var files = new List<string>();
            var dirs = new List<string> { path };

            if (searchOption == SearchOption.AllDirectories)
            {
                // Add all the subpaths
                dirs.AddRange(LongDirectory.GetDirectories(path, null, SearchOption.AllDirectories));
            }

            foreach (var dir in dirs)
            {
                NativeMethods.WIN32_FIND_DATA findData;
                IntPtr findHandle = NativeMethods.FindFirstFile(System.IO.Path.Combine(GetWin32LongPath(dir), searchPattern), out findData);

                try
                {
                    if (findHandle != new IntPtr(-1))
                    {

                        do
                        {
                            if ((findData.dwFileAttributes & System.IO.FileAttributes.Directory) == 0)
                            {
                                string filename = System.IO.Path.Combine(dir, findData.cFileName);
                                files.Add(GetCleanPath(filename));
                            }
                        } while (NativeMethods.FindNextFile(findHandle, out findData));
                        NativeMethods.FindClose(findHandle);
                    }
                }
                catch (Exception)
                {
                    NativeMethods.FindClose(findHandle);
                    throw;
                }
            }

            return files.ToArray();
        }

        public static void Move(string sourceDirName, string destDirName)
        {
            if (sourceDirName.Length < MAX_PATH || destDirName.Length < MAX_PATH)
            {
                System.IO.Directory.Move(sourceDirName, destDirName);
            }
            else
            {
                var ok = NativeMethods.MoveFileW(GetWin32LongPath(sourceDirName), GetWin32LongPath(destDirName));
                if (!ok)
                {
                    ThrowWin32Exception();
                }
            }
        }

        #region Helper methods

        [DebuggerStepThrough]
        public static void ThrowWin32Exception()
        {
            int code = Marshal.GetLastWin32Error();
            if (code != 0)
            {
                throw new System.ComponentModel.Win32Exception(code);
            }
        }

        public static string GetWin32LongPath(string path)
        {

            if (path.StartsWith(@"\\?\"))
            {
                return path;
            }

            var newpath = path;
            if (newpath.StartsWith("\\"))
            {
                newpath = @"\\?\UNC\" + newpath.Substring(2);
            }
            else if (newpath.Contains(":"))
            {
                newpath = @"\\?\" + newpath;
            }
            else
            {
                var currdir = Environment.CurrentDirectory;
                newpath = Combine(currdir, newpath);
                while (newpath.Contains("\\.\\"))
                {
                    newpath = newpath.Replace("\\.\\", "\\");
                }

                newpath = @"\\?\" + newpath;
            }

            return newpath.TrimEnd('.');
        }

        private static string GetCleanPath(string path)
        {
            if (path.StartsWith(@"\\?\UNC\"))
            {
                return @"\\" + path.Substring(8);
            }

            if (path.StartsWith(@"\\?\"))
            {
                return path.Substring(4);
            }

            return path;
        }

        private static List<string> GetAllPathsFromPath(string path)
        {
            bool unc = false;
            var prefix = @"\\?\";
            if (path.StartsWith(prefix + @"UNC\"))
            {
                prefix += @"UNC\";
                unc = true;
            }

            var split = path.Split('\\');
            int i = unc ? 6 : 4;
            var list = new List<string>();

            var sb = new StringBuilder();

            for (int a = 0; a < i; a++)
            {
                if (a > 0)
                {
                    sb.Append('\\');
                }

                sb.Append(split[a]);
            }

            var txt = sb.ToString();

            for (; i < split.Length; i++)
            {
                txt = Combine(txt, split[i]);
                list.Add(txt);
            }

            return list;
        }

        private static string Combine(string path1, string path2)
        {
            return path1.TrimEnd('\\') + "\\" + path2.TrimStart('\\').TrimEnd('.');
        }

        #endregion
    }
}