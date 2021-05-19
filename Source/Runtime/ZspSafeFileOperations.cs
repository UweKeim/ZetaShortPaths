namespace ZetaShortPaths
{
    using JetBrains.Annotations;
    using System;
    using System.IO;

    /// <summary>
    /// The goal of this class is to provide more error-tolerant functions
    /// for basic file operations. Especially when you have a larger project
    /// and ask yourself "why is this file being deleted?" this class helps
    /// by logging each operation and doing it in a more error-tolerant way,
    /// too. So do all file operations through this class and you get a more
    /// determinable system, hopefully.
    /// </summary>
    /// <remarks>
    /// 2007-03-08: Initially created class.
    /// </remarks>
    public static class ZspSafeFileOperations
    {
        public static void SafeDeleteFile(
            FileInfo filePath)
        {
            if (filePath != null)
            {
                SafeDeleteFile(filePath.FullName);
            }
        }

        [UsedImplicitly]
        public static void SafeDeleteFile(
            string filePath)
        {
#if WANT_TRACE
            Trace.TraceInformation(@"About to safe-delete file '{0}'.", filePath);
#endif

            if (!string.IsNullOrEmpty(filePath) &&
                SafeFileExists(filePath))
            {
                try
                {
                    var attributes = File.GetAttributes(filePath);

                    // Remove read-only attributes.
                    if ((attributes & FileAttributes.ReadOnly) != 0)
                    {
                        File.SetAttributes(
                            filePath,
                            attributes & ~FileAttributes.ReadOnly);
                    }

                    File.Delete(filePath);
                }
                catch (UnauthorizedAccessException x)
                {
                    var newFilePath =
                        $@"{filePath}.{Guid.NewGuid():N}.deleted";

#if WANT_TRACE
                    Trace.TraceWarning(@"Caught UnauthorizedAccessException while deleting file '{0}'. " +
                                       @"Renaming now to '{1}'. {2}", filePath, newFilePath, x.Message);
#endif

                    try
                    {
                        if (File.Exists(newFilePath)) File.Delete(newFilePath);

                        File.Move(filePath, newFilePath);
                    }
                    catch (Exception x2)
                    {
#if WANT_TRACE
                        Trace.TraceWarning(@"Caught IOException while renaming upon failed deleting file '{0}'. " +
                                           @"Renaming now to '{1}'. {2}", filePath, newFilePath, x2.Message);
#else 
                        // Ignore.
#endif
                    }
                }
                catch (Exception x)
                {
                    var newFilePath =
                        $@"{filePath}.{Guid.NewGuid():N}.deleted";

#if WANT_TRACE
                    Trace.TraceWarning(@"Caught IOException while deleting file '{0}'. " +
                                       @"Renaming now to '{1}'. {2}", filePath, newFilePath, x.Message);
#else
                    // Ignore.
#endif

                    try
                    {
                        if (File.Exists(newFilePath)) File.Delete(newFilePath);
                        File.Move(filePath, newFilePath);
                    }
                    catch (Exception x2)
                    {
#if WANT_TRACE
                        Trace.TraceWarning(@"Caught IOException while renaming upon failed deleting file '{0}'. " +
                                           @"Renaming now to '{1}'. {2}", filePath, newFilePath, x2.Message);
#else 
                        // Ignore.
#endif
                    }
                }
            }
            else
            {
#if WANT_TRACE
                Trace.TraceInformation(@"Not safe-deleting file '{0}', " +
                                       @"because the file does not exist.", filePath);
#endif
            }
        }

        public static bool SafeFileExists(
            FileInfo filePath)
        {
            return filePath != null && SafeFileExists(filePath.FullName);
        }

        [UsedImplicitly]
        public static bool SafeFileExists(
            string filePath)
        {
            return !string.IsNullOrEmpty(filePath) && File.Exists(filePath);
        }

        /// <summary>
        /// Deep-deletes the contents, as well as the folder itself.
        /// </summary>
        public static void SafeDeleteDirectory(
            DirectoryInfo folderPath)
        {
            if (folderPath != null)
            {
                SafeDeleteDirectory(folderPath.FullName);
            }
        }

        /// <summary>
        /// Deep-deletes the contents, as well as the folder itself.
        /// </summary>
        [UsedImplicitly]
        public static void SafeDeleteDirectory(
            string folderPath)
        {
#if WANT_TRACE
            Trace.TraceInformation(@"About to safe-delete directory '{0}'.", folderPath);
#endif

            if (!string.IsNullOrEmpty(folderPath) && SafeDirectoryExists(folderPath))
            {
                try
                {
                    Directory.Delete(folderPath, true);
                }
                catch (Exception x)
                {
                    var newFilePath = $@"{folderPath}.{Guid.NewGuid():B}.deleted";

#if WANT_TRACE
                    Trace.TraceWarning(@"Caught IOException while deleting directory '{0}'. " +
                                       @"Renaming now to '{1}'. {2}", folderPath, newFilePath, x.Message);
#endif

                    try
                    {
                        Directory.Move(folderPath, newFilePath);
                    }
                    catch (Exception x2)
                    {
#if WANT_TRACE
                        Trace.TraceWarning(@"Caught IOException while renaming upon failed deleting directory '{0}'. " +
                                           @"Renaming now to '{1}'. {2}", folderPath, newFilePath, x2.Message);
#endif
                    }
                }
            }
            else
            {
#if WANT_TRACE
                Trace.TraceInformation(@"Not safe-deleting directory '{0}', " +
                                       @"because the directory does not exist.", folderPath);
#endif
            }
        }

        public static bool SafeDirectoryExists(
            DirectoryInfo folderPath)
        {
            return folderPath != null && SafeDirectoryExists(folderPath.FullName);
        }

        [UsedImplicitly]
        public static bool SafeDirectoryExists(
            string folderPath)
        {
            return !string.IsNullOrEmpty(folderPath) && Directory.Exists(folderPath);
        }

        public static void SafeMoveFile(
            FileInfo sourcePath,
            string dstFilePath)
        {
            SafeMoveFile(
                sourcePath?.FullName,
                dstFilePath);
        }

        [UsedImplicitly]
        public static void SafeMoveFile(
            string sourcePath,
            FileInfo dstFilePath)
        {
            SafeMoveFile(
                sourcePath,
                dstFilePath?.FullName);
        }

        public static void SafeMoveFile(
            FileInfo sourcePath,
            FileInfo dstFilePath)
        {
            SafeMoveFile(
                sourcePath?.FullName,
                dstFilePath?.FullName);
        }

        [UsedImplicitly]
        public static void SafeMoveFile(
            string sourcePath,
            string dstFilePath)
        {
#if WANT_TRACE
            Trace.TraceInformation(@"About to safe-move file from '{0}' to '{1}'.", sourcePath, dstFilePath);
#endif

            if (sourcePath == null || dstFilePath == null)
            {
#if WANT_TRACE
                Trace.TraceInformation(
                    string.Format(
                        @"Source file path or destination file path does not exist. " +
                        @"Not moving."
                    ));
#endif
            }
            else
            {
                if (SafeFileExists(sourcePath))
                {
                    SafeDeleteFile(dstFilePath);

                    var d = ZspPathHelper.GetDirectoryPathNameFromFilePath(dstFilePath);

                    if (!Directory.Exists(d))
                    {
#if WANT_TRACE
                        Trace.TraceInformation(@"Creating non-existing folder '{0}'.", d);
#endif
                        Directory.CreateDirectory(d);
                    }

                    if (File.Exists(dstFilePath)) File.Delete(dstFilePath);
                    File.Move(sourcePath, dstFilePath);
                }
                else
                {
#if WANT_TRACE
                    Trace.TraceInformation(@"Source file path to move does not exist: '{0}'.", sourcePath);
#endif
                }
            }
        }

        public static void SafeCopyFile(
            FileInfo sourcePath,
            string dstFilePath,
            bool overwrite = true)
        {
            SafeCopyFile(sourcePath?.FullName, dstFilePath, overwrite);
        }

        [UsedImplicitly]
        public static void SafeCopyFile(
            string sourcePath,
            FileInfo dstFilePath,
            bool overwrite = true)
        {
            SafeCopyFile(sourcePath, dstFilePath?.FullName, overwrite);
        }

        public static void SafeCopyFile(
            FileInfo sourcePath,
            FileInfo dstFilePath,
            bool overwrite = true)
        {
            SafeCopyFile(
                sourcePath?.FullName,
                dstFilePath?.FullName,
                overwrite);
        }

        [UsedImplicitly]
        public static void SafeCopyFile(
            string sourcePath,
            string dstFilePath,
            bool overwrite = true)
        {
#if WANT_TRACE
            Trace.TraceInformation(@"About to safe-copy file from '{0}' to '{1}' " +
                                   @"with overwrite = '{2}'.", sourcePath, dstFilePath, overwrite);
#endif

            if (sourcePath == null || dstFilePath == null)
            {
#if WANT_TRACE
                Trace.TraceInformation(
                    string.Format(
                        @"Source file path or destination file path does not exist. " +
                        @"Not copying."
                    ));
#endif
            }
            else
            {
                if (string.Compare(sourcePath, dstFilePath, StringComparison.OrdinalIgnoreCase) == 0)
                {
#if WANT_TRACE
                    Trace.TraceInformation(@"Source path and destination path are the same: " +
                                           @"'{0}' is '{1}'. Not copying.", sourcePath, dstFilePath);
#endif
                }
                else
                {
                    if (SafeFileExists(sourcePath))
                    {
                        if (overwrite)
                        {
                            SafeDeleteFile(dstFilePath);
                        }

                        var d = ZspPathHelper.GetDirectoryPathNameFromFilePath(dstFilePath);

                        if (!Directory.Exists(d))
                        {
#if WANT_TRACE
                            Trace.TraceInformation(@"Creating non-existing folder '{0}'.", d);
#endif
                            Directory.CreateDirectory(d);
                        }

                        File.Copy(sourcePath, dstFilePath, overwrite);
                    }
                    else
                    {
#if WANT_TRACE
                        Trace.TraceInformation(@"Source file path to copy does not exist: '{0}'.", sourcePath);
#endif
                    }
                }
            }
        }

        /// <summary>
        /// Deep-deletes the contents, but not the folder itself.
        /// </summary>
        [UsedImplicitly]
        public static void SafeDeleteDirectoryContents(
            string folderPath)
        {
            var info = new DirectoryInfo(folderPath);
            SafeDeleteDirectoryContents(info);
        }

        /// <summary>
        /// Deep-deletes the contents, but not the folder itself.
        /// </summary>
        public static void SafeDeleteDirectoryContents(
            DirectoryInfo folderPath)
        {
            if (folderPath is {Exists: true})
            {
                foreach (var filePath in folderPath.GetFiles())
                {
                    SafeDeleteFile(filePath);
                }

                foreach (var childFolderPath in
                    folderPath.GetDirectories())
                {
                    SafeDeleteDirectoryContents(childFolderPath);

                    // If empty now, remove.
                    // Only for childs, not for the root.
                    if (childFolderPath.GetFiles().Length <= 0 &&
                        childFolderPath.GetDirectories().Length <= 0)
                    {
                        childFolderPath.Delete(true);
                    }
                }
            }
        }

        public static void SafeCheckCreateDirectory(
            DirectoryInfo folderPath)
        {
            SafeCheckCreateDirectory(folderPath?.FullName);
        }

        [UsedImplicitly]
        public static void SafeCheckCreateDirectory(
            string folderPath)
        {
#if WANT_TRACE
            Trace.TraceInformation(@"About to safe check-create folder '{0}'.", folderPath);
#endif

            if (!string.IsNullOrEmpty(folderPath) && !SafeDirectoryExists(folderPath))
            {
                try
                {
                    Directory.CreateDirectory(folderPath);
                }
                catch (UnauthorizedAccessException x)
                {
#if WANT_TRACE
                    Trace.TraceWarning(
                        @"Caught UnauthorizedAccessException while safe check-creating folder '{0}'. {1}", folderPath,
                        x.Message);
#endif
                }
                catch (Exception x)
                {
#if WANT_TRACE
                    Trace.TraceWarning(@"Caught IOException while safe check-creating folder '{0}'. {1}", folderPath,
                        x.Message);
#endif
                }
            }
            else
            {
#if WANT_TRACE
                Trace.TraceInformation(
                    @"Not safe check-creating folder '{0}', because the folder is null or already exists.", folderPath);
#endif
            }
        }
    }
}