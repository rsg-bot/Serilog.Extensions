using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rocket.Surgery.Extensions.DependencyInjection;

namespace Rocket.Surgery.Extensions.Serilog.AspNetCore.Conventions
{
    class RequestLoggingConvention : IServiceConvention
    {
        public void Register(IServiceConventionContext context)
        {
            context.Services.AddTransient<ISerilogDiagnosticListener, HostingDiagnosticListener>();
            context.OnBuild.Subscribe(new ServiceProviderObserver());
        }

        class DiagnosticListenerObserver : IObserver<DiagnosticListener>, IDisposable
        {
            private readonly List<IDisposable> _subscriptions;
            private readonly IEnumerable<ISerilogDiagnosticListener> _diagnosticListeners;

            /// <summary>
            /// Initializes a new instance of the <see cref="DiagnosticListenerObserver"/> class.
            /// </summary>
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

        class ServiceProviderObserver : IObserver<IServiceProvider>
        {
            void IObserver<IServiceProvider>.OnCompleted()
            {
            }

            void IObserver<IServiceProvider>.OnError(Exception error)
            {
            }

            void IObserver<IServiceProvider>.OnNext(IServiceProvider value)
            {
                var disposable = DiagnosticListener.AllListeners.Subscribe(
                    new DiagnosticListenerObserver(
                        value.GetRequiredService<IEnumerable<ISerilogDiagnosticListener>>()));

                value.GetRequiredService<IApplicationLifetime>().ApplicationStopped.Register(() => disposable.Dispose());
            }
        }
    }
}
