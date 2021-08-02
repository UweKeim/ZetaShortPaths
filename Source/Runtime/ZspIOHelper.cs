namespace ZetaShortPaths
{
    using JetBrains.Annotations;
    using System;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;

    public static class ZspIOHelper
    {
        [UsedImplicitly]
        public static DirectoryInfo GetTempDirectory()
        {
            return new(Path.GetTempPath());
        }

        [UsedImplicitly]
        public static bool IsDirectoryEmpty(
            DirectoryInfo directoryPath)
        {
            return directoryPath == null || IsDirectoryEmpty(directoryPath.FullName);
        }

        [UsedImplicitly]
        public static bool IsDirectoryEmpty(
            string directoryPath)
        {
            return
                string.IsNullOrEmpty(directoryPath) ||
                !Directory.Exists(directoryPath) ||
                Directory.GetFiles(directoryPath).Length <= 0 &&
                Directory.GetDirectories(directoryPath).Length <= 0;
        }

        /// <summary>
        /// Returns the same MD5 hash as the PHP function call http://php.net/manual/de/function.hash-file.php 
        /// with 'md5' as the first parameter.
        /// </summary>
        [UsedImplicitly]
        public static string CalculateMD5Hash(
            string path)
        {
            // https://stackoverflow.com/a/10520086/107625

            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            using var md5 = MD5.Create();

            var hash = md5.ComputeHash(fs);
            return BitConverter.ToString(hash).Replace(@"-", string.Empty).ToLowerInvariant();
        }

        [UsedImplicitly]
        public static void CopyFileExact(
            string sourceFilePath,
            string destinationFilePath,
            bool overwriteExisting)
        {
            File.Copy(sourceFilePath, destinationFilePath, overwriteExisting);
            CloneDates(sourceFilePath, destinationFilePath);
        }

        [UsedImplicitly]
        private static void CloneDates(string sourceFilePath, string destinationFilePath)
        {
            var s = new FileInfo(sourceFilePath);
            var d = new FileInfo(destinationFilePath);

            var sc = s.CreationTime;
            var sa = s.LastAccessTime;
            var sw = s.LastWriteTime;

            if (sc > DateTime.MinValue) d.CreationTime = sc;
            if (sa > DateTime.MinValue) d.LastAccessTime = sa;
            if (sw > DateTime.MinValue) d.LastWriteTime = sw;
        }

        [UsedImplicitly]
        public static bool DriveExists(char driveLetter)
        {
            return DriveInfo.GetDrives().Any(di =>
                di.Name.StartsWith($@"{driveLetter}:", StringComparison.InvariantCultureIgnoreCase));
        }

        [UsedImplicitly]
        public static void Touch(string filePath)
        {
            var now = DateTime.Now;

            File.SetCreationTime(filePath, now);
            File.SetLastAccessTime(filePath, now);
            File.SetLastWriteTime(filePath, now);
        }

        //[UsedImplicitly]
        //public static void MoveDirectory(
        //    string sourceFolderPath,
        //    string destinationFolderPath)
        //{
        //    if (!PInvokeHelper.MoveFile(sourceFolderPath, destinationFolderPath))
        //    {
        //        // http://msdn.microsoft.com/en-us/library/ms681382(VS.85).aspx.

        //        var lastWin32Error = Marshal.GetLastWin32Error();
        //        throw new Win32Exception(
        //            lastWin32Error,
        //            string.Format(
        //                Resources.ErrorMovingFolder,
        //                lastWin32Error,
        //                sourceFolderPath,
        //                destinationFolderPath,
        //                CheckAddDotEnd(new Win32Exception(lastWin32Error).Message)));
        //    }
        //}

        [UsedImplicitly]
        public static void DeleteDirectoryContents(string folderPath, bool recursive)
        {
            if (Directory.Exists(folderPath))
            {
                if (recursive)
                {
                    var files = Directory.GetFiles(folderPath);
                    var dirs = Directory.GetDirectories(folderPath);

                    foreach (var file in files)
                    {
                        File.Delete(file);
                    }

                    foreach (var dir in dirs)
                    {
                        Directory.Delete(dir, true);
                    }
                }
            }
        }

        [UsedImplicitly]
        public static long GetFileLength(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return 0;
            else if (!File.Exists(filePath)) return 0;
            else return new FileInfo(filePath).Length;
        }

        [UsedImplicitly]
        public static FileInfo[] GetFiles(string directoryPath, string pattern = @"*.*")
        {
            return GetFiles(directoryPath, pattern, SearchOption.TopDirectoryOnly);
        }

        [UsedImplicitly]
        public static FileInfo[] GetFiles(string directoryPath, SearchOption searchOption)
        {
            return GetFiles(directoryPath, @"*.*", searchOption);
        }

        [UsedImplicitly]
        public static FileInfo[] GetFiles(string directoryPath, string pattern, SearchOption searchOption)
        {
            if (directoryPath == null) throw new ArgumentNullException(nameof(directoryPath));
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            return new DirectoryInfo(directoryPath).GetFiles(pattern, searchOption);
        }

        [UsedImplicitly]
        public static DirectoryInfo[] GetDirectories(string directoryPath, string pattern = @"*")
        {
            return GetDirectories(directoryPath, pattern, SearchOption.TopDirectoryOnly);
        }

        [UsedImplicitly]
        public static DirectoryInfo[] GetDirectories(string directoryPath, SearchOption searchOption)
        {
            return GetDirectories(directoryPath, @"*", searchOption);
        }

        [UsedImplicitly]
        public static DirectoryInfo[] GetDirectories(string directoryPath, string pattern, SearchOption searchOption)
        {
            if (directoryPath == null) throw new ArgumentNullException(nameof(directoryPath));
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            return new DirectoryInfo(directoryPath).GetDirectories(pattern, searchOption);
        }
    }
}