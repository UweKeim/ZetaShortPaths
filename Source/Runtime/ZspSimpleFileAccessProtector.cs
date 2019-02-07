﻿namespace ZetaShortPaths
{
    using JetBrains.Annotations;
    using Properties;
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.Threading;

    /// <summary>
    /// Execute an action. On error retry multiple times, sleep between the retries.
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public static class ZspSimpleFileAccessProtector
    {
        private const string PassThroughProtector = @"zlp-pass-through-protector";

        /// <summary>
        /// Call on an exception instance that you do NOT want to retry in this class but immediately
        /// throw it.
        /// </summary>
        [UsedImplicitly]
        public static Exception MarkAsPassThroughZspProtector(this Exception x)
        {
            if (x == null) return null;

            x.Data[PassThroughProtector] = true;

            return x;
        }

        /// <summary>
        /// Execute an action. On error retry multiple times, sleep between the retries.
        /// </summary>
        [UsedImplicitly]
        public static void Protect(
            Action action,
            ZspSimpleFileAccessProtectorInformation info = null)
        {
            info = info ?? new ZspSimpleFileAccessProtectorInformation();

            if (info.Use)
            {
                var count = 0;
                while (true)
                {
                    try
                    {
                        action?.Invoke();
                        return;
                    }
                    catch (Exception x)
                    {
                        Trace.TraceWarning($@"Error during file operation. ('{info.Info}'): {x.Message}");

                        if (count++ > info.RetryCount)
                        {
                            throw new ZspSimpleFileAccessProtectorException(
                                string.Format(
                                    info.RetryCount == 1
                                        ? Resources.TriedTooOftenSingular
                                        : Resources.TriedTooOftenPlural, info.RetryCount), x);
                        }
                        else
                        {
                            var p = new HandleExceptionInfo(x, count);
                            info.HandleException?.Invoke(p);

                            if (p.WantThrow)
                            {
                                throw new ZspSimpleFileAccessProtectorException(
                                    string.Format(
                                        info.RetryCount == 1
                                            ? Resources.TriedTooOftenSingular
                                            : Resources.TriedTooOftenPlural, info.RetryCount), x);
                            }

                            if (info.DoGarbageCollectBeforeSleep)
                            {
                                Trace.TraceInformation(
                                    $@"Error '{x}' during file operation, tried {
                                            count
                                        } times, doing a garbage collect now.");
                                DoGarbageCollect();
                            }

                            Trace.TraceInformation(
                                $@"Error '{x}' during file operation, tried {count} times, sleeping for {
                                        info
                                            .SleepDelaySeconds
                                    } seconds and retry again.");
                            Thread.Sleep(TimeSpan.FromSeconds(info.SleepDelaySeconds));
                        }
                    }
                }
            }
            else
            {
                action?.Invoke();
            }
        }

        /// <summary>
        /// Execute an action. On error retry multiple times, sleep between the retries.
        /// </summary>
        [UsedImplicitly]
        public static T Protect<T>(
            Func<T> func,
            ZspSimpleFileAccessProtectorInformation info = null)
        {
            info = info ?? new ZspSimpleFileAccessProtectorInformation();

            if (info.Use)
            {
                var count = 0;
                while (true)
                {
                    try
                    {
                        return func.Invoke();
                    }
                    catch (Exception x)
                    {
                        Trace.TraceWarning($@"Error during file operation. ('{info.Info}'): {x.Message}");

                        // Bestimmte Fehler direkt durchlassen.
                        if (x.Data[PassThroughProtector] is bool b && b) throw;

                        if (count++ > info.RetryCount)
                        {
                            throw new ZspSimpleFileAccessProtectorException(
                                string.Format(
                                    info.RetryCount == 1
                                        ? Resources.TriedTooOftenSingular
                                        : Resources.TriedTooOftenPlural, info.RetryCount), x);
                        }
                        else
                        {
                            var p = new HandleExceptionInfo(x, count);
                            info.HandleException?.Invoke(p);

                            if (p.WantThrow)
                            {
                                throw new ZspSimpleFileAccessProtectorException(
                                    string.Format(
                                        info.RetryCount == 1
                                            ? Resources.TriedTooOftenSingular
                                            : Resources.TriedTooOftenPlural, info.RetryCount), x);
                            }

                            if (info.DoGarbageCollectBeforeSleep)
                            {
                                Trace.TraceInformation(
                                    $@"Error '{x}' during file operation, tried {
                                            count
                                        } times, doing a garbage collect now.");
                                DoGarbageCollect();
                            }

                            Trace.TraceInformation(
                                $@"Error '{x}' during file operation, tried {count} times, sleeping for {
                                        info
                                            .SleepDelaySeconds
                                    } seconds and retry again.");
                            Thread.Sleep(TimeSpan.FromSeconds(info.SleepDelaySeconds));
                        }
                    }
                }
            }
            else
            {
                return func.Invoke();
            }
        }

        [UsedImplicitly]
        public static void DoGarbageCollect(bool waitForPendingFinalizers = true)
        {
            GC.Collect();

            /*
            // https://www.experts-exchange.com/questions/26638525/GC-WaitForPendingFinalizers-hangs.html
            // https://blogs.msdn.microsoft.com/tess/2008/04/21/does-interrupting-gc-waitforpendingfinalizers-interrupt-finalization/
            GC.WaitForPendingFinalizers();
            GC.Collect();
            */

            if (waitForPendingFinalizers)
            {
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

            GC.WaitForFullGCComplete(1000);
        }

        internal static int GetConfigIntOrDef(string key, int def)
        {
            var val = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(val)) return def;

            return int.TryParse(val, out var r) ? r : def;
        }
    }
}