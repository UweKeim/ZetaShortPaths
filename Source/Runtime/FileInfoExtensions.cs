namespace ZetaShortPaths;

[PublicAPI]
public static class FileInfoExtensions
{
    [PublicAPI]
    public static void WriteAllText(this FileInfo i, string text)
    {
        File.WriteAllText(i.FullName, text);
    }

    [PublicAPI]
    public static void WriteAllText(this FileInfo i, string text, Encoding encoding)
    {
        File.WriteAllText(i.FullName, text, encoding);
    }

    [PublicAPI]
    public static string ReadAllText(this FileInfo i)
    {
        return File.ReadAllText(i.FullName);
    }

    [PublicAPI]
    public static string ReadAllText(this FileInfo i, Encoding encoding)
    {
        return File.ReadAllText(i.FullName, encoding);
    }

    [PublicAPI]
    public static void WriteAllBytes(this FileInfo i, byte[] bytes)
    {
        File.WriteAllBytes(i.FullName, bytes);
    }

    [PublicAPI]
    public static byte[] ReadAllBytes(this FileInfo i)
    {
        return File.ReadAllBytes(i.FullName);
    }

    [PublicAPI]
    public static void MoveTo(this FileInfo i, FileInfo dst)
    {
        i.MoveTo(dst.FullName);
    }

    [PublicAPI]
    public static void MoveTo(this FileInfo i, string dst, bool overwriteExisting)
    {
        if(overwriteExisting&&File.Exists(dst))File.Delete(dst);
        i.MoveTo(dst);
    }

    [PublicAPI]
    public static string MD5Hash(this FileInfo i)
    {
        return ZspIOHelper.CalculateMD5Hash(i.FullName);
    }
}