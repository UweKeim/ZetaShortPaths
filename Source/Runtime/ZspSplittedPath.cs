namespace ZetaShortPaths;

[PublicAPI]
public sealed class ZspSplittedPath
{
    [PublicAPI]
    public ZspSplittedPath(
        string? path)
    {
        Info = new ZspFileOrDirectoryInfo(path);
    }

    public ZspSplittedPath(
        ZspFileOrDirectoryInfo path)
    {
        Info = new ZspFileOrDirectoryInfo(path);
    }

    [PublicAPI] public string? FullPath => Info.FullName;

    [PublicAPI] public ZspFileOrDirectoryInfo Info { get; }

    [PublicAPI] public string? Drive => ZspPathHelper.GetDrive(Info.FullName);

    [PublicAPI] public string? Share => ZspPathHelper.GetShare(Info.FullName);

    [PublicAPI] public string? DriveOrShare => ZspPathHelper.GetDriveOrShare(Info.FullName);

    [PublicAPI] public string? Directory => ZspPathHelper.GetDirectory(Info.FullName);

    [PublicAPI] public string? NameWithoutExtension => ZspPathHelper.GetNameWithoutExtension(Info.FullName);

    [PublicAPI] public string? NameWithExtension => ZspPathHelper.GetNameWithExtension(Info.FullName);

    [PublicAPI] public string? Extension => ZspPathHelper.GetExtension(Info.FullName);

    [PublicAPI] public string? DriveOrShareAndDirectory => ZspPathHelper.Combine(DriveOrShare, Directory);

    [PublicAPI]
    public string? DriveOrShareAndDirectoryAndNameWithoutExtension =>
        ZspPathHelper.Combine(ZspPathHelper.Combine(DriveOrShare, Directory), NameWithoutExtension);

    [PublicAPI] public string? DirectoryAndNameWithExtension => ZspPathHelper.Combine(Directory, NameWithExtension);
}