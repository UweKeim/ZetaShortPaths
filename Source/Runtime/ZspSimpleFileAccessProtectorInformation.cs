namespace ZetaShortPaths;

[PublicAPI]
public sealed class ZspSimpleFileAccessProtectorInformation
{
	/// <summary>
	/// Always creates a new instance, does not reuse a previous one.
	/// </summary>
	public static ZspSimpleFileAccessProtectorInformation Default => new();

	public static int DefaultRetryCount => 3;
	public static int DefaultSleepDelaySeconds => 2;
	public bool Use { get; set; } = true;
	public string? Info { get; set; }
	public int RetryCount { get; set; } = DefaultRetryCount;
	public int SleepDelaySeconds { get; set; } = DefaultSleepDelaySeconds;
	public bool DoGarbageCollectBeforeSleep { get; set; } = true;
	public bool GarbageCollectionWaitForPendingFinalizers { get; set; } = true;
	public bool GarbageCollectionWaitForFullGCComplete { get; set; } = true;
	public ZspHandleExceptionDelegate? HandleException { get; set; }
}