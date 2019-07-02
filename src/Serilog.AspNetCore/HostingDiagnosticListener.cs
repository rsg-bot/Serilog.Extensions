using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DiagnosticAdapter;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Rocket.Surgery.Extensions.Serilog.AspNetCore
{
    /// <summary>
    /// <see cref="IDiagnosticListener" /> implementation that listens for events specific to AspNetCore hosting layer.
    /// Implements the <see cref="Rocket.Surgery.Extensions.Serilog.ISerilogDiagnosticListener" />
    /// </summary>
    /// <seealso cref="Rocket.Surgery.Extensions.Serilog.ISerilogDiagnosticListener" />
    internal class HostingDiagnosticListener : ISerilogDiagnosticListener
    {
        private static readonly AsyncLocal<Queue<IDisposable>> HostingDisposable = new AsyncLocal<Queue<IDisposable>>();

        static Queue<IDisposable> GetOrCreateHostingQueue()
        {
            var enrichers = HostingDisposable.Value;
            if (enrichers == null)
            {
                enrichers = HostingDisposable.Value = new Queue<IDisposable>();
            }
            return enrichers;
        }

        private static void QueueValue(Queue<IDisposable> queue, string name, object value)
        {
            queue.Enqueue(LogContext.PushProperty(name, value));
        }

        /// <summary>
        /// Gets a value indicating which listener this instance should be subscribed to
        /// </summary>
        /// <value>The name of the listener.</value>
        /// <inheritdoc />
        public string ListenerName { get; } = "Microsoft.AspNetCore";

        /// <summary>
        /// Diagnostic event handler method for 'Microsoft.AspNetCore.Hosting.HttpRequestIn' event.
        /// </summary>
        [DiagnosticName("Microsoft.AspNetCore.Hosting.HttpRequestIn")]
        public void OnHttpRequestIn()
        {
            // do nothing, just enable the diagnotic source
        }

        /// <summary>
        /// Diagnostic event handler method for 'Microsoft.AspNetCore.Hosting.HttpRequestIn.Start' event.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        [DiagnosticName("Microsoft.AspNetCore.Hosting.HttpRequestIn.Start")]
        public void OnHttpRequestInStart(HttpContext httpContext)
        {
            if (Activity.Current == null)
            {
                return;
            }

            var currentActivity = Activity.Current;
            var queue = GetOrCreateHostingQueue();

            QueueValue(queue, RequestResponseHeaders.RequestIdHeader, currentActivity.Id);
            QueueValue(queue, "Parent-Id", currentActivity.ParentId);
            QueueValue(queue, "Root-Id", currentActivity.RootId);

            if (httpContext.Request.Headers.TryGetValue(RequestResponseHeaders.StandardRootIdHeader, out var xmsRequestRootId))
            {
                QueueValue(queue, RequestResponseHeaders.StandardRootIdHeader, xmsRequestRootId.ToString());
            }

            if (httpContext.Request.Headers.TryGetValue(RequestResponseHeaders.StandardParentIdHeader, out var xmsRequestParentId))
            {
                QueueValue(queue, RequestResponseHeaders.StandardParentIdHeader, xmsRequestParentId.ToString());
            }
        }

        /// <summary>
        /// Diagnostic event handler method for 'Microsoft.AspNetCore.Hosting.HttpRequestIn.Stop' event.
        /// </summary>
        [DiagnosticName("Microsoft.AspNetCore.Hosting.HttpRequestIn.Stop")]
        public void OnHttpRequestInStop()
        {
            var list = GetOrCreateHostingQueue();
            while (list.Count > 0)
            {
                var item = list.Dequeue();
                item.Dispose();
            }
        }
    }
}
