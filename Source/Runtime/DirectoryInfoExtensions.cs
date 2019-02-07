namespace ZetaShortPaths
{
    using JetBrains.Annotations;
    using System.IO;

    [UsedImplicitly]
    public static class DirectoryInfoExtensions
    {
        [UsedImplicitly]
        public static FileSystemInfo[] GetFileSystemInfos(this DirectoryInfo i, SearchOption searchOption)
        {
            return i.GetFileSystemInfos(@"*", searchOption);
        }

        public static void MoveTo(this DirectoryInfo i, DirectoryInfo to)
        {
            doCopyDir(i.FullName, to.FullName);
        }

        private static void doCopyDir(string sourceFolder, string destFolder)
        {
            // https://stackoverflow.com/a/3911658/107625

            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);

            // Get Files & Copy
            var files = Directory.GetFiles(sourceFolder);
            foreach (var file in files)
            {
                var name = Path.GetFileName(file);

                // ADD Unique File Name Check to Below!!!!
                var dest = Path.Combine(destFolder, name);
                File.Copy(file, dest);
            }

            // Get dirs recursively and copy files
            var folders = Directory.GetDirectories(sourceFolder);
            foreach (var folder in folders)
            {
                var name = Path.GetFileName(folder);
                var dest = Path.Combine(destFolder, name);
                doCopyDir(folder, dest);
            }
        }
    }
}
