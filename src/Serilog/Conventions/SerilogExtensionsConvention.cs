using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rocket.Surgery.Conventions.Scanners;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using System.Diagnostics;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.Serilog.Conventions;
using Serilog.Core;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Rocket.Surgery.Extensions.DependencyInjection;

[assembly: Convention(typeof(SerilogExtensionsConvention))]

namespace Rocket.Surgery.Extensions.Serilog.Conventions
{
    /// <summary>
    ///  SerilogExtensionsConvention.
    /// Implements the <see cref="IServiceConvention" />
    /// </summary>
    /// <seealso cref="IServiceConvention" />
    public class SerilogExtensionsConvention : IServiceConvention
    {
        private readonly IConventionScanner _scanner;
        private readonly ILogger _diagnosticSource;
        private readonly RocketSerilogOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerilogExtensionsConvention"/> class.
        /// </summary>
        /// <param name="scanner">The scanner.</param>
        /// <param name="diagnosticSource">The diagnostic source.</param>
        /// <param name="options">The options.</param>
        public SerilogExtensionsConvention(
            IConventionScanner scanner,
            ILogger diagnosticSource,
            RocketSerilogOptions? options = null)
        {
            _scanner = scanner;
            _diagnosticSource = diagnosticSource;
            _options = options ?? new RocketSerilogOptions();
        }

        /// <inheritdoc />
        public void Register(IServiceConventionContext context)
        {
            context.Services.AddSingleton<ILoggerFactory>(_ =>
            {
                if (_options.WriteToProviders)
                {
                    var providerCollection = _.GetRequiredService<LoggerProviderCollection>();
                    var factory = new SerilogLoggerFactory(_.GetRequiredService<global::Serilog.ILogger>(), true, null);

                    foreach (var provider in _.GetServices<ILoggerProvider>())
                    {
                        factory.AddProvider(provider);
                    }

                    return factory;
                }
                else
                {
                    return new SerilogLoggerFactory(_.GetRequiredService<global::Serilog.ILogger>(), true, null);
                }
            });
            context.Services.AddHostedService<SerilogFinalizerHostedService>();

            var loggerConfiguration = context.Get<LoggerConfiguration>() ?? new LoggerConfiguration();

            if (_options.WriteToProviders)
            {
                var loggerProviderCollection = context.Get<LoggerProviderCollection>() ?? new LoggerProviderCollection();
                context.Services.AddSingleton(loggerProviderCollection);
                loggerConfiguration.WriteTo.Providers(loggerProviderCollection);
            }

            var serilogBuilder = new SerilogBuilder(
                _scanner,
                context.AssemblyProvider,
                context.AssemblyCandidateFinder,
                context.Environment,
                context.Configuration,
                loggerConfiguration,
                _diagnosticSource,
                context.Properties
            );

            var logger = serilogBuilder.Build();
            if (!_options.PreserveStaticLogger)
            {
                Log.Logger = logger;
            }

            context.Services.AddSingleton(logger);
        }
    }
}
