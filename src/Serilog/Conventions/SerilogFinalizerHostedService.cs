using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Rocket.Surgery.Extensions.Serilog.Conventions
{
    /// <summary>
    ///  SerilogFinalizerHostedService.
    /// Implements the <see cref="Microsoft.Extensions.Hosting.IHostedService" />
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Hosting.IHostedService" />
    class SerilogFinalizerHostedService : IHostedService
    {
#if NETSTANDARD2_0 || NETCOREAPP2_1
        private readonly IApplicationLifetime _lifetime;
#else
        private readonly IHostApplicationLifetime _lifetime;
#endif
        /// <summary>
        /// Initializes a new instance of the <see cref="SerilogFinalizerHostedService"/> class.
        /// </summary>
        /// <param name="lifetime">The lifetime.</param>
        public SerilogFinalizerHostedService(
#if NETSTANDARD2_0 || NETCOREAPP2_1
            IApplicationLifetime lifetime
#else
            IHostApplicationLifetime lifetime
#endif
        )
        {
            _lifetime = lifetime;
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
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
