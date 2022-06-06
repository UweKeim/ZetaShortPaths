namespace ZetaShortPaths;

public class ZspSimpleFileAccessProtectorInformation
{
    [PublicAPI]
    public static ZspSimpleFileAccessProtectorInformation Default => new();

    [PublicAPI]
    public static int DefaultRetryCount =>
        ZspSimpleFileAccessProtector.GetConfigIntOrDef(@"zsp.sfap.retryCount", 3);

    [PublicAPI]
    public static int DefaultSleepDelaySeconds =>
        ZspSimpleFileAccessProtector.GetConfigIntOrDef(@"zsp.sfap.sleepDelaySeconds", 2);

    [PublicAPI] public bool Use { get; set; } = true;

    [PublicAPI] public string? Info { get; set; }

    [PublicAPI] public int RetryCount { get; set; } = DefaultRetryCount;

    [PublicAPI] public int SleepDelaySeconds { get; set; } = DefaultSleepDelaySeconds;

    [PublicAPI] public bool DoGarbageCollectBeforeSleep { get; set; } = true;

    [PublicAPI] public ZspHandleExceptionDelegate? HandleException { get; set; }
}