namespace ZetaShortPaths
{
    using JetBrains.Annotations;
    using System;
    using System.ComponentModel;

    [UsedImplicitly]
    public class ZspHandleExceptionInfo
    {
        [UsedImplicitly] public Exception Exception { get; }

        [UsedImplicitly] public int CurrentRetryCount { get; }

        public ZspHandleExceptionInfo(Exception exception, int currentRetryCount)
        {
            Exception = exception;
            CurrentRetryCount = currentRetryCount;
        }

        /// <summary>
        /// Return value. Set optionally to TRUE to force premature throwing.
        /// </summary>
        [DefaultValue(false)]
        [UsedImplicitly]
        public bool WantThrow { get; set; }
    }
}