namespace ZetaShortPaths
{
    using JetBrains.Annotations;
    using Properties;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// "Nice to have" extensions.
    /// </summary>
    public static class ZspExtensions
    {
        [UsedImplicitly]
        public static string MakeRelativeTo(
            this DirectoryInfo pathToMakeRelative,
            DirectoryInfo pathToWhichToMakeRelativeTo)
        {
            return ZspPathHelper.GetRelativePath(pathToWhichToMakeRelativeTo.FullName, pathToMakeRelative.FullName);
        }

        [UsedImplicitly]
        public static string MakeRelativeTo(
            this DirectoryInfo pathToMakeRelative,
            string pathToWhichToMakeRelativeTo)
        {
            return ZspPathHelper.GetRelativePath(pathToWhichToMakeRelativeTo, pathToMakeRelative.FullName);
        }

        [UsedImplicitly]
        public static string MakeRelativeTo(
            this FileInfo pathToMakeRelative,
            DirectoryInfo pathToWhichToMakeRelativeTo)
        {
            return ZspPathHelper.GetRelativePath(pathToWhichToMakeRelativeTo.FullName, pathToMakeRelative.FullName);
        }

        [UsedImplicitly]
        public static string MakeRelativeTo(
            this FileInfo pathToMakeRelative,
            string pathToWhichToMakeRelativeTo)
        {
            return ZspPathHelper.GetRelativePath(pathToWhichToMakeRelativeTo, pathToMakeRelative.FullName);
        }

        [UsedImplicitly]
        public static string MakeAbsoluteTo(
            this DirectoryInfo pathToMakeAbsolute,
            DirectoryInfo basePathToWhichToMakeAbsoluteTo)
        {
            return ZspPathHelper.GetAbsolutePath(pathToMakeAbsolute.FullName, basePathToWhichToMakeAbsoluteTo.FullName);
        }

        [UsedImplicitly]
        public static string MakeAbsoluteTo(
            this string pathToMakeAbsolute,
            DirectoryInfo basePathToWhichToMakeAbsoluteTo)
        {
            return ZspPathHelper.GetAbsolutePath(pathToMakeAbsolute, basePathToWhichToMakeAbsoluteTo.FullName);
        }

        [UsedImplicitly]
        public static string MakeAbsoluteTo(
            this DirectoryInfo pathToMakeAbsolute,
            string basePathToWhichToMakeAbsoluteTo)
        {
            return ZspPathHelper.GetAbsolutePath(pathToMakeAbsolute.FullName, basePathToWhichToMakeAbsoluteTo);
        }

        [UsedImplicitly]
        public static string MakeAbsoluteTo(
            this FileInfo pathToMakeAbsolute,
            DirectoryInfo basePathToWhichToMakeAbsoluteTo)
        {
            return ZspPathHelper.GetAbsolutePath(pathToMakeAbsolute.FullName, basePathToWhichToMakeAbsoluteTo.FullName);
        }

        [UsedImplicitly]
        public static string MakeAbsoluteTo(
            this FileInfo pathToMakeAbsolute,
            string basePathToWhichToMakeAbsoluteTo)
        {
            return ZspPathHelper.GetAbsolutePath(pathToMakeAbsolute.FullName, basePathToWhichToMakeAbsoluteTo);
        }

        [UsedImplicitly]
        public static DirectoryInfo CombineDirectory(this DirectoryInfo one, DirectoryInfo two)
        {
            if (one == null) return two;
            else if (two == null) return one;

            else return new DirectoryInfo(ZspPathHelper.Combine(one.FullName, two.FullName));
        }

        [UsedImplicitly]
        public static DirectoryInfo CombineDirectory(this DirectoryInfo one,
            DirectoryInfo two,
            DirectoryInfo three,
            params DirectoryInfo[] fours)
        {
            var result = CombineDirectory(one, two);
            result = CombineDirectory(result, three);

            result = fours.Aggregate(result, CombineDirectory);
            return result;
        }

        public static DirectoryInfo CombineDirectory(this DirectoryInfo one, string two)
        {
            if (one == null && two == null) return null;
            else if (two == null) return one;
            else if (one == null) return new DirectoryInfo(two);

            else return new DirectoryInfo(ZspPathHelper.Combine(one.FullName, two));
        }

        [UsedImplicitly]
        public static DirectoryInfo CombineDirectory(this DirectoryInfo one,
            string two,
            string three,
            params string[] fours)
        {
            var result = CombineDirectory(one, two);
            result = CombineDirectory(result, three);

            result = fours.Aggregate(result, CombineDirectory);
            return result;
        }

        [UsedImplicitly]
        public static DirectoryInfo CombineDirectory( /*this*/ string one, string two)
        {
            if (one == null && two == null) return null;
            else if (two == null) return new DirectoryInfo(one);
            else if (one == null) return new DirectoryInfo(two);

            else return new DirectoryInfo(ZspPathHelper.Combine(one, two));
        }

        [UsedImplicitly]
        public static FileInfo CombineFile(this DirectoryInfo one, FileInfo two)
        {
            if (one == null) return two;
            else if (two == null) return null;

            else return new FileInfo(ZspPathHelper.Combine(one.FullName, two.FullName));
        }

        [UsedImplicitly]
        public static FileInfo CombineFile(
            this DirectoryInfo one,
            FileInfo two,
            FileInfo three,
            params FileInfo[] fours)
        {
            var result = CombineFile(one, two);
            result = CombineFile(result?.FullName, three?.FullName);

            return fours.Aggregate(result,
                (current, four) =>
                    CombineFile(current?.FullName, four?.FullName));
        }

        public static FileInfo CombineFile(this DirectoryInfo one, string two)
        {
            if (one == null && two == null) return null;
            else if (two == null) return null;
            else if (one == null) return new FileInfo(two);

            else return new FileInfo(ZspPathHelper.Combine(one.FullName, two));
        }

        [UsedImplicitly]
        public static FileInfo CombineFile(this DirectoryInfo one, string two, string three,
            params string[] fours)
        {
            var result = CombineFile(one, two);
            result = CombineFile(result?.FullName, three);

            return fours.Aggregate(result,
                (current, four) =>
                    CombineFile(current?.FullName, four));
        }

        [UsedImplicitly]
        public static FileInfo CombineFile( /*this*/ string one, string two)
        {
            if (one == null && two == null) return null;
            else if (two == null) return null;
            else if (one == null) return new FileInfo(two);

            else return new FileInfo(ZspPathHelper.Combine(one, two));
        }

        /// <summary>
        /// Creates a copy of the calling instance with a changed extension.
        /// This calling instance remains unmodified.
        /// </summary>
        [UsedImplicitly]
        public static FileInfo ChangeExtension(
            this FileInfo o,
            string extension)
        {
            return new FileInfo(ZspPathHelper.ChangeExtension(o.FullName, extension));
        }

        [UsedImplicitly]
        public static DirectoryInfo CreateSubdirectory(this DirectoryInfo o, string name)
        {
            var path = ZspPathHelper.Combine(o.FullName, name);
            Directory.CreateDirectory(path);
            return new DirectoryInfo(path);
        }

        [UsedImplicitly]
        public static bool EqualsNoCase(this DirectoryInfo o, DirectoryInfo p)
        {
            if (o == null && p == null) return true;
            else if (o == null || p == null) return false;

            return string.Equals(
                o.FullName.TrimEnd('\\', '/'),
                p.FullName.TrimEnd('\\', '/'),
                StringComparison.OrdinalIgnoreCase);
        }

        [UsedImplicitly]
        public static bool EqualsNoCase(this DirectoryInfo o, string p)
        {
            if (o == null && p == null) return true;
            else if (o == null || p == null) return false;

            return string.Equals(
                o.FullName.TrimEnd('\\', '/'),
                p.TrimEnd('\\', '/'),
                StringComparison.OrdinalIgnoreCase);
        }

        [UsedImplicitly]
        public static bool EqualsNoCase(this FileInfo o, FileInfo p)
        {
            if (o == null && p == null) return true;
            else if (o == null || p == null) return false;

            return string.Equals(
                o.FullName.TrimEnd('\\', '/'),
                p.FullName.TrimEnd('\\', '/'),
                StringComparison.OrdinalIgnoreCase);
        }

        [UsedImplicitly]
        public static bool EqualsNoCase(this FileInfo o, string p)
        {
            if (o == null && p == null) return true;
            else if (o == null || p == null) return false;

            return string.Equals(
                o.FullName.TrimEnd('\\', '/'),
                p.TrimEnd('\\', '/'),
                StringComparison.OrdinalIgnoreCase);
        }

        [UsedImplicitly]
        public static string ReplaceNoCase(this string s1, string s2, string s3)
        {
            if (s1 == null && s2 == null) return null;
            else if (s1 == null || s2 == null) return null;

            else return Replace(s1, s2, s3 ?? string.Empty, StringComparison.OrdinalIgnoreCase);
        }

        [UsedImplicitly]
        public static string Replace(
            this string str,
            string oldValue,
            string newValue,
            StringComparison comparison)
        {
            // http://stackoverflow.com/questions/244531/is-there-an-alternative-to-string-replace-that-is-case-insensitive

            var sb = new StringBuilder();

            var previousIndex = 0;
            var index = str.IndexOf(oldValue, comparison);
            while (index != -1)
            {
                sb.Append(str.Substring(previousIndex, index - previousIndex));
                sb.Append(newValue);
                index += oldValue.Length;

                previousIndex = index;
                index = str.IndexOf(oldValue, index, comparison);
            }
            sb.Append(str.Substring(previousIndex));

            return sb.ToString();
        }

        /// <summary>
        /// Similar to "S1 ?? S2", this simulates an operator that not only
        /// checks for NULL but also for an empty string.
        /// </summary>
        /// <returns>Returns S2 if S1 is NULL or empty, returns S1 otherwise.</returns>
        [UsedImplicitly]
        public static string NullOrEmptyOther(this string s1, string s2)
        {
            return string.IsNullOrEmpty(s1) ? s2 : s1;
        }

        [UsedImplicitly]
        public static int IndexOfNoCase(this string s1, string s2)
        {
            if (s1 == null && s2 == null) return 0;
            else if (s1 == null || s2 == null) return -1;
            else return s1.IndexOf(s2, StringComparison.OrdinalIgnoreCase);
        }

        [UsedImplicitly]
        public static int LastIndexOfNoCase(this string s1, string s2)
        {
            if (s1 == null && s2 == null) return 0;
            else if (s1 == null || s2 == null) return -1;
            else return s1.LastIndexOf(s2, StringComparison.OrdinalIgnoreCase);
        }

        [UsedImplicitly]
        public static bool EqualsNoCase(this string s1, string s2)
        {
            if (s1 == null && s2 == null) return true;
            else if (s1 == null || s2 == null) return false;
            else return string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);
        }

        [UsedImplicitly]
        public static bool StartsWithNoCase(this string s1, string s2)
        {
            if (s1 == null && s2 == null) return true;
            else if (s1 == null || s2 == null) return false;
            else return s1.StartsWith(s2, StringComparison.OrdinalIgnoreCase);
        }

        [UsedImplicitly]
        public static bool ContainsNoCase(this string s1, string s2)
        {
            if (s1 == null && s2 == null) return true;
            else if (s1 == null || s2 == null) return false;
            else return s1.IndexOf(s2, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        [UsedImplicitly]
        public static bool ContainsNoCase(this string s1, string s2, int startIndex)
        {
            if (s1 == null && s2 == null) return true;
            else if (s1 == null || s2 == null) return false;
            else return s1.IndexOf(s2, startIndex, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        [UsedImplicitly]
        public static bool EndsWithNoCase(this string s1, string s2)
        {
            if (s1 == null && s2 == null) return true;
            else if (s1 == null || s2 == null) return false;
            else return s1.EndsWith(s2, StringComparison.OrdinalIgnoreCase);
        }

        [UsedImplicitly]
        public static int CompareNoCase(this string s1, string s2)
        {
            if (s1 == null && s2 == null) return 0;
            else if (s1 == null) return 1;
            else if (s2 == null) return -1;
            else return string.Compare(s1, s2, StringComparison.OrdinalIgnoreCase);
        }

        [UsedImplicitly]
        public static bool EndsWithAnyNoCase(this string s1, params string[] s2)
        {
            if (s1 == null || s2 == null) return false;
            return s2.Any(s22 => !string.IsNullOrEmpty(s22) && s1.EndsWithNoCase(s22));
        }

        [UsedImplicitly]
        public static FileInfo CheckExists(this FileInfo file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            if (!file.Exists)
            {
                throw new ZspException(string.Format(Resources.FileNotFound, file));
            }

            return file;
        }

        [UsedImplicitly]
        public static DirectoryInfo CheckExists(this DirectoryInfo folder)
        {
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            if (!folder.Exists)
            {
                throw new ZspException(string.Format(Resources.FolderNotFound, folder));
            }

            return folder;
        }

        public static DirectoryInfo CheckCreate(this DirectoryInfo folder)
        {
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            if (!folder.Exists) folder.Create();

            return folder;
        }

        [UsedImplicitly]
        public static bool IsSame(this DirectoryInfo folder1, DirectoryInfo folder2)
        {
            return IsSame(folder1, folder2?.FullName);
        }

        [UsedImplicitly]
        public static bool IsSame(this DirectoryInfo folder1, string folder2)
        {
            return ZspPathHelper.AreSameFolders(folder1.FullName, folder2);
        }

        [UsedImplicitly]
        public static bool StartsWith(this DirectoryInfo folder1, DirectoryInfo folder2)
        {
            return StartsWith(folder1, folder2?.FullName);
        }

        [UsedImplicitly]
        public static bool StartsWith(this DirectoryInfo folder1, string folder2)
        {
            var f1 = folder1?.FullName;

            return !string.IsNullOrEmpty(f1) && !string.IsNullOrEmpty(folder2) &&
                   f1.TrimEnd('\\').ToLowerInvariant().StartsWith(folder2.TrimEnd('\\').ToLowerInvariant());
        }
    }
}