namespace ZetaShortPaths;

[PublicAPI]
public static class ZspSafeFileExtensions
{
    public static DirectoryInfo? SafeDelete(this DirectoryInfo? folderPath)
    {
        ZspSafeFileOperations.SafeDeleteDirectory(folderPath);
        return folderPath;
    }

    public static DirectoryInfo? SafeDeleteContents(this DirectoryInfo? folderPath)
    {
        ZspSafeFileOperations.SafeDeleteDirectoryContents(folderPath);
        return folderPath;
    }

    public static FileInfo? SafeDelete(this FileInfo? filePath)
    {
        ZspSafeFileOperations.SafeDeleteFile(filePath);
        return filePath;
    }

    public static bool SafeExists(this DirectoryInfo? folderPath)
    {
        return ZspSafeFileOperations.SafeDirectoryExists(folderPath);
    }

    public static bool SafeExists(this FileInfo? filePath)
    {
        return ZspSafeFileOperations.SafeFileExists(filePath);
    }

    public static DirectoryInfo? SafeCheckCreate(this DirectoryInfo? folderPath)
    {
        ZspSafeFileOperations.SafeCheckCreateDirectory(folderPath);
        return folderPath;
    }

    public static FileInfo? SafeMove(this FileInfo? sourcePath, string? dstFilePath)
    {
        ZspSafeFileOperations.SafeMoveFile(sourcePath, dstFilePath);
        return sourcePath;
    }

    public static FileInfo? SafeMove(this FileInfo? sourcePath, FileInfo? dstFilePath)
    {
        ZspSafeFileOperations.SafeMoveFile(sourcePath, dstFilePath);
        return sourcePath;
    }

    public static FileInfo? SafeCopy(this FileInfo? sourcePath, string? dstFilePath, bool overwrite = true)
    {
        ZspSafeFileOperations.SafeCopyFile(sourcePath, dstFilePath, overwrite);
        return sourcePath;
    }

    public static FileInfo? SafeCopy(this FileInfo? sourcePath, FileInfo? dstFilePath, bool overwrite = true)
    {
        ZspSafeFileOperations.SafeCopyFile(sourcePath, dstFilePath, overwrite);
        return sourcePath;
    }
}