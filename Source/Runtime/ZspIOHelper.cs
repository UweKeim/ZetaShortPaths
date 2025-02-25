namespace ZetaShortPaths;

/// <summary>
/// 
/// </summary>
[PublicAPI]
public static class ZspIOHelper
{
	public static DirectoryInfo GetTempDirectory()
	{
		return new(ZspPathHelper.GetTempDirectoryPath());
	}

	public static FileInfo GetTempFile()
	{
		return new(ZspPathHelper.GetTempFilePath());
	}

	public static bool IsDirectoryEmpty(
		DirectoryInfo? directoryPath)
	{
		return directoryPath == null || IsDirectoryEmpty(directoryPath.FullName);
	}

	public static bool IsDirectoryEmpty(
		string? directoryPath)
	{
		return
			string.IsNullOrEmpty(directoryPath) ||
			!Directory.Exists(directoryPath) ||
			Directory.GetFiles(directoryPath).Length <= 0 &&
			Directory.GetDirectories(directoryPath).Length <= 0;
	}

	/// <summary>
	/// Returns the same MD5 hash as the PHP function call http://php.net/manual/de/function.hash-file.php 
	/// with 'md5' as the first parameter.
	/// </summary>
	public static string? CalculateMD5Hash(
		string? path)
	{
		if (string.IsNullOrEmpty(path)) return null;

		// https://stackoverflow.com/a/10520086/107625

		using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
		using var md5 = MD5.Create();

		var hash = md5.ComputeHash(fs);
		return BitConverter.ToString(hash).Replace(@"-", string.Empty).ToLowerInvariant();
	}

	public static void CopyFileExact(
		string? sourceFilePath,
		string? destinationFilePath,
		bool overwriteExisting)
	{
		if (string.IsNullOrEmpty(sourceFilePath)) throw new ArgumentNullException(nameof(sourceFilePath));
		if (string.IsNullOrEmpty(destinationFilePath)) throw new ArgumentNullException(nameof(destinationFilePath));

		File.Copy(sourceFilePath, destinationFilePath, overwriteExisting);
		CloneDates(sourceFilePath, destinationFilePath);
	}

	private static void CloneDates(string? sourceFilePath, string? destinationFilePath)
	{
		if (string.IsNullOrEmpty(sourceFilePath)) throw new ArgumentNullException(nameof(sourceFilePath));
		if (string.IsNullOrEmpty(destinationFilePath)) throw new ArgumentNullException(nameof(destinationFilePath));

		var s = new FileInfo(sourceFilePath);
		var d = new FileInfo(destinationFilePath);

		var sc = s.CreationTime;
		var sa = s.LastAccessTime;
		var sw = s.LastWriteTime;

		if (sc > DateTime.MinValue) d.CreationTime = sc;
		if (sa > DateTime.MinValue) d.LastAccessTime = sa;
		if (sw > DateTime.MinValue) d.LastWriteTime = sw;
	}

	public static bool DriveExists(char driveLetter)
	{
		return DriveInfo.GetDrives().Any(di =>
			di.Name.StartsWith($@"{driveLetter}:", StringComparison.InvariantCultureIgnoreCase));
	}

	public static void Touch(string? filePath)
	{
		if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));

		var now = DateTime.Now;

		File.SetCreationTime(filePath, now);
		File.SetLastAccessTime(filePath, now);
		File.SetLastWriteTime(filePath, now);
	}

	public static void DeleteDirectoryContents(string? folderPath, bool recursive)
	{
		if (!Directory.Exists(folderPath)) return;

		var files = Directory.GetFiles(folderPath);
		var dirs = Directory.GetDirectories(folderPath);

		foreach (var file in files)
		{
			File.Delete(file);
		}

		foreach (var dir in dirs)
		{
			if (recursive || IsDirectoryEmpty(dir)) Directory.Delete(dir, recursive);
		}
	}

	public static long GetFileLength(string? filePath)
	{
		if (string.IsNullOrEmpty(filePath)) return 0;
		else if (!File.Exists(filePath)) return 0;
		else return new FileInfo(filePath).Length;
	}

	public static FileInfo[] GetFiles(string? directoryPath, string? pattern = @"*")
	{
		return GetFiles(directoryPath, pattern, SearchOption.TopDirectoryOnly);
	}

	public static FileInfo[] GetFiles(string? directoryPath, SearchOption searchOption)
	{
		return GetFiles(directoryPath, @"*", searchOption);
	}

	public static FileInfo[] GetFiles(string? directoryPath, string? pattern, SearchOption searchOption)
	{
		if (directoryPath == null) throw new ArgumentNullException(nameof(directoryPath));
		if (pattern == null) throw new ArgumentNullException(nameof(pattern));
		if (string.IsNullOrEmpty(directoryPath) || !Directory.Exists(directoryPath)) return [];

		return new DirectoryInfo(directoryPath).GetFiles(pattern, searchOption);
	}

	public static DirectoryInfo[] GetDirectories(string? directoryPath, string? pattern = @"*")
	{
		return GetDirectories(directoryPath, pattern, SearchOption.TopDirectoryOnly);
	}

	public static DirectoryInfo[] GetDirectories(string? directoryPath, SearchOption searchOption)
	{
		return GetDirectories(directoryPath, @"*", searchOption);
	}

	public static DirectoryInfo[] GetDirectories(string? directoryPath, string? pattern, SearchOption searchOption)
	{
		if (directoryPath == null) throw new ArgumentNullException(nameof(directoryPath));
		if (pattern == null) throw new ArgumentNullException(nameof(pattern));
		if (string.IsNullOrEmpty(directoryPath) || !Directory.Exists(directoryPath))
			return [];

		return new DirectoryInfo(directoryPath).GetDirectories(pattern, searchOption);
	}

	public static ZspFileDateInfos GetFileDateInfos(FileInfo? filePath)
	{
		if (filePath == null) throw new ArgumentNullException(nameof(filePath));
		return GetFileDateInfos(filePath.FullName);
	}

	public static ZspFileDateInfos GetFileDateInfos(string? filePath)
	{
		if (filePath == null) throw new ArgumentNullException(nameof(filePath));

		var fi = new FileInfo(filePath);
		return new()
			{ CreationTime = fi.CreationTime, LastAccessTime = fi.LastAccessTime, LastWriteTime = fi.LastWriteTime };
	}

	public static void SetFileDateInfos(FileInfo? filePath, ZspFileDateInfos? infos)
	{
		if (filePath == null) throw new ArgumentNullException(nameof(filePath));
		if (infos == null) throw new ArgumentNullException(nameof(infos));
		SetFileDateInfos(filePath.FullName, infos);
	}

	public static void SetFileDateInfos(string? filePath, ZspFileDateInfos? infos)
	{
		if (filePath == null) throw new ArgumentNullException(nameof(filePath));
		if (infos == null) throw new ArgumentNullException(nameof(infos));

		// ReSharper disable once UnusedVariable
#pragma warning disable IDE0059
		var fi = new FileInfo(filePath)
#pragma warning restore IDE0059
		{
			CreationTime = infos.CreationTime,
			LastAccessTime = infos.LastAccessTime,
			LastWriteTime = infos.LastWriteTime
		};
	}

	/// <summary>
	/// Die maximale Zeichenlänge. Hier bewusst auf 247 gesetzt, siehe:
	/// https://web.archive.org/web/20140503163140/http://zetalongpaths.codeplex.com/discussions/230652
	/// </summary>
	public const int MAX_PATH = 247;

	/// <summary>
	/// The is current path longer, then supported by standard System.IO methods
	/// </summary>
	/// <param name="path">
	/// The path to check.
	/// </param>
	/// <returns>
	/// True, if path longer then MAX_PATH constant, or is UNC path, else - False
	/// </returns>
	public static bool IsPathLong(string? path)
	{
		return MustBeLongPath(path);
	}

	public static bool MustBeLongPath(string? path)
	{
		if (path == null || string.IsNullOrEmpty(path))
		{
			return false;
		}
		else if (path.StartsWith(@"\\?\"))
		{
			return true;
		}
		else if (path.Contains(@"~"))
		{
			// See https://github.com/UweKeim/ZetaLongPaths/issues/12
			// Example: "C:\\Users\\cliente\\Desktop\\DRIVES~2\\mdzip\\PASTAC~1\\SUBPAS~1\\PASTAC~1\\SUBPAS~1\\SUBDAS~1\\bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb.txt"
			return true;
		}
		else if (path.Length > MAX_PATH)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public static string? CheckAddLongPathPrefix(string? path)
	{
		if (path == null || string.IsNullOrEmpty(path) || path.StartsWith(@"\\?\"))
		{
			return path;
		}
		else if (path.Length > MAX_PATH ||
		         // See https://github.com/UweKeim/ZetaLongPaths/issues/12
		         // Example: "C:\\Users\\cliente\\Desktop\\DRIVES~2\\mdzip\\PASTAC~1\\SUBPAS~1\\PASTAC~1\\SUBPAS~1\\SUBDAS~1\\bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb.txt"
		         path.Contains(@"~"))
		{
			return ForceAddLongPathPrefix(path);
		}
		else
		{
			return path;
		}
	}

	public static string? ForceRemoveLongPathPrefix(string? path)
	{
		if (path == null || string.IsNullOrEmpty(path) || !path.StartsWith(@"\\?\"))
		{
			return path;
		}
		else if (path.StartsWith(@"\\?\UNC\", StringComparison.OrdinalIgnoreCase))
		{
			return @"\\" + path[@"\\?\UNC\".Length..];
		}
		else
		{
			return path[@"\\?\".Length..];
		}
	}

	public static string? ForceAddLongPathPrefix(string? path)
	{
		if (path == null || string.IsNullOrEmpty(path) || path.StartsWith(@"\\?\"))
		{
			return path;
		}
		else
		{
			// http://msdn.microsoft.com/en-us/library/aa365247.aspx

			if (path.StartsWith(@"\\"))
			{
				// UNC.
				return @"\\?\UNC\" + path[2..];
			}
			else
			{
				return @"\\?\" + path;
			}
		}
	}
}