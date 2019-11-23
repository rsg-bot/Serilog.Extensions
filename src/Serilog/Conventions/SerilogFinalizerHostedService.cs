using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Rocket.Surgery.Extensions.Serilog.Conventions
{
    /// <summary>
    /// SerilogFinalizerHostedService.
    /// Implements the <see cref="IHostedService" />
    /// </summary>
    /// <seealso cref="IHostedService" />
    internal class SerilogFinalizerHostedService : IHostedService
    {
#if NETSTANDARD2_0 || NETCOREAPP2_1
        private readonly IApplicationLifetime _lifetime;
#else
        private readonly IHostApplicationLifetime _lifetime;
#endif
        /// <summary>
        /// Initializes a new instance of the <see cref="SerilogFinalizerHostedService" /> class.
        /// </summary>
        /// <param name="lifetime">The lifetime.</param>
        public SerilogFinalizerHostedService(
#if NETSTANDARD2_0 || NETCOREAPP2_1
            [NotNull] IApplicationLifetime lifetime
#else
            [NotNull] IHostApplicationLifetime lifetime
#endif
        )
        {
            _lifetime = lifetime ?? throw new ArgumentNullException(nameof(lifetime));
        }

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>Task.</returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _lifetime.ApplicationStopped.Register(Log.CloseAndFlush);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        /// <returns>Task.</returns>
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}