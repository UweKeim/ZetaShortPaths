namespace ZetaShortPaths;

public class ZspSimpleFileAccessProtectorException :
    Exception
{
    [UsedImplicitly]
    public ZspSimpleFileAccessProtectorException()
    {
    }

    [UsedImplicitly]
    public ZspSimpleFileAccessProtectorException(string message) : base(message)
    {
    }

    [UsedImplicitly]
    public ZspSimpleFileAccessProtectorException(string message, Exception inner) : base(message, inner)
    {
    }
}