using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.DependencyInjection;
using Rocket.Surgery.Extensions.Logging;
using Rocket.Surgery.Extensions.Serilog.AspNetCore.Conventions;

[assembly:Convention(typeof(RequestLoggingConvention))]

namespace Rocket.Surgery.Extensions.Serilog.AspNetCore.Conventions
{
    /// <summary>
    ///  RequestLoggingConvention.
    /// Implements the <see cref="ILoggingConvention" />
    /// </summary>
    /// <seealso cref="ILoggingConvention" />
    public class RequestLoggingConvention : ILoggingConvention
    {
        /// <summary>
        /// Registers the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Register(ILoggingConventionContext context)
        {
            context.Services.AddTransient<ISerilogDiagnosticListener, HostingDiagnosticListener>();
            context.Services.AddHostedService<HostedService>();
        }

        /// <summary>
        ///  DiagnosticListenerObserver.
        /// Implements the <see cref="IObserver{DiagnosticListener}" />
        /// Implements the <see cref="IDisposable" />
        /// </summary>
        /// <seealso cref="IObserver{DiagnosticListener}" />
        /// <seealso cref="IDisposable" />
        private class DiagnosticListenerObserver : IObserver<DiagnosticListener>, IDisposable
        {
            private readonly List<IDisposable> _subscriptions;
            private readonly IEnumerable<ISerilogDiagnosticListener> _diagnosticListeners;

            /// <summary>
            /// Initializes a new instance of the <see cref="DiagnosticListenerObserver" /> class.
            /// </summary>
            /// <param name="diagnosticListeners">The diagnostic listeners.</param>
            public DiagnosticListenerObserver(
                IEnumerable<ISerilogDiagnosticListener> diagnosticListeners)
            {
                _diagnosticListeners = diagnosticListeners;
                _subscriptions = new List<IDisposable>();
            }

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
            void IObserver<DiagnosticListener>.OnError(Exception error)
            {
            }

            /// <inheritdoc />
            void IObserver<DiagnosticListener>.OnCompleted()
            {
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            /// <inheritdoc />
            public void Dispose()
            {
                Dispose(true);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (disposing)
                {
                    foreach (var subscription in _subscriptions)
                    {
                        subscription.Dispose();
                    }
                }
            }
        }

        /// <summary>
        ///  HostedService.
        /// Implements the <see cref="IHostedService" />
        /// </summary>
        /// <seealso cref="IHostedService" />
        private class HostedService : IHostedService
        {
            private readonly IEnumerable<ISerilogDiagnosticListener> _diagnosticListeners;
#if NETSTANDARD2_0 || NETCOREAPP2_1
        private readonly IApplicationLifetime _lifetime;
#else
            private readonly IHostApplicationLifetime _lifetime;
#endif

            /// <summary>
            /// Initializes a new instance of the <see cref="HostedService"/> class.
            /// </summary>
            /// <param name="diagnosticListeners">The diagnostic listeners.</param>
            /// <param name="lifetime">The lifetime.</param>
            public HostedService(
                IEnumerable<ISerilogDiagnosticListener> diagnosticListeners,
#if NETSTANDARD2_0 || NETCOREAPP2_1
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
                var disposable = DiagnosticListener.AllListeners.Subscribe(new DiagnosticListenerObserver(_diagnosticListeners));

                _lifetime.ApplicationStopped.Register(() => disposable.Dispose());
                return Task.CompletedTask;
            }

            /// <summary>
            /// Triggered when the application host is performing a graceful shutdown.
            /// </summary>
            /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
            /// <returns>Task.</returns>
            public Task StopAsync(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }
    }
}
