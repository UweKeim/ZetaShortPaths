﻿namespace ZetaLongPaths;

[UsedImplicitly]
public static class ZlpFileOrDirectoryInfoExtensions
{
    [UsedImplicitly]
    public static bool SafeExists(this ZlpFileOrDirectoryInfo i)
    {
        if (i == null || i.IsEmpty) return false;
        else if (i.IsDirectory) return ZlpSafeFileOperations.SafeDirectoryExists(i.Directory);
        else if (i.IsFile) return ZlpSafeFileOperations.SafeFileExists(i.File);
        else return false;
    }
}