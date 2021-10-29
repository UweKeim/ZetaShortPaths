namespace ZetaShortPaths;

public class ZspSimpleFileAccessProtectorInformation
{
    [UsedImplicitly]
    public static ZspSimpleFileAccessProtectorInformation Default => new();

    [UsedImplicitly]
    public static int DefaultRetryCount =>
        ZspSimpleFileAccessProtector.GetConfigIntOrDef(@"zsp.sfap.retryCount", 3);

    [UsedImplicitly]
    public static int DefaultSleepDelaySeconds =>
        ZspSimpleFileAccessProtector.GetConfigIntOrDef(@"zsp.sfap.sleepDelaySeconds", 2);

    [UsedImplicitly] public bool Use { get; set; } = true;

    [UsedImplicitly] public string Info { get; set; }

    [UsedImplicitly] public int RetryCount { get; set; } = DefaultRetryCount;

    [UsedImplicitly] public int SleepDelaySeconds { get; set; } = DefaultSleepDelaySeconds;

    [UsedImplicitly] public bool DoGarbageCollectBeforeSleep { get; set; } = true;

    [UsedImplicitly] public ZspHandleExceptionDelegate HandleException { get; set; }
}