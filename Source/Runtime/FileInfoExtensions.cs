namespace ZetaShortPaths
{
    using JetBrains.Annotations;
    using System.IO;
    using System.Text;

    [UsedImplicitly]
    public static class FileInfoExtensions
    {
        [UsedImplicitly]
        public static void WriteAllText(this FileInfo i, string text)
        {
            File.WriteAllText(i.FullName, text);
        }

        [UsedImplicitly]
        public static void WriteAllText(this FileInfo i, string text, Encoding encoding)
        {
            File.WriteAllText(i.FullName, text, encoding);
        }

        [UsedImplicitly]
        public static string ReadAllText(this FileInfo i)
        {
            return File.ReadAllText(i.FullName);
        }

        [UsedImplicitly]
        public static string ReadAllText(this FileInfo i, Encoding encoding)
        {
            return File.ReadAllText(i.FullName, encoding);
        }

        [UsedImplicitly]
        public static void WriteAllBytes(this FileInfo i, byte[] bytes)
        {
            File.WriteAllBytes(i.FullName, bytes);
        }

        [UsedImplicitly]
        public static byte[] ReadAllBytes(this FileInfo i)
        {
            return File.ReadAllBytes(i.FullName);
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