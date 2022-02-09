namespace ZetaShortPaths;

[PublicAPI]
public static class ZspFileOrDirectoryInfoExtensions
{
    [PublicAPI]
    public static bool SafeExists(this ZspFileOrDirectoryInfo i)
    {
        if (i == null || i.IsEmpty) return false;
        else if (i.IsDirectory) return ZspSafeFileOperations.SafeDirectoryExists(i.Directory);
        else if (i.IsFile) return ZspSafeFileOperations.SafeFileExists(i.File);
        else return false;
    }
}