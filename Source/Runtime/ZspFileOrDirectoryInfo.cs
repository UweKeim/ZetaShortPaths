namespace ZetaShortPaths
{
    using JetBrains.Annotations;
    using System;
    using System.IO;

    public sealed class ZspFileOrDirectoryInfo
    {
        public enum PreferedType
        {
            Unspecified,
            File,
            Directory
        }

        private readonly string _fullPath;
        private readonly string _originalPath;

        private PreferedType _preferedType;

        [UsedImplicitly]
        public ZspFileOrDirectoryInfo()
        {
            _preferedType = PreferedType.Unspecified;
        }

        public ZspFileOrDirectoryInfo(
            string fullPath)
        {
            _preferedType = PreferedType.Unspecified;
            _fullPath = fullPath;
            _originalPath = fullPath;
        }

        [UsedImplicitly]
        public ZspFileOrDirectoryInfo(
            string fullPath,
            bool detectTypeFromFileSystem)
        {
            _fullPath = fullPath;
            _originalPath = fullPath;

            if (detectTypeFromFileSystem)
            {
                _preferedType =
                    IsFile
                        ? PreferedType.File
                        : IsDirectory
                            ? PreferedType.Directory
                            : PreferedType.Unspecified;
            }
            else
            {
                _preferedType = PreferedType.Unspecified;
            }
        }

        public ZspFileOrDirectoryInfo(
            string fullPath,
            PreferedType preferedType)
        {
            _preferedType = preferedType;
            _fullPath = fullPath;
            _originalPath = fullPath;
        }

        public ZspFileOrDirectoryInfo(
            ZspFileOrDirectoryInfo info)
        {
            _preferedType = info._preferedType;
            _fullPath = info.FullName;
            _originalPath = info._originalPath;
        }

        public ZspFileOrDirectoryInfo(
                // ReSharper disable SuggestBaseTypeForParameter
                FileInfo info)
        // ReSharper restore SuggestBaseTypeForParameter
        {
            _preferedType = PreferedType.File;
            _fullPath = info.FullName;
            _originalPath = info.ToString();
        }

        public ZspFileOrDirectoryInfo(
                // ReSharper disable SuggestBaseTypeForParameter
                DirectoryInfo info)
        // ReSharper restore SuggestBaseTypeForParameter
        {
            _preferedType = PreferedType.Directory;
            _fullPath = info.FullName;
            _originalPath = info.ToString();
        }

        [UsedImplicitly]
        public ZspFileOrDirectoryInfo Clone()
        {
            return new ZspFileOrDirectoryInfo(this);
        }

        public bool IsEmpty => string.IsNullOrEmpty(_fullPath);

        public FileInfo File => new FileInfo(_fullPath);

        public DirectoryInfo Directory => new DirectoryInfo(_fullPath);

        [UsedImplicitly]
        public DirectoryInfo EffectiveDirectory
        {
            get
            {
                switch (_preferedType)
                {
                    case PreferedType.File:
                        return File.Directory;

                    case PreferedType.Unspecified:
                        if (ZspSafeFileOperations.SafeDirectoryExists(Directory))
                            return Directory;
                        else if (ZspSafeFileOperations.SafeFileExists(File))
                            return File.Directory;
                        else
                            return Directory;

                    case PreferedType.Directory:
                        return Directory;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Get a value indicating whether the file or the directory exists.
        /// </summary>
        [UsedImplicitly]
        public bool Exists
        {
            get
            {
                if (string.IsNullOrEmpty(_fullPath))
                {
                    return false;
                }
                else
                {
                    switch (_preferedType)
                    {
                        default:
                            return Directory.Exists || File.Exists;
                        case PreferedType.File:
                            return File.Exists || Directory.Exists;
                    }
                }
            }
        }

        public string FullName => _fullPath;

        [UsedImplicitly]
        public string OriginalPath => _originalPath;

        public ZspSplittedPath ZspSplittedPath => new ZspSplittedPath(this);

        [UsedImplicitly]
        public string Name => IsFile ? File?.Name : Directory?.Name;

        /// <summary>
        /// Gets a value indicating whether this instance is file by querying the file system
        /// whether the file exists on disk.
        /// </summary>
        public bool IsFile => File.Exists;

        /// <summary>
        /// Gets a value indicating whether this instance is directory by quering the file system
        /// whether the directory exists on disk.
        /// </summary>
        public bool IsDirectory => Directory.Exists;

        [UsedImplicitly]
        public static int Compare(
            DirectoryInfo one,
            DirectoryInfo two)
        {
            return string.Compare(one.FullName.TrimEnd('\\'), two.FullName.TrimEnd('\\'),
                StringComparison.OrdinalIgnoreCase);
        }

        [UsedImplicitly]
        public static int Compare(
            FileInfo one,
            FileInfo two)
        {
            return string.Compare(one.FullName.TrimEnd('\\'), two.FullName.TrimEnd('\\'),
                StringComparison.OrdinalIgnoreCase);
        }

        [UsedImplicitly]
        public static int Compare(
            ZspFileOrDirectoryInfo one,
            ZspFileOrDirectoryInfo two)
        {
            return string.Compare(one.FullName.TrimEnd('\\'), two.FullName.TrimEnd('\\'),
                StringComparison.OrdinalIgnoreCase);
        }

        [UsedImplicitly]
        public static int Compare(
            ZspFileOrDirectoryInfo one,
            FileInfo two)
        {
            return string.Compare(one.FullName.TrimEnd('\\'), two.FullName.TrimEnd('\\'),
                StringComparison.OrdinalIgnoreCase);
        }

        [UsedImplicitly]
        public static int Compare(
            ZspFileOrDirectoryInfo one,
            DirectoryInfo two)
        {
            return string.Compare(one.FullName.TrimEnd('\\'), two.FullName.TrimEnd('\\'),
                StringComparison.OrdinalIgnoreCase);
        }

        [UsedImplicitly]
        public static int Compare(
            string one,
            DirectoryInfo two)
        {
            return new ZspFileOrDirectoryInfo(one).Compare(two);
        }

        [UsedImplicitly]
        public static int Compare(
            string one,
            FileInfo two)
        {
            return new ZspFileOrDirectoryInfo(one).Compare(two);
        }

        [UsedImplicitly]
        public static int Compare(
            string one,
            ZspFileOrDirectoryInfo two)
        {
            return new ZspFileOrDirectoryInfo(one).Compare(two);
        }

        [UsedImplicitly]
        public static int Compare(
            DirectoryInfo one,
            string two)
        {
            return new ZspFileOrDirectoryInfo(one).Compare(two);
        }

        [UsedImplicitly]
        public static int Compare(
            FileInfo one,
            string two)
        {
            return new ZspFileOrDirectoryInfo(one).Compare(two);
        }

        [UsedImplicitly]
        public static int Compare(
            ZspFileOrDirectoryInfo one,
            string two)
        {
            return new ZspFileOrDirectoryInfo(one).Compare(two);
        }

        [UsedImplicitly]
        public static int Compare(
            string one,
            string two)
        {
            return new ZspFileOrDirectoryInfo(one).Compare(two);
        }

        [UsedImplicitly]
        public int Compare(
            string b)
        {
            return Compare(this, new ZspFileOrDirectoryInfo(b));
        }

        [UsedImplicitly]
        public int Compare(
            FileInfo b)
        {
            return Compare(b.FullName);
        }

        [UsedImplicitly]
        public int Compare(
            DirectoryInfo b)
        {
            return Compare(b.FullName);
        }

        [UsedImplicitly]
        public int Compare(
            ZspFileOrDirectoryInfo b)
        {
            return Compare(b.FullName);
        }

        [UsedImplicitly]
        public static ZspFileOrDirectoryInfo Combine(
            DirectoryInfo one,
            DirectoryInfo two)
        {
            return new ZspFileOrDirectoryInfo(one).Combine(two);
        }

        [UsedImplicitly]
        public static ZspFileOrDirectoryInfo Combine(
            FileInfo one,
            FileInfo two)
        {
            return new ZspFileOrDirectoryInfo(one).Combine(two);
        }

        [UsedImplicitly]
        public static ZspFileOrDirectoryInfo Combine(
            ZspFileOrDirectoryInfo one,
            ZspFileOrDirectoryInfo two)
        {
            return new ZspFileOrDirectoryInfo(one).Combine(two);
        }

        [UsedImplicitly]
        public static ZspFileOrDirectoryInfo Combine(
            ZspFileOrDirectoryInfo one,
            FileInfo two)
        {
            return new ZspFileOrDirectoryInfo(one).Combine(two);
        }

        [UsedImplicitly]
        public static ZspFileOrDirectoryInfo Combine(
            ZspFileOrDirectoryInfo one,
            DirectoryInfo two)
        {
            return new ZspFileOrDirectoryInfo(one).Combine(two);
        }

        [UsedImplicitly]
        public static ZspFileOrDirectoryInfo Combine(
            string one,
            DirectoryInfo two)
        {
            return new ZspFileOrDirectoryInfo(one).Combine(two);
        }

        [UsedImplicitly]
        public static ZspFileOrDirectoryInfo Combine(
            string one,
            FileInfo two)
        {
            return new ZspFileOrDirectoryInfo(one).Combine(two);
        }

        [UsedImplicitly]
        public static ZspFileOrDirectoryInfo Combine(
            string one,
            ZspFileOrDirectoryInfo two)
        {
            return new ZspFileOrDirectoryInfo(one).Combine(two);
        }

        [UsedImplicitly]
        public static ZspFileOrDirectoryInfo Combine(
            DirectoryInfo one,
            string two)
        {
            return new ZspFileOrDirectoryInfo(one).Combine(two);
        }

        [UsedImplicitly]
        public static ZspFileOrDirectoryInfo Combine(
            FileInfo one,
            string two)
        {
            return new ZspFileOrDirectoryInfo(one).Combine(two);
        }

        [UsedImplicitly]
        public static ZspFileOrDirectoryInfo Combine(
            ZspFileOrDirectoryInfo one,
            string two)
        {
            return new ZspFileOrDirectoryInfo(one).Combine(two);
        }

        [UsedImplicitly]
        public static ZspFileOrDirectoryInfo Combine(
            string one,
            string two)
        {
            return new ZspFileOrDirectoryInfo(one).Combine(two);
        }

        [UsedImplicitly]
        public ZspFileOrDirectoryInfo Combine(
            ZspFileOrDirectoryInfo info)
        {
            return
                new ZspFileOrDirectoryInfo(
                    ZspPathHelper.Combine(
                        EffectiveDirectory.FullName,
                        info.FullName));
        }

        [UsedImplicitly]
        public ZspFileOrDirectoryInfo Combine(
            string path)
        {
            return
                new ZspFileOrDirectoryInfo(
                    ZspPathHelper.Combine(
                        EffectiveDirectory.FullName,
                        path));
        }

        [UsedImplicitly]
        public ZspFileOrDirectoryInfo Combine(
            FileInfo info)
        {
            return
                new ZspFileOrDirectoryInfo(
                    ZspPathHelper.Combine(
                        EffectiveDirectory.FullName,
                        // According to Reflector, "ToString()" returns the 
                        // "OriginalPath". This is what we need here.
                        info.ToString()));
        }

        [UsedImplicitly]
        public ZspFileOrDirectoryInfo Combine(
            DirectoryInfo info)
        {
            return
                new ZspFileOrDirectoryInfo(
                    ZspPathHelper.Combine(
                        EffectiveDirectory.FullName,
                        // According to Reflector, "ToString()" returns the 
                        // "OriginalPath". This is what we need here.
                        info.ToString()));
        }

        /// <summary>
        /// Schaut ins Dateisystem, wenn der Typ "unspecified" ist und versucht den
        /// korreten Typ festzustellen.
        /// </summary>
        [UsedImplicitly]
        public void LookupType()
        {
            if (_preferedType == PreferedType.Unspecified)
            {
                _preferedType =
                    IsFile
                        ? PreferedType.File
                        : IsDirectory
                            ? PreferedType.Directory
                            : PreferedType.Unspecified;
            }
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(_originalPath)
                ? _fullPath
                : _originalPath;
        }
    }
}