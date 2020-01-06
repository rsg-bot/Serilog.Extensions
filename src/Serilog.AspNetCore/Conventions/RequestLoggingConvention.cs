using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rocket.Surgery.AspNetCore.Serilog.Conventions;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.Logging;
using Rocket.Surgery.Extensions.Serilog;

[assembly: Convention(typeof(RequestLoggingConvention))]

namespace Rocket.Surgery.AspNetCore.Serilog.Conventions
{
    /// <summary>
    /// RequestLoggingConvention.
    /// Implements the <see cref="ILoggingConvention" />
    /// </summary>
    /// <seealso cref="ILoggingConvention" />
    public class RequestLoggingConvention : ILoggingConvention
    {
        /// <summary>
        /// Registers the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Register([NotNull] ILoggingConventionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.Services.AddTransient<ISerilogDiagnosticListener, HostingDiagnosticListener>();
            context.Services.AddHostedService<HostedService>();
        }

        /// <summary>
        /// DiagnosticListenerObserver.
        /// Implements the <see cref="IObserver{T}" />
        /// Implements the <see cref="IDisposable" />
        /// </summary>
        /// <seealso cref="IObserver{DiagnosticListener}" />
        /// <seealso cref="IDisposable" />
        private sealed class DiagnosticListenerObserver : IObserver<DiagnosticListener>, IDisposable
        {
            private readonly List<IDisposable> _subscriptions;
            private readonly IEnumerable<ISerilogDiagnosticListener> _diagnosticListeners;

            /// <summary>
            /// Initializes a new instance of the <see cref="DiagnosticListenerObserver" /> class.
            /// </summary>
            /// <param name="diagnosticListeners">The diagnostic listeners.</param>
            public DiagnosticListenerObserver(
                IEnumerable<ISerilogDiagnosticListener> diagnosticListeners
            )
            {
                _diagnosticListeners = diagnosticListeners;
                _subscriptions = new List<IDisposable>();
            }

            private void Dispose(bool disposing)
            {
                if (disposing)
                {
                    foreach (var subscription in _subscriptions)
                    {
                        subscription.Dispose();
                    }
                }
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            /// <inheritdoc />
            public void Dispose() => Dispose(true);

            /// <inheritdoc />
            void IObserver<DiagnosticListener>.OnNext(DiagnosticListener value)
            {
                foreach (var applicationInsightDiagnosticListener in _diagnosticListeners)
                {
                    if (applicationInsightDiagnosticListener.ListenerName == value.Name)
                    {
                        _subscriptions.Add(value.SubscribeWithAdapter(applicationInsightDiagnosticListener));
                    }
                }
            }

            /// <inheritdoc />
            void IObserver<DiagnosticListener>.OnError(Exception error) { }

            /// <inheritdoc />
            void IObserver<DiagnosticListener>.OnCompleted() { }
        }

        /// <summary>
        /// HostedService.
        /// Implements the <see cref="IHostedService" />
        /// </summary>
        /// <seealso cref="IHostedService" />
        private class HostedService : IHostedService
        {
            private readonly IEnumerable<ISerilogDiagnosticListener> _diagnosticListeners;
#if NETSTANDARD2_0
        private readonly IApplicationLifetime _lifetime;
#else
            private readonly IHostApplicationLifetime _lifetime;
#endif
            private readonly List<IDisposable> _disposables = new List<IDisposable>();

            /// <summary>
            /// Initializes a new instance of the <see cref="HostedService" /> class.
            /// </summary>
            /// <param name="diagnosticListeners">The diagnostic listeners.</param>
            /// <param name="lifetime">The lifetime.</param>
            public HostedService(
                IEnumerable<ISerilogDiagnosticListener> diagnosticListeners,
#if NETSTANDARD2_0
                IApplicationLifetime lifetime
#else
                IHostApplicationLifetime lifetime
#endif
            )
            {
                _diagnosticListeners = diagnosticListeners;
                _lifetime = lifetime;
            }

            /// <summary>
            /// Triggered when the application host is ready to start the service.
            /// </summary>
            /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
            /// <returns>Task.</returns>
            public Task StartAsync(CancellationToken cancellationToken)
            {
                var listener = new DiagnosticListenerObserver(_diagnosticListeners);
                var disposable = DiagnosticListener.AllListeners.Subscribe(listener);
                _disposables.Add(listener);
                _disposables.Add(disposable);
                return Task.CompletedTask;
            }

            /// <summary>
            /// Triggered when the application host is performing a graceful shutdown.
            /// </summary>
            /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
            /// <returns>Task.</returns>
            public Task StopAsync(CancellationToken cancellationToken)
            {
                foreach (var item in _disposables)
                {
                    item.Dispose();
                }

                return Task.CompletedTask;
            }
        }
    }
}