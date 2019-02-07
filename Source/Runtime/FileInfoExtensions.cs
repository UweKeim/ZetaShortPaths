namespace ZetaShortPaths
{
    using System.IO;
    using JetBrains.Annotations;

    [UsedImplicitly]
    public static class FileInfoExtensions
    {
        [UsedImplicitly]
        public static void WriteAllText(this FileInfo i, string text)
        {
            File.WriteAllText(i.FullName, text);
        }
    }
}