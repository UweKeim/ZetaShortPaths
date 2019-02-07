namespace ZetaShortPaths
{
    using JetBrains.Annotations;
    using System.IO;

    public static class ZspSafeFileExtensions
    {
        [UsedImplicitly]
        public static DirectoryInfo SafeDelete(this DirectoryInfo folderPath)
        {
            ZspSafeFileOperations.SafeDeleteDirectory(folderPath);
            return folderPath;
        }

        [UsedImplicitly]
        public static DirectoryInfo SafeDeleteContents(this DirectoryInfo folderPath)
        {
            ZspSafeFileOperations.SafeDeleteDirectoryContents(folderPath);
            return folderPath;
        }

        [UsedImplicitly]
        public static FileInfo SafeDelete(this FileInfo filePath)
        {
            ZspSafeFileOperations.SafeDeleteFile(filePath);
            return filePath;
        }

        [UsedImplicitly]
        public static bool SafeExists(this DirectoryInfo folderPath)
        {
            return ZspSafeFileOperations.SafeDirectoryExists(folderPath);
        }

        [UsedImplicitly]
        public static bool SafeExists(this FileInfo filePath)
        {
            return ZspSafeFileOperations.SafeFileExists(filePath);
        }

        [UsedImplicitly]
        public static DirectoryInfo SafeCheckCreate(this DirectoryInfo folderPath)
        {
            ZspSafeFileOperations.SafeCheckCreateDirectory(folderPath);
            return folderPath;
        }

        [UsedImplicitly]
        public static FileInfo SafeMove(this FileInfo sourcePath, string dstFilePath)
        {
            ZspSafeFileOperations.SafeMoveFile(sourcePath, dstFilePath);
            return sourcePath;
        }

        [UsedImplicitly]
        public static FileInfo SafeMove(this FileInfo sourcePath, FileInfo dstFilePath)
        {
            ZspSafeFileOperations.SafeMoveFile(sourcePath, dstFilePath);
            return sourcePath;
        }

        [UsedImplicitly]
        public static FileInfo SafeCopy(this FileInfo sourcePath, string dstFilePath, bool overwrite = true)
        {
            ZspSafeFileOperations.SafeCopyFile(sourcePath, dstFilePath, overwrite);
            return sourcePath;
        }

        [UsedImplicitly]
        public static FileInfo SafeCopy(this FileInfo sourcePath, FileInfo dstFilePath, bool overwrite = true)
        {
            ZspSafeFileOperations.SafeCopyFile(sourcePath, dstFilePath, overwrite);
            return sourcePath;
        }
    }
}