﻿namespace ZetaShortPaths;

[PublicAPI]
public static class DirectoryInfoExtensions
{
	[PublicAPI]
	public static FileSystemInfo[] GetFileSystemInfos(this DirectoryInfo? i, SearchOption searchOption)
	{
		return i?.GetFileSystemInfos(@"*", searchOption) ?? [];
	}

	[PublicAPI]
	public static void MoveTo(this DirectoryInfo? i, DirectoryInfo? to, bool ovewriteExisting = true)
	{
		doCopyDir(i?.FullName, to?.FullName, ovewriteExisting);
	}

	[PublicAPI]
	public static bool IsEmpty(this DirectoryInfo? i)
	{
		return ZspIOHelper.IsDirectoryEmpty(i);
	}

	private static void doCopyDir(string? sourceFolder, string? destFolder, bool ovewriteExisting)
	{
		// https://stackoverflow.com/a/3911658/107625

		if (string.IsNullOrWhiteSpace(sourceFolder) || string.IsNullOrWhiteSpace(destFolder))
			return;

		if (!Directory.Exists(sourceFolder))
			return;

		if (!Directory.Exists(destFolder))
			Directory.CreateDirectory(destFolder);

		// Get Files & Copy
		var files = Directory.GetFiles(sourceFolder);
		foreach (var file in files)
		{
			var name = Path.GetFileName(file);

			// ADD Unique File Name Check to Below.
			var dest = ZspPathHelper.Combine(destFolder, name);
			if (dest != null) File.Copy(file, dest, ovewriteExisting);
		}

		// Get dirs recursively and copy files
		var folders = Directory.GetDirectories(sourceFolder);
		foreach (var folder in folders)
		{
			var name = Path.GetFileName(folder);
			var dest = ZspPathHelper.Combine(destFolder, name);
			if (dest != null) doCopyDir(folder, dest, ovewriteExisting);
		}
	}
}