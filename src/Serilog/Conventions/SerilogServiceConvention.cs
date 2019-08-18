using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rocket.Surgery.Conventions.Scanners;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using System.Diagnostics;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.Logging;
using Rocket.Surgery.Extensions.Serilog.Conventions;
using Serilog.Core;
using ILogger = Microsoft.Extensions.Logging.ILogger;

[assembly: Convention(typeof(SerilogServiceConvention))]

namespace Rocket.Surgery.Extensions.Serilog.Conventions
{
    /// <summary>
    ///  SerilogServiceConvention.
    /// Implements the <see cref="ILoggingConvention" />
    /// </summary>
    /// <seealso cref="ILoggingConvention" />
    public class SerilogServiceConvention : ILoggingConvention
    {
        private readonly IConventionScanner _scanner;
        private readonly ILogger _diagnosticSource;
        private readonly RocketSerilogOptions _serilogOptions;
        private readonly RocketLoggingOptions _loggingOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerilogServiceConvention"/> class.
        /// </summary>
        /// <param name="scanner">The scanner.</param>
        /// <param name="diagnosticSource">The diagnostic source.</param>
        /// <param name="serilogOptions">The serilog options.</param>
        /// <param name="loggingOptions">The logging options.</param>
        public SerilogServiceConvention(
            IConventionScanner scanner,
            ILogger diagnosticSource,
            RocketSerilogOptions? serilogOptions = null,
            RocketLoggingOptions? loggingOptions = null)
        {
            _scanner = scanner;
            _diagnosticSource = diagnosticSource;
            _serilogOptions = serilogOptions ?? new RocketSerilogOptions();
            _loggingOptions = loggingOptions ?? new RocketLoggingOptions();
        }

        /// <summary>
        /// Registers the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Register(ILoggingConventionContext context)
        {
            var loggingLevelSwitch = context.Get<LoggingLevelSwitch>() ?? new LoggingLevelSwitch();
            var loggerConfiguration = context.Get<LoggerConfiguration>() ?? new LoggerConfiguration();

            var serilogBuilder = new SerilogBuilder(
                _scanner,
                context.AssemblyProvider,
                context.AssemblyCandidateFinder,
                context.Environment,
                context.Configuration,
                context,
                loggingLevelSwitch,
                loggerConfiguration,
                _diagnosticSource,
                context.Properties
            );

            loggingLevelSwitch.MinimumLevel = _serilogOptions.GetLogLevel?.Invoke(serilogBuilder)
                                                              ?? GetLogEventLevel(_loggingOptions.GetLogLevel(context));
            var logger = serilogBuilder.Build();
            Log.Logger = logger;

            context.Services.AddSingleton<ILoggerProvider>(new SerilogLoggerProvider(logger, true));
            context.Services.AddSingleton(loggingLevelSwitch);
            context.Services.AddSingleton(logger);
            context.Services.AddHostedService<SerilogFinalizerHostedService>();
        }

        private static LogEventLevel GetLogEventLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.None:
                    return LogEventLevel.Verbose;
                case LogLevel.Debug:
                    return LogEventLevel.Debug;
                case LogLevel.Information:
                    return LogEventLevel.Information;
                case LogLevel.Warning:
                    return LogEventLevel.Warning;
                case LogLevel.Error:
                    return LogEventLevel.Error;
                case LogLevel.Critical:
                    return LogEventLevel.Fatal;
                default:
                    return LogEventLevel.Information;
            }
        }
    }
}
