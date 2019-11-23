using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DiagnosticAdapter;
using Rocket.Surgery.Extensions.Serilog;
using Serilog.Context;

namespace Rocket.Surgery.AspNetCore.Serilog
{
    /// <summary>
    /// <see cref="ISerilogDiagnosticListener" /> implementation that listens for events specific to AspNetCore hosting layer.
    /// Implements the <see cref="ISerilogDiagnosticListener" />
    /// </summary>
    /// <seealso cref="ISerilogDiagnosticListener" />
    internal class HostingDiagnosticListener : ISerilogDiagnosticListener
    {
        private static readonly AsyncLocal<Queue<IDisposable>> HostingDisposable = new AsyncLocal<Queue<IDisposable>>();

        private static Queue<IDisposable> GetOrCreateHostingQueue()
        {
            var enrichers = HostingDisposable.Value ?? ( HostingDisposable.Value = new Queue<IDisposable>() );
            return enrichers;
        }

        private static void QueueValue(Queue<IDisposable> queue, string name, object value)
            => queue.Enqueue(LogContext.PushProperty(name, value));

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

            if (!string.IsNullOrEmpty(currentActivity.Id))
            {
                QueueValue(queue, RequestResponseHeaders.RequestIdHeader, currentActivity.Id);
            }

            if (!string.IsNullOrEmpty(currentActivity.ParentId))
            {
                QueueValue(queue, "Parent-Id", currentActivity.ParentId);
            }

            if (!string.IsNullOrEmpty(currentActivity.RootId))
            {
                QueueValue(queue, "Root-Id", currentActivity.RootId);
            }

            if (httpContext.Request.Headers.TryGetValue(RequestResponseHeaders.CorrelationContextHeader, out var value))
            {
                QueueValue(queue, RequestResponseHeaders.CorrelationContextHeader, value.ToArray());
            }

            if (httpContext.Request.Headers.TryGetValue(
                RequestResponseHeaders.StandardRootIdHeader,
                out var xmsRequestRootId
            ))
            {
                QueueValue(queue, RequestResponseHeaders.StandardRootIdHeader, xmsRequestRootId.ToString());
            }

            if (httpContext.Request.Headers.TryGetValue(
                RequestResponseHeaders.StandardParentIdHeader,
                out var xmsRequestParentId
            ))
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

        /// <summary>
        /// Gets a value indicating which listener this instance should be subscribed to
        /// </summary>
        /// <value>The name of the listener.</value>
        /// <inheritdoc />
        public string ListenerName { get; } = "Microsoft.AspNetCore";
    }
}