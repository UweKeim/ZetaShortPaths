namespace ZetaShortPaths;

[PublicAPI]
public class ZspHandleExceptionInfo(
    Exception exception,
    int currentRetryCount,
    ZspSimpleFileAccessProtectorInformation info)
{
	public Exception Exception { get; } = exception;
	public int CurrentRetryCount { get; } = currentRetryCount;

	/// <summary>
	/// For informational purposes only inside the exception handler delegate.
	/// </summary>
    public ZspSimpleFileAccessProtectorInformation Info { get; } = info;

    /// <summary>
	/// Return value. Set optionally to TRUE to force premature throwing.
	/// </summary>
	[DefaultValue(false)]
	public bool WantThrow { get; set; }
}