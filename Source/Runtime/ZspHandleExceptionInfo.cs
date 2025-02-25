namespace ZetaShortPaths;

[PublicAPI]
public class ZspHandleExceptionInfo(
	Exception exception,
	int currentRetryCount,
	ZspSimpleFileAccessProtectorInformation info)
{
	/// <summary>
	/// When this value is assigned, the assigned exception is thrown instead of the original one.
	/// </summary>
	public Exception Exception { get; set; } = exception;

	/// <summary>
	/// How often already retried.
	/// </summary>
	public int CurrentRetryCount { get; } = currentRetryCount;

	/// <summary>
	/// For informational purposes only inside the exception handler delegate.
	/// </summary>
	public ZspSimpleFileAccessProtectorInformation Info { get; } = info;

	/// <summary>
	/// Return value. Set optionally to TRUE to force premature throwing.
	/// When called for the last time, this value is ignored.
	/// An exception is always thrown in that case.
	/// </summary>
	[DefaultValue(false)]
	public bool WantThrow { get; set; }
}