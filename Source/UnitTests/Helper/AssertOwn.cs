namespace ZetaShortPaths.UnitTests.Helper
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    public static class AssertOwn
    {
        public static void DoesNotThrow(Action a)
        {
            try
            {
                a?.Invoke();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}
