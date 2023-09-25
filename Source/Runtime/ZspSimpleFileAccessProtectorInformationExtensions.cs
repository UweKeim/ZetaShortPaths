namespace ZetaShortPaths;

[PublicAPI]
public static class ZspSimpleFileAccessProtectorInformationExtensions
{
    public static ZspSimpleFileAccessProtectorInformation SetUse(
        this ZspSimpleFileAccessProtectorInformation @this,
        bool use)
    {
        @this.Use = use;
        return @this;
    }

    public static ZspSimpleFileAccessProtectorInformation SetInfo(
        this ZspSimpleFileAccessProtectorInformation @this,
        string info)
    {
        @this.Info = info;
        return @this;
    }

    public static ZspSimpleFileAccessProtectorInformation SetRetryCount(
        this ZspSimpleFileAccessProtectorInformation @this,
        int retryCount)
    {
        @this.RetryCount = retryCount;
        return @this;
    }

    public static ZspSimpleFileAccessProtectorInformation SetSleepDelaySeconds(
        this ZspSimpleFileAccessProtectorInformation @this,
        int sleepDelaySeconds)
    {
        @this.SleepDelaySeconds = sleepDelaySeconds;
        return @this;
    }

    public static ZspSimpleFileAccessProtectorInformation DoGarbageCollectBeforeSleep(
        this ZspSimpleFileAccessProtectorInformation @this,
        bool doGarbageCollectBeforeSleep)
    {
        @this.DoGarbageCollectBeforeSleep = doGarbageCollectBeforeSleep;
        return @this;
    }

    public static ZspSimpleFileAccessProtectorInformation SetHandleException(
        this ZspSimpleFileAccessProtectorInformation @this,
        ZspHandleExceptionDelegate handleException)
    {
        @this.HandleException = handleException;
        return @this;
    }
}