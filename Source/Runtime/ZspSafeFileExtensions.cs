namespace ZetaShortPaths;

public static class ZspSafeFileExtensions
{
    [PublicAPI]
    public static DirectoryInfo SafeDelete(this DirectoryInfo folderPath)
    {
        ZspSafeFileOperations.SafeDeleteDirectory(folderPath);
        return folderPath;
    }

    [PublicAPI]
    public static DirectoryInfo SafeDeleteContents(this DirectoryInfo folderPath)
    {
        ZspSafeFileOperations.SafeDeleteDirectoryContents(folderPath);
        return folderPath;
    }

    [PublicAPI]
    public static FileInfo SafeDelete(this FileInfo filePath)
    {
        ZspSafeFileOperations.SafeDeleteFile(filePath);
        return filePath;
    }

    [PublicAPI]
    public static bool SafeExists(this DirectoryInfo folderPath)
    {
        return ZspSafeFileOperations.SafeDirectoryExists(folderPath);
    }

    [PublicAPI]
    public static bool SafeExists(this FileInfo filePath)
    {
        return ZspSafeFileOperations.SafeFileExists(filePath);
    }

    [PublicAPI]
    public static DirectoryInfo SafeCheckCreate(this DirectoryInfo folderPath)
    {
        ZspSafeFileOperations.SafeCheckCreateDirectory(folderPath);
        return folderPath;
    }

    [PublicAPI]
    public static FileInfo SafeMove(this FileInfo sourcePath, string dstFilePath)
    {
        ZspSafeFileOperations.SafeMoveFile(sourcePath, dstFilePath);
        return sourcePath;
    }

    [PublicAPI]
    public static FileInfo SafeMove(this FileInfo sourcePath, FileInfo dstFilePath)
    {
        ZspSafeFileOperations.SafeMoveFile(sourcePath, dstFilePath);
        return sourcePath;
    }

    [PublicAPI]
    public static FileInfo SafeCopy(this FileInfo sourcePath, string dstFilePath, bool overwrite = true)
    {
        ZspSafeFileOperations.SafeCopyFile(sourcePath, dstFilePath, overwrite);
        return sourcePath;
    }

    [PublicAPI]
    public static FileInfo SafeCopy(this FileInfo sourcePath, FileInfo dstFilePath, bool overwrite = true)
    {
        ZspSafeFileOperations.SafeCopyFile(sourcePath, dstFilePath, overwrite);
        return sourcePath;
    }
}