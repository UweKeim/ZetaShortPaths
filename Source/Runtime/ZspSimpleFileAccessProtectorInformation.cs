namespace ZetaShortPaths
{
    using JetBrains.Annotations;

    public class ZspSimpleFileAccessProtectorInformation
    {
        [UsedImplicitly]
        public static int DefaultRetryCount =>
            ZspSimpleFileAccessProtector.GetConfigIntOrDef(@"zlp.sfap.retryCount", 3);

        [UsedImplicitly]
        public static int DefaultSleepDelaySeconds =>
            ZspSimpleFileAccessProtector.GetConfigIntOrDef(@"zlp.sfap.sleepDelaySeconds", 2);

        [UsedImplicitly] public bool Use { get; set; } = true;

        [UsedImplicitly] public string Info { get; set; }

        [UsedImplicitly] public int RetryCount { get; set; } = DefaultRetryCount;

        [UsedImplicitly] public int SleepDelaySeconds { get; set; } = DefaultSleepDelaySeconds;

        [UsedImplicitly] public bool DoGarbageCollectBeforeSleep { get; set; } = true;

        [UsedImplicitly] public HandleExceptionDelegate HandleException { get; set; }
    }
}