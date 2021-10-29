namespace ZetaShortPaths.UnitTests.Helper
{
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