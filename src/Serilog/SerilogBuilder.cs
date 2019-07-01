using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Conventions.Reflection;
using Rocket.Surgery.Conventions.Scanners;
using Serilog;
using Serilog.Core;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Rocket.Surgery.Extensions.Serilog
{
    public class SerilogBuilder : ConventionBuilder<ISerilogBuilder, ISerilogConvention, SerilogConventionDelegate>, ISerilogBuilder, ISerilogConventionContext
    {
        public SerilogBuilder(
            IConventionScanner scanner,
            IAssemblyProvider assemblyProvider,
            IAssemblyCandidateFinder assemblyCandidateFinder,
            IRocketEnvironment environment,
            IConfiguration configuration,
            ILoggingBuilder loggingBuilder,
            LoggingLevelSwitch @switch,
            LoggerConfiguration loggerConfiguration,
            DiagnosticSource diagnosticSource,
            IDictionary<object, object> properties) : this(
                scanner,
                assemblyProvider,
                assemblyCandidateFinder,
                environment,
                configuration,
                loggingBuilder,
                @switch,
                loggerConfiguration,
                new DiagnosticLogger(diagnosticSource ?? throw new ArgumentNullException(nameof(diagnosticSource))),
                properties
            )
        { }

        public SerilogBuilder(
            IConventionScanner scanner,
            IAssemblyProvider assemblyProvider,
            IAssemblyCandidateFinder assemblyCandidateFinder,
            IRocketEnvironment environment,
            IConfiguration configuration,
            ILoggingBuilder loggingBuilder,
            LoggingLevelSwitch @switch,
            LoggerConfiguration loggerConfiguration,
            ILogger diagnosticSource,
            IDictionary<object, object> properties) : base(scanner, assemblyProvider, assemblyCandidateFinder, properties)
        {
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            LoggingBuilder = loggingBuilder ?? throw new ArgumentNullException(nameof(loggingBuilder));
            Logger = diagnosticSource ?? throw new ArgumentNullException(nameof(diagnosticSource));
            Switch = @switch ?? throw new ArgumentNullException(nameof(@switch));
            LoggerConfiguration = loggerConfiguration ?? throw new ArgumentNullException(nameof(loggerConfiguration));
        }

        public IConfiguration Configuration { get; }
        public ILogger Logger { get; }
        public LoggingLevelSwitch Switch { get; }
        public LoggerConfiguration LoggerConfiguration { get; }
        public ILoggingBuilder LoggingBuilder { get; }
        public IRocketEnvironment Environment { get; }

        public global::Serilog.ILogger Build()
        {
            new ConventionComposer(Scanner)
                    .Register(this, typeof(ISerilogConvention), typeof(SerilogConventionDelegate));

            return LoggerConfiguration.CreateLogger();
        }
    }
}
