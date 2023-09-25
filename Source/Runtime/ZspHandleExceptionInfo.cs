namespace ZetaShortPaths;

[PublicAPI]
public class ZspHandleExceptionInfo(Exception exception, int currentRetryCount)
{
	public Exception Exception { get; } = exception;
	public int CurrentRetryCount { get; } = currentRetryCount;

	/// <summary>
	/// Return value. Set optionally to TRUE to force premature throwing.
	/// </summary>
	[DefaultValue(false)]
	public bool WantThrow { get; set; }
}