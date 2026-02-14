namespace ZetaShortPaths.UnitTests;

using System.Threading.Tasks;

[TestClass]
public sealed class SimpleFileAccessProtectorTest
{
    [TestMethod]
    public void Protect_UsesConfiguredRetryCount()
    {
        var attempts = 0;
        var info = new ZspSimpleFileAccessProtectorInformation
        {
            RetryCount = 1,
            SleepDelaySeconds = 0,
            DoGarbageCollectBeforeSleep = false
        };

        Assert.Throws<ZspSimpleFileAccessProtectorException>(() =>
            ZspSimpleFileAccessProtector.Protect(() =>
            {
                attempts++;
                throw new IOException("expected");
            }, info));

        Assert.AreEqual(2, attempts);
    }

    [TestMethod]
    public void ProtectGeneric_UsesConfiguredRetryCount()
    {
        var attempts = 0;
        var info = new ZspSimpleFileAccessProtectorInformation
        {
            RetryCount = 1,
            SleepDelaySeconds = 0,
            DoGarbageCollectBeforeSleep = false
        };

        Assert.Throws<ZspSimpleFileAccessProtectorException>(() =>
            ZspSimpleFileAccessProtector.Protect<int>(() =>
            {
                attempts++;
                throw new IOException("expected");
            }, info));

        Assert.AreEqual(2, attempts);
    }

    [TestMethod]
    public async Task ProtectAsync_UsesConfiguredRetryCount()
    {
        var attempts = 0;
        var info = new ZspSimpleFileAccessProtectorInformation
        {
            RetryCount = 1,
            SleepDelaySeconds = 0,
            DoGarbageCollectBeforeSleep = false
        };

        await Assert.ThrowsAsync<ZspSimpleFileAccessProtectorException>(async () =>
            await ZspSimpleFileAccessProtector.ProtectAsync(async () =>
            {
                attempts++;
                await Task.Yield();
                throw new IOException("expected");
            }, info));

        Assert.AreEqual(2, attempts);
    }

    [TestMethod]
    public void Protect_PassThroughExceptionIsNotRetried()
    {
        var attempts = 0;
        var info = new ZspSimpleFileAccessProtectorInformation
        {
            RetryCount = 5,
            SleepDelaySeconds = 0,
            DoGarbageCollectBeforeSleep = false
        };

        Assert.Throws<InvalidOperationException>(() =>
            ZspSimpleFileAccessProtector.Protect(() =>
            {
                attempts++;
                throw new InvalidOperationException("pass-through").MarkAsPassThroughZspProtector()!;
            }, info));

        Assert.AreEqual(1, attempts);
    }
}
