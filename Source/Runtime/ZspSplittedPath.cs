namespace ZetaShortPaths;

[UsedImplicitly]
public sealed class ZspSplittedPath
{
    [UsedImplicitly]
    public ZspSplittedPath(
        string path)
    {
        Info = new ZspFileOrDirectoryInfo(path);
    }

    public ZspSplittedPath(
        ZspFileOrDirectoryInfo path)
    {
        Info = new ZspFileOrDirectoryInfo(path);
    }

    [UsedImplicitly] public string FullPath => Info.FullName;

    [UsedImplicitly] public ZspFileOrDirectoryInfo Info { get; }

    [UsedImplicitly] public string Drive => ZspPathHelper.GetDrive(Info.FullName);

    [UsedImplicitly] public string Share => ZspPathHelper.GetShare(Info.FullName);

    [UsedImplicitly] public string DriveOrShare => ZspPathHelper.GetDriveOrShare(Info.FullName);

    [UsedImplicitly] public string Directory => ZspPathHelper.GetDirectory(Info.FullName);

    [UsedImplicitly] public string NameWithoutExtension => ZspPathHelper.GetNameWithoutExtension(Info.FullName);

    [UsedImplicitly] public string NameWithExtension => ZspPathHelper.GetNameWithExtension(Info.FullName);

    [UsedImplicitly] public string Extension => ZspPathHelper.GetExtension(Info.FullName);

    [UsedImplicitly] public string DriveOrShareAndDirectory => ZspPathHelper.Combine(DriveOrShare, Directory);

    [UsedImplicitly]
    public string DriveOrShareAndDirectoryAndNameWithoutExtension =>
        ZspPathHelper.Combine(ZspPathHelper.Combine(DriveOrShare, Directory), NameWithoutExtension);

    [UsedImplicitly]
    public string DirectoryAndNameWithExtension => ZspPathHelper.Combine(Directory, NameWithExtension);
}