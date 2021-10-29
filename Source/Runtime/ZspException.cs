namespace ZetaShortPaths;

[Serializable]
[UsedImplicitly]
public class ZspException : Exception
{
    public ZspException()
    {
    }

    public ZspException(string message) : base(message)
    {
    }

    public ZspException(string message, Exception inner) : base(message, inner)
    {
    }

    protected ZspException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
}