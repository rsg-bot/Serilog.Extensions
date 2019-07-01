using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rocket.Surgery.Conventions.Scanners;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using System.Diagnostics;
using Rocket.Surgery.Extensions.Logging;

namespace Rocket.Surgery.Extensions.Serilog.Conventions
{
    public class SerilogServiceConvention : ILoggingConvention
    {
        private readonly IConventionScanner _scanner;
        private readonly DiagnosticSource _diagnosticSource;
        private readonly SerilogOptions _options;

        public SerilogServiceConvention(
            IConventionScanner scanner,
            DiagnosticSource diagnosticSource,
            SerilogOptions options)
        {
            this._scanner = scanner;
            this._diagnosticSource = diagnosticSource;
            this._options = options;
        }

        public void Register(ILoggingConventionContext context)
        {
            var serilogBuilder = new SerilogBuilder(
                _scanner,
                context.AssemblyProvider,
                context.AssemblyCandidateFinder,
                context.Environment,
                context.Configuration,
                context,
                _options.LoggingLevelSwitch,
                _options.LoggerConfiguration,
                _diagnosticSource,
                context.Properties
            );

            var logLevel = _options.GetLogLevel(serilogBuilder);

            _options.LoggingLevelSwitch.MinimumLevel = GetLogEventLevel(logLevel);
            context.Services.AddSingleton(_options.LoggingLevelSwitch);
            context.Services.AddSingleton<ILoggerProvider>(new SerilogLoggerProvider(serilogBuilder.Build(), true));
        }

        private static LogEventLevel GetLogEventLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
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
                case LogLevel.None:
                    return LogEventLevel.Fatal;
                default:
                    return LogEventLevel.Information;
            }
        }
    }
}
