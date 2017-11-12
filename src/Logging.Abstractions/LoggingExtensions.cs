using System;
using System.Diagnostics;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Logging
{
    /// <summary>
    /// Class LoggingExtensions.
    /// </summary>
    /// TODO Edit XML Comment Template for LoggingExtensions
    public static class LoggingExtensions
    {
        class Disposable : IDisposable
        {
            private readonly IDisposable _disposable;
            private readonly Action<long> _action;
            private readonly Stopwatch _sw;

            public Disposable(IDisposable disposable, Action<long> action)
            {
                _disposable = disposable;
                _action = action;
                _sw = new Stopwatch();
                _sw.Start();
            }

            public void Dispose()
            {
                _sw.Stop();
                _action(_sw.ElapsedMilliseconds);
                _disposable.Dispose();
            }
        }

        /// <summary>
        /// Times the trace.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>IDisposable.</returns>
        /// TODO Edit XML Comment Template for TimeTrace
        public static IDisposable TimeTrace(this ILogger logger, string message, params object[] args)
        {
            var scope = logger.BeginScope(new { });
            logger.LogTrace($"Starting: {message}", args);
            return new Disposable(scope, elapsed =>
            {
                var a = args.Concat(new object[] { elapsed }).ToArray();
                logger.LogTrace($"Finished: {message} in {{ElapsedMilliseconds}}ms", a);
            });
        }

        /// <summary>
        /// Times the debug.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>IDisposable.</returns>
        /// TODO Edit XML Comment Template for TimeDebug
        public static IDisposable TimeDebug(this ILogger logger, string message, params object[] args)
        {
            var scope = logger.BeginScope(new { });
            logger.LogDebug($"Starting: {message}", args);
            return new Disposable(scope, elapsed =>
            {
                var a = args.Concat(new object[] { elapsed }).ToArray();
                logger.LogDebug($"Finished: {message} in {{ElapsedMilliseconds}}ms", a);
            });
        }

        /// <summary>
        /// Times the information.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>IDisposable.</returns>
        /// TODO Edit XML Comment Template for TimeInformation
        public static IDisposable TimeInformation(this ILogger logger, string message, params object[] args)
        {
            var scope = logger.BeginScope(new { });
            logger.LogInformation($"Starting: {message}", args);
            return new Disposable(scope, elapsed =>
            {
                var a = args.Concat(new object[] { elapsed }).ToArray();
                logger.LogInformation($"Finished: {message} in {{ElapsedMilliseconds}}ms", a);
            });
        }
    }
}
