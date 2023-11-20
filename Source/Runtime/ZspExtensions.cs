namespace ZetaShortPaths;

using Properties;

/// <summary>
/// "Nice to have" extensions.
/// </summary>
[PublicAPI]
public static class ZspExtensions
{
	public static string? MakeRelativeTo(
		this DirectoryInfo? pathToMakeRelative,
		DirectoryInfo? pathToWhichToMakeRelativeTo)
	{
		return ZspPathHelper.GetRelativePath(pathToWhichToMakeRelativeTo?.FullName, pathToMakeRelative?.FullName);
	}

	public static string? NameWithoutExtension(
		this FileInfo? filePath)
	{
		return Path.GetFileNameWithoutExtension(filePath?.FullName);
	}

	public static void Touch(
		this FileInfo? filePath)
	{
		if (filePath == null) throw new ArgumentNullException(nameof(filePath));
		ZspIOHelper.Touch(filePath.FullName);
	}

	public static void CopyToExact(
		this FileInfo? filePath,
		FileInfo destinationFilePath,
		bool overwriteExisting)
	{
		if (filePath == null) throw new ArgumentNullException(nameof(filePath));
		ZspIOHelper.CopyFileExact(filePath.FullName, destinationFilePath.FullName, overwriteExisting);
	}

	public static void CopyToExact(
		this FileInfo? filePath,
		string destinationFilePath,
		bool overwriteExisting)
	{
		if (filePath == null) throw new ArgumentNullException(nameof(filePath));
		ZspIOHelper.CopyFileExact(filePath.FullName, destinationFilePath, overwriteExisting);
	}

	public static string? MakeRelativeTo(
		this DirectoryInfo? pathToMakeRelative,
		string? pathToWhichToMakeRelativeTo)
	{
		return ZspPathHelper.GetRelativePath(pathToWhichToMakeRelativeTo, pathToMakeRelative?.FullName);
	}

	public static string? MakeRelativeTo(
		this FileInfo? pathToMakeRelative,
		DirectoryInfo? pathToWhichToMakeRelativeTo)
	{
		return ZspPathHelper.GetRelativePath(pathToWhichToMakeRelativeTo?.FullName, pathToMakeRelative?.FullName);
	}

	public static string? MakeRelativeTo(
		this FileInfo? pathToMakeRelative,
		string? pathToWhichToMakeRelativeTo)
	{
		return ZspPathHelper.GetRelativePath(pathToWhichToMakeRelativeTo, pathToMakeRelative?.FullName);
	}

	public static string? MakeAbsoluteTo(
		this DirectoryInfo? pathToMakeAbsolute,
		DirectoryInfo? basePathToWhichToMakeAbsoluteTo)
	{
		return ZspPathHelper.GetAbsolutePath(pathToMakeAbsolute?.FullName, basePathToWhichToMakeAbsoluteTo?.FullName);
	}

	public static string? MakeAbsoluteTo(
		this string? pathToMakeAbsolute,
		DirectoryInfo basePathToWhichToMakeAbsoluteTo)
	{
		return ZspPathHelper.GetAbsolutePath(pathToMakeAbsolute, basePathToWhichToMakeAbsoluteTo.FullName);
	}

	public static string? MakeAbsoluteTo(
		this DirectoryInfo? pathToMakeAbsolute,
		string basePathToWhichToMakeAbsoluteTo)
	{
		return ZspPathHelper.GetAbsolutePath(pathToMakeAbsolute?.FullName, basePathToWhichToMakeAbsoluteTo);
	}

	public static string? MakeAbsoluteTo(
		this FileInfo? pathToMakeAbsolute,
		DirectoryInfo basePathToWhichToMakeAbsoluteTo)
	{
		return ZspPathHelper.GetAbsolutePath(pathToMakeAbsolute?.FullName, basePathToWhichToMakeAbsoluteTo.FullName);
	}

	public static string? MakeAbsoluteTo(
		this FileInfo? pathToMakeAbsolute,
		string basePathToWhichToMakeAbsoluteTo)
	{
		return ZspPathHelper.GetAbsolutePath(pathToMakeAbsolute?.FullName, basePathToWhichToMakeAbsoluteTo);
	}

	public static DirectoryInfo? CombineDirectory(this DirectoryInfo? one, DirectoryInfo? two)
	{
		if (one == null) return two;
		else if (two == null) return one;

		else
		{
			var r = ZspPathHelper.Combine(one.FullName, two.FullName);
			return r == null ? null : new DirectoryInfo(r);
		}
	}

	public static DirectoryInfo? CombineDirectory(this DirectoryInfo? one,
		DirectoryInfo two,
		DirectoryInfo three,
		params DirectoryInfo[] fours)
	{
		var result = CombineDirectory(one, two);
		result = CombineDirectory(result, three);

		result = fours.Aggregate(result, CombineDirectory);
		return result;
	}

	public static DirectoryInfo? CombineDirectory(this DirectoryInfo? one, string? two)
	{
		if (one == null && two == null) return null;
		else if (two == null) return one;
		else if (one == null) return new(two);

		else
		{
			var r = ZspPathHelper.Combine(one.FullName, two);
			return r == null ? null : new DirectoryInfo(r);
		}
	}

	public static DirectoryInfo? CombineDirectory(this DirectoryInfo? one,
		string? two,
		string? three,
		params string?[] fours)
	{
		var result = CombineDirectory(one, two);
		result = CombineDirectory(result, three);

		result = fours.Aggregate(result, CombineDirectory);
		return result;
	}

	public static DirectoryInfo? CombineDirectory( /*this*/ string? one, string? two)
	{
		if (one == null && two == null) return null;
		else if (two == null) return new(one!);
		else if (one == null) return new(two);

		else
		{
			var r = ZspPathHelper.Combine(one, two);
			return r == null ? null : new DirectoryInfo(r);
		}
	}

	public static FileInfo? CombineFile(this DirectoryInfo? one, FileInfo? two)
	{
		if (one == null) return two;
		else if (two == null) return null;

		else
		{
			var r = ZspPathHelper.Combine(one.FullName, two.FullName);
			return r == null ? null : new FileInfo(r);
		}
	}

	public static FileInfo? CombineFile(
		this DirectoryInfo? one,
		FileInfo? two,
		FileInfo? three,
		params FileInfo?[] fours)
	{
		var result = CombineFile(one, two);
		result = CombineFile(result?.FullName, three?.FullName);

		return fours.Aggregate(result,
			(current, four) =>
				CombineFile(current?.FullName, four?.FullName));
	}

	public static FileInfo? CombineFile(this DirectoryInfo? one, string? two)
	{
		if (one == null && two == null) return null;
		else if (two == null) return null;
		else if (one == null) return new(two);

		else
		{
			var r = ZspPathHelper.Combine(one.FullName, two);
			return r == null ? null : new FileInfo(r);
		}
	}

	public static FileInfo? CombineFile(
		this DirectoryInfo? one,
		string? two,
		string? three,
		params string?[] fours)
	{
		var result = CombineFile(one, two);
		result = CombineFile(result?.FullName, three);

		return fours.Aggregate(result,
			(current, four) =>
				CombineFile(current?.FullName, four));
	}

	public static FileInfo? CombineFile( /*this*/ string? one, string? two)
	{
		if (one == null && two == null) return null;
		else if (two == null) return null;
		else if (one == null) return new(two);

		else
		{
			var r = ZspPathHelper.Combine(one, two);
			return r == null ? null : new FileInfo(r);
		}
	}

	/// <summary>
	/// Creates a copy of the calling instance with a changed extension.
	/// This calling instance remains unmodified.
	/// </summary>
	public static FileInfo? ChangeExtension(
		this FileInfo? o,
		string? extension)
	{
		var r = ZspPathHelper.ChangeExtension(o?.FullName, extension);
		return r == null ? null : new(r);
	}

	public static DirectoryInfo? CreateSubdirectory(this DirectoryInfo? o, string? name)
	{
		var path = ZspPathHelper.Combine(o?.FullName, name);
		if (path == null)
		{
			return null;
		}
		else
		{
			Directory.CreateDirectory(path);
			return new(path);
		}
	}

	public static bool EqualsNoCase(this DirectoryInfo? o, DirectoryInfo? p)
	{
		if (o == null && p == null) return true;
		else if (o == null || p == null) return false;

		return string.Equals(
			o.FullName.TrimEnd('\\', '/'),
			p.FullName.TrimEnd('\\', '/'),
			StringComparison.OrdinalIgnoreCase);
	}

	public static bool EqualsNoCase(this DirectoryInfo? o, string? p)
	{
		if (o == null && p == null) return true;
		else if (o == null || p == null) return false;

		return string.Equals(
			o.FullName.TrimEnd('\\', '/'),
			p.TrimEnd('\\', '/'),
			StringComparison.OrdinalIgnoreCase);
	}

	public static bool EqualsNoCase(this FileInfo? o, FileInfo? p)
	{
		if (o == null && p == null) return true;
		else if (o == null || p == null) return false;

		return string.Equals(
			o.FullName.TrimEnd('\\', '/'),
			p.FullName.TrimEnd('\\', '/'),
			StringComparison.OrdinalIgnoreCase);
	}

	public static bool EqualsNoCase(this FileInfo? o, string? p)
	{
		if (o == null && p == null) return true;
		else if (o == null || p == null) return false;

		return string.Equals(
			o.FullName.TrimEnd('\\', '/'),
			p.TrimEnd('\\', '/'),
			StringComparison.OrdinalIgnoreCase);
	}

	public static string? ReplaceNoCase(this string? s1, string? s2, string? s3)
	{
		if (s1 == null && s2 == null) return null;
		else if (s1 == null || s2 == null) return null;

		else return Replace(s1, s2, s3 ?? string.Empty, StringComparison.OrdinalIgnoreCase);
	}

	public static string Replace(
		this string? str,
		string oldValue,
		string? newValue,
		StringComparison comparison)
	{
		// http://stackoverflow.com/questions/244531/is-there-an-alternative-to-string-replace-that-is-case-insensitive

		var sb = new StringBuilder();

		var previousIndex = 0;
		var index = str?.IndexOf(oldValue, comparison) ?? -1;
		while (index != -1)
		{
			sb.Append(str?.Substring(previousIndex, index - previousIndex));
			sb.Append(newValue);
			index += oldValue.Length;

			previousIndex = index;
			index = str?.IndexOf(oldValue, index, comparison) ?? -1;
		}

		sb.Append(str?[previousIndex..]);

		return sb.ToString();
	}

	public static string? IfNullOrEmpty(this string? s, string? fallBack)
	{
		return string.IsNullOrEmpty(s) ? fallBack : s;
	}

	public static string? IfNullOrEmpty(this string? s, Func<string?>? fallBack)
	{
		return string.IsNullOrEmpty(s) ? fallBack?.Invoke() : s;
	}

	public static string? IfNullOrWhiteSpace(this string? s, string? fallBack)
	{
		return string.IsNullOrWhiteSpace(s) ? fallBack : s;
	}

	public static string? IfNullOrWhiteSpace(this string? s, Func<string?>? fallBack)
	{
		return string.IsNullOrWhiteSpace(s) ? fallBack?.Invoke() : s;
	}

	/// <summary>
	/// Similar to "S1 ?? S2", this simulates an operator that not only
	/// checks for NULL but also for an empty string.
	/// </summary>
	/// <returns>Returns S2 if S1 is NULL or empty, returns S1 otherwise.</returns>
	public static string? NullOrEmptyOther(this string? s1, string? s2)
	{
		return string.IsNullOrEmpty(s1) ? s2 : s1;
	}

	public static int IndexOfNoCase(this string? s1, string? s2)
	{
		if (s1 == null && s2 == null) return 0;
		else if (s1 == null || s2 == null) return -1;
		else return s1.IndexOf(s2, StringComparison.OrdinalIgnoreCase);
	}

	public static int IndexOfNoCase(this string? s1, string? s2, int startIndex)
	{
		if (s1 == null && s2 == null) return 0;
		else if (s1 == null || s2 == null) return -1;
		else return s1.IndexOf(s2, startIndex, StringComparison.OrdinalIgnoreCase);
	}

	public static int IndexOfNoCase(this string? s1, string? s2, int startIndex, int count)
	{
		if (s1 == null && s2 == null) return 0;
		else if (s1 == null || s2 == null) return -1;
		else return s1.IndexOf(s2, startIndex, count, StringComparison.OrdinalIgnoreCase);
	}

	public static int LastIndexOfNoCase(this string? s1, string? s2)
	{
		if (s1 == null && s2 == null) return 0;
		else if (s1 == null || s2 == null) return -1;
		else return s1.LastIndexOf(s2, StringComparison.OrdinalIgnoreCase);
	}

	public static bool EqualsNoCase(this string? s1, string? s2)
	{
		if (s1 == null && s2 == null) return true;
		else if (s1 == null || s2 == null) return false;
		else return string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);
	}

	public static bool StartsWithNoCase(this string? s1, string? s2)
	{
		if (s1 == null && s2 == null) return true;
		else if (s1 == null || s2 == null) return false;
		else return s1.StartsWith(s2, StringComparison.OrdinalIgnoreCase);
	}

	public static bool ContainsNoCase(this string? s1, string? s2)
	{
		if (s1 == null && s2 == null) return true;
		else if (s1 == null || s2 == null) return false;
		else return s1.IndexOf(s2, StringComparison.OrdinalIgnoreCase) >= 0;
	}

	public static bool ContainsNoCase(this string? s1, string? s2, int startIndex)
	{
		if (s1 == null && s2 == null) return true;
		else if (s1 == null || s2 == null) return false;
		else return s1.IndexOf(s2, startIndex, StringComparison.OrdinalIgnoreCase) >= 0;
	}

	public static bool EndsWithNoCase(this string? s1, string? s2)
	{
		if (s1 == null && s2 == null) return true;
		else if (s1 == null || s2 == null) return false;
		else return s1.EndsWith(s2, StringComparison.OrdinalIgnoreCase);
	}

	public static bool IsNullOrEmpty(this string? s)
	{
		return string.IsNullOrEmpty(s);
	}

	public static bool IsNullOrWhiteSpace(this string? s)
	{
		return string.IsNullOrWhiteSpace(s);
	}

	public static int CompareNoCase(this string? s1, string? s2)
	{
		switch (s1)
		{
			case null when s2 == null:
				return 0;
			case null:
				return 1;
			default:
			{
				if (s2 == null) return -1;
				else return string.Compare(s1, s2, StringComparison.OrdinalIgnoreCase);
			}
		}
	}

	public static bool EndsWithAnyNoCase(this string? s1, params string?[] s2)
	{
		if (s1 == null || s2.Length <= 0) return false;
		return s2.Any(s22 => !string.IsNullOrEmpty(s22) && s1.EndsWithNoCase(s22));
	}

	public static FileInfo CheckExists(this FileInfo? file)
	{
		if (file == null) throw new ArgumentNullException(nameof(file));

		if (!file.Exists)
		{
			throw new ZspException(string.Format(Resources.FileNotFound, file));
		}

		return file;
	}

	public static DirectoryInfo CheckExists(this DirectoryInfo? folder)
	{
		if (folder == null) throw new ArgumentNullException(nameof(folder));

		if (!folder.Exists)
		{
			throw new ZspException(string.Format(Resources.FolderNotFound, folder));
		}

		return folder;
	}

	public static DirectoryInfo CheckCreate(this DirectoryInfo? folder)
	{
		if (folder == null) throw new ArgumentNullException(nameof(folder));

		if (!folder.Exists) folder.Create();

		return folder;
	}

	public static bool IsSame(this DirectoryInfo? folder1, DirectoryInfo? folder2)
	{
		return IsSame(folder1, folder2?.FullName);
	}

	public static bool IsSame(this DirectoryInfo? folder1, string? folder2)
	{
		return ZspPathHelper.AreSameFolderPaths(folder1?.FullName, folder2);
	}

	public static bool IsSame(this FileInfo? file1, FileInfo? file2)
	{
		return IsSame(file1, file2?.FullName);
	}

	public static bool IsSame(this FileInfo? file1, string? file2)
	{
		return ZspPathHelper.AreSameFilePaths(file1?.FullName, file2);
	}

	public static bool StartsWith(this DirectoryInfo? folder1, DirectoryInfo? folder2)
	{
		return StartsWith(folder1, folder2?.FullName);
	}

	public static bool StartsWith(this DirectoryInfo? folder1, string? folder2)
	{
		var f1 = folder1?.FullName;

		return !string.IsNullOrEmpty(f1) && !string.IsNullOrEmpty(folder2) &&
		       f1.TrimEnd('\\').ToLowerInvariant()
			       .StartsWith(folder2.TrimEnd('\\').ToLowerInvariant() );
	}

	/// <summary>
	/// Converts a path with forward or backward slashs to use the platform's directory separator character.
	/// </summary>
	public static string? ConvertSlashsToPlatform(
		this string? path)
	{
		return ZspPathHelper.ConvertSlashsToPlatform(path);
	}

	public static string[] ReadAllLines(this FileInfo? file)
	{
		if (file == null) throw new ArgumentNullException(nameof(file));
		return File.ReadAllLines(file.FullName);
	}

	public static void CopyTo(this FileInfo? file, FileInfo other, bool overwrite = false)
	{
		if (file == null) throw new ArgumentNullException(nameof(file));
		file.CopyTo(other.FullName, overwrite);
	}

	public static ZspFileDateInfos GetDateInfos(this FileInfo? file)
	{
		return ZspIOHelper.GetFileDateInfos(file?.FullName);
	}

	public static void SetDateInfos(this FileInfo? file, ZspFileDateInfos? infos)
	{
		ZspIOHelper.SetFileDateInfos(file?.FullName, infos);
	}

	public static FileInfo[] GetFiles(this DirectoryInfo? folder)
	{
		if (folder == null) throw new ArgumentNullException(nameof(folder));
		return ZspIOHelper.GetFiles(folder.FullName);
	}

	public static FileInfo[] GetFiles(this DirectoryInfo? folder, SearchOption searchOption)
	{
		if (folder == null) throw new ArgumentNullException(nameof(folder));
		return ZspIOHelper.GetFiles(folder.FullName, searchOption);
	}

	public static FileInfo[] GetFiles(this DirectoryInfo? folder, string? pattern, SearchOption searchOption)
	{
		if (folder == null) throw new ArgumentNullException(nameof(folder));
		return ZspIOHelper.GetFiles(folder.FullName, pattern, searchOption);
	}
}