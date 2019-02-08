namespace ZetaShortPaths
{
    using JetBrains.Annotations;
    using System.IO;

    [UsedImplicitly]
    public static class FileInfoExtensions
    {
        [UsedImplicitly]
        public static void WriteAllText(this FileInfo i, string text)
        {
            File.WriteAllText(i.FullName, text);
        }

        [UsedImplicitly]
        public static void MoveTo(this FileInfo i, FileInfo dst)
        {
            i.MoveTo(dst.FullName);
        }

        [UsedImplicitly]
        public static void MoveTo(this FileInfo i, string dst, bool overwriteExisting)
        {
            if(overwriteExisting&&File.Exists(dst))File.Delete(dst);
            i.MoveTo(dst);
        }

        [UsedImplicitly]
        public static string MD5Hash(this FileInfo i)
        {
            return ZspIOHelper.CalculateMD5Hash(i.FullName);
        }
    }
}