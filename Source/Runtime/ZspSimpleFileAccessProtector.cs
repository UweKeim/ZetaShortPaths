namespace ZetaShortPaths;

using Properties;
using System.Threading.Tasks;

/// <summary>
/// Execute an action. On error retry multiple times, sleep between the retries.
/// </summary>
// ReSharper disable once UnusedMember.Global
[PublicAPI]
public static class ZspSimpleFileAccessProtector
{
	private const string PassThroughProtector = @"zsp-pass-through-protector";

	/// <summary>
	/// Marks an exception as pass-through, preventing retries in this class.
	/// </summary>
	/// <param name="x">The exception to mark.</param>
	/// <returns>The same exception instance, marked for pass-through.</returns>
	public static Exception? MarkAsPassThroughZspProtector(this Exception? x)
	{
		if (x == null) return null;

		x.Data[PassThroughProtector] = true;

		return x;
	}

	/// <summary>
	/// Executes an action with automatic retries in case of an exception.
	/// </summary>
	/// <param name="action">The action to execute.</param>
	/// <param name="info">Optional configuration parameters for retry behavior.</param>
	/// <exception cref="ZspSimpleFileAccessProtectorException">
	/// Thrown when the maximum number of retries is reached.
	/// </exception>
	/// <exception cref="OperationCanceledException">
	/// Thrown immediately if the operation is canceled.
	/// </exception>
	public static void Protect(
		Action? action,
		ZspSimpleFileAccessProtectorInformation? info = null)
	{
		info ??= new();

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
#if WANT_TRACE
                    Trace.TraceWarning($@"Error during file operation. ('{info.Info}'): {x.Message}");
#endif

					// Bestimmte Fehler direkt durchlassen.
					if (x.Data[PassThroughProtector] is true) throw;
					if (x is OperationCanceledException) throw;

					if (count++ > info.RetryCount)
					{
						var p = new ZspHandleExceptionInfo(x, count, info);
						info.HandleException?.Invoke(p);

						//if (p.WantThrow)
						{
							throw new ZspSimpleFileAccessProtectorException(
									string.Format(
										info.RetryCount == 1
											? Resources.TriedTooOftenSingular
											: Resources.TriedTooOftenPlural, info.RetryCount), p.Exception)
								{ Data = { { nameof(info.Info), info.Info } } };
						}
					}
					else
					{
						var p = new ZspHandleExceptionInfo(x, count, info);
						info.HandleException?.Invoke(p);

						if (p.WantThrow)
						{
							throw new ZspSimpleFileAccessProtectorException(
									string.Format(
										info.RetryCount == 1
											? Resources.TriedTooOftenSingular
											: Resources.TriedTooOftenPlural, info.RetryCount), p.Exception)
								{ Data = { { nameof(info.Info), info.Info } } };
						}

						if (info.DoGarbageCollectBeforeSleep)
						{
#if WANT_TRACE
                            Trace.TraceInformation(
                                $@"Error '{x}' during file operation, tried {count} times, doing a garbage collect now.");
#endif
							DoGarbageCollect(
								info.GarbageCollectionWaitForPendingFinalizers,
								info.GarbageCollectionWaitForFullGCComplete);
						}

#if WANT_TRACE
                        Trace.TraceInformation(
                            $@"Error '{x}' during file operation, tried {count} times, sleeping for {info.SleepDelaySeconds} seconds and retry again.");
#endif
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
	/// Executes a function with automatic retries in case of an exception and returns a result.
	/// </summary>
	/// <typeparam name="T">The type of the result returned by the function.</typeparam>
	/// <param name="func">The function to execute.</param>
	/// <param name="info">Optional configuration parameters for retry behavior.</param>
	/// <returns>The result of the function execution.</returns>
	/// <exception cref="ZspSimpleFileAccessProtectorException">
	/// Thrown when the maximum number of retries is reached.
	/// </exception>
	/// <exception cref="OperationCanceledException">
	/// Thrown immediately if the operation is canceled.
	/// </exception>
	public static T Protect<T>(
		Func<T> func,
		ZspSimpleFileAccessProtectorInformation? info = null)
	{
		info ??= new();

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
#if WANT_TRACE
                    Trace.TraceWarning($@"Error during file operation. ('{info.Info}'): {x.Message}");
#endif

					// Bestimmte Fehler direkt durchlassen.
					if (x.Data[PassThroughProtector] is true) throw;
					if (x is OperationCanceledException) throw;

					if (count++ > info.RetryCount)
					{
						var p = new ZspHandleExceptionInfo(x, count, info);
						info.HandleException?.Invoke(p);

						//if (p.WantThrow)
						{
							throw new ZspSimpleFileAccessProtectorException(
									string.Format(
										info.RetryCount == 1
											? Resources.TriedTooOftenSingular
											: Resources.TriedTooOftenPlural, info.RetryCount), p.Exception)
								{ Data = { { nameof(info.Info), info.Info } } };
						}
					}
					else
					{
						var p = new ZspHandleExceptionInfo(x, count, info);
						info.HandleException?.Invoke(p);

						if (p.WantThrow)
						{
							throw new ZspSimpleFileAccessProtectorException(
									string.Format(
										info.RetryCount == 1
											? Resources.TriedTooOftenSingular
											: Resources.TriedTooOftenPlural, info.RetryCount), p.Exception)
								{ Data = { { nameof(info.Info), info.Info } } };
						}

						if (info.DoGarbageCollectBeforeSleep)
						{
#if WANT_TRACE
                            Trace.TraceInformation(
                                $@"Error '{x}' during file operation, tried {count} times, doing a garbage collect now.");
#endif
							DoGarbageCollect(
								info.GarbageCollectionWaitForPendingFinalizers,
								info.GarbageCollectionWaitForFullGCComplete);
						}

#if WANT_TRACE
                        Trace.TraceInformation(
                            $@"Error '{x}' during file operation, tried {count} times, sleeping for {info.SleepDelaySeconds} seconds and retry again.");
#endif
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

	/// <summary>
	/// Executes an asynchronous action with automatic retries in case of an exception.
	/// </summary>
	/// <param name="action">The asynchronous action to execute.</param>
	/// <param name="info">Optional configuration parameters for retry behavior.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ZspSimpleFileAccessProtectorException">
	/// Thrown when the maximum number of retries is reached.
	/// </exception>
	/// <exception cref="OperationCanceledException">
	/// Thrown immediately if the operation is canceled.
	/// </exception>
	public static async Task ProtectAsync(
		Func<Task> action,
		ZspSimpleFileAccessProtectorInformation? info = null,
		CancellationToken cancellationToken = default)
	{
		info ??= new();

		if (info.Use)
		{
			var count = 0;
			while (true)
			{
				try
				{
					await action.Invoke();
					return;
				}
				catch (Exception x)
				{
#if WANT_TRACE
					Trace.TraceWarning($@"Error during file operation. ('{info.Info}'): {x.Message}");
#endif

					// Bestimmte Fehler direkt durchlassen.
					if (x.Data[PassThroughProtector] is true) throw;
					if (x is OperationCanceledException) throw;

					if (count++ > info.RetryCount)
					{
						var p = new ZspHandleExceptionInfo(x, count, info);
						info.HandleException?.Invoke(p);

						throw new ZspSimpleFileAccessProtectorException(
								string.Format(
									info.RetryCount == 1
										? Resources.TriedTooOftenSingular
										: Resources.TriedTooOftenPlural, info.RetryCount), p.Exception)
							{ Data = { { nameof(info.Info), info.Info } } };
					}
					else
					{
						var p = new ZspHandleExceptionInfo(x, count, info);
						info.HandleException?.Invoke(p);

						if (p.WantThrow)
						{
							throw new ZspSimpleFileAccessProtectorException(
									string.Format(
										info.RetryCount == 1
											? Resources.TriedTooOftenSingular
											: Resources.TriedTooOftenPlural, info.RetryCount), p.Exception)
								{ Data = { { nameof(info.Info), info.Info } } };
						}

						if (info.DoGarbageCollectBeforeSleep)
						{
#if WANT_TRACE
	                        Trace.TraceInformation(
	                            $@"Error '{x}' during file operation, tried {count} times, doing a garbage collect now.");
#endif
							DoGarbageCollect(
								info.GarbageCollectionWaitForPendingFinalizers,
								info.GarbageCollectionWaitForFullGCComplete);
						}

#if WANT_TRACE
	                    Trace.TraceInformation(
	                        $@"Error '{x}' during file operation, tried {count} times, sleeping for {info.SleepDelaySeconds} seconds and retry again.");
#endif
						await Task.Delay(TimeSpan.FromSeconds(info.SleepDelaySeconds), cancellationToken);
					}
				}
			}
		}
		else
		{
			await action.Invoke();
		}
	}

	/// <summary>
	/// Executes an asynchronous function with automatic retries in case of an exception and returns a result.
	/// </summary>
	/// <typeparam name="T">The type of the result returned by the function.</typeparam>
	/// <param name="func">The asynchronous function to execute.</param>
	/// <param name="info">Optional configuration parameters for retry behavior.</param>
	/// <param name="cancellationToken"></param>
	/// <returns>
	/// A task representing the asynchronous operation, which resolves to the function's result.
	/// </returns>
	/// <exception cref="ZspSimpleFileAccessProtectorException">
	/// Thrown when the maximum number of retries is reached.
	/// </exception>
	/// <exception cref="OperationCanceledException">
	/// Thrown immediately if the operation is canceled.
	/// </exception>
	public static async Task<T> ProtectAsync<T>(
		Func<Task<T>> func,
		ZspSimpleFileAccessProtectorInformation? info = null,
		CancellationToken cancellationToken = default)
	{
		info ??= new();

		if (info.Use)
		{
			var count = 0;
			while (true)
			{
				try
				{
					return await func.Invoke();
				}
				catch (Exception x)
				{
#if WANT_TRACE
					Trace.TraceWarning($@"Error during file operation. ('{info.Info}'): {x.Message}");
#endif

					// Bestimmte Fehler direkt durchlassen.
					if (x.Data[PassThroughProtector] is true) throw;
					if (x is OperationCanceledException) throw;

					if (count++ > info.RetryCount)
					{
						var p = new ZspHandleExceptionInfo(x, count, info);
						info.HandleException?.Invoke(p);

						throw new ZspSimpleFileAccessProtectorException(
								string.Format(
									info.RetryCount == 1
										? Resources.TriedTooOftenSingular
										: Resources.TriedTooOftenPlural, info.RetryCount), p.Exception)
							{ Data = { { nameof(info.Info), info.Info } } };
					}
					else
					{
						var p = new ZspHandleExceptionInfo(x, count, info);
						info.HandleException?.Invoke(p);

						if (p.WantThrow)
						{
							throw new ZspSimpleFileAccessProtectorException(
									string.Format(
										info.RetryCount == 1
											? Resources.TriedTooOftenSingular
											: Resources.TriedTooOftenPlural, info.RetryCount), p.Exception)
								{ Data = { { nameof(info.Info), info.Info } } };
						}

						if (info.DoGarbageCollectBeforeSleep)
						{
#if WANT_TRACE
	                        Trace.TraceInformation(
	                            $@"Error '{x}' during file operation, tried {count} times, doing a garbage collect now.");
#endif
							DoGarbageCollect(
								info.GarbageCollectionWaitForPendingFinalizers,
								info.GarbageCollectionWaitForFullGCComplete);
						}

#if WANT_TRACE
	                    Trace.TraceInformation(
	                        $@"Error '{x}' during file operation, tried {count} times, sleeping for {
	                            info.SleepDelaySeconds} seconds and retry again.");
#endif
						await Task.Delay(TimeSpan.FromSeconds(info.SleepDelaySeconds), cancellationToken);
					}
				}
			}
		}
		else
		{
			return await func.Invoke();
		}
	}

	/// <summary>
	/// Performs garbage collection and optionally waits for pending finalizers or full GC completion.
	/// </summary>
	/// <param name="waitForPendingFinalizers">Specifies whether to wait for pending finalizers.</param>
	/// <param name="waitForFullGCComplete">Specifies whether to wait for full garbage collection completion.</param>
	public static void DoGarbageCollect(
		bool waitForPendingFinalizers = true,
		bool waitForFullGCComplete = true)
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

		if (waitForFullGCComplete)
		{
			GC.WaitForFullGCComplete(1000);
		}
	}
}