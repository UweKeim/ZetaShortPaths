namespace ZetaShortPaths;

public class ZspSimpleFileAccessProtectorException :
    Exception
{
    [PublicAPI]
    public ZspSimpleFileAccessProtectorException()
    {
    }

    [PublicAPI]
    public ZspSimpleFileAccessProtectorException(string message) : base(message)
    {
    }

    [PublicAPI]
    public ZspSimpleFileAccessProtectorException(string message, Exception inner) : base(message, inner)
    {
    }
}