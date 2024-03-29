﻿namespace ZetaShortPaths;

[Serializable]
[PublicAPI]
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
}