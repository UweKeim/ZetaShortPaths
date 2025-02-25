namespace ZetaShortPaths;

[PublicAPI]
public sealed class ZspSplittedPath
{
	public ZspSplittedPath(string? path)
	{
		Info = new(path);
	}

	public ZspSplittedPath(ZspFileOrDirectoryInfo? path)
	{
		Info = new(path);
	}

	public string? FullPath => Info?.FullName;

	public ZspFileOrDirectoryInfo? Info { get; }

	public string? Drive => ZspPathHelper.GetDrive(Info?.FullName);

	public string? Share => ZspPathHelper.GetShare(Info?.FullName);

	public string? DriveOrShare => ZspPathHelper.GetDriveOrShare(Info?.FullName);

	public string? Directory => ZspPathHelper.GetDirectory(Info?.FullName);

	public string? NameWithoutExtension => ZspPathHelper.GetNameWithoutExtension(Info?.FullName);

	public string? NameWithExtension => ZspPathHelper.GetNameWithExtension(Info?.FullName);

	public string? Extension => ZspPathHelper.GetExtension(Info?.FullName);

	public string? DriveOrShareAndDirectory => ZspPathHelper.Combine(DriveOrShare, Directory);

	public string? DriveOrShareAndDirectoryAndNameWithoutExtension =>
		ZspPathHelper.Combine(ZspPathHelper.Combine(DriveOrShare, Directory), NameWithoutExtension);

	public string? DirectoryAndNameWithExtension => ZspPathHelper.Combine(Directory, NameWithExtension);
}