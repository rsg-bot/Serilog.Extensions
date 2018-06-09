using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Rocket.Surgery.Builders;
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
        private readonly DiagnosticSource _diagnosticSource;
        public SerilogBuilder(
            IConventionScanner scanner,
            IAssemblyProvider assemblyProvider,
            IAssemblyCandidateFinder assemblyCandidateFinder,
            IHostingEnvironment environment,
            IConfiguration configuration,
            DiagnosticSource diagnosticSource,
            LoggingLevelSwitch @switch,
            LoggerConfiguration loggerConfiguration,
            IDictionary<object, object> properties) : base(scanner, assemblyProvider, assemblyCandidateFinder, properties)
        {
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _diagnosticSource = diagnosticSource ?? throw new ArgumentNullException(nameof(diagnosticSource));
            Logger = new DiagnosticLogger(_diagnosticSource);
            Switch = @switch ?? throw new ArgumentNullException(nameof(@switch));
            LoggerConfiguration = loggerConfiguration ?? throw new ArgumentNullException(nameof(loggerConfiguration));
        }

        protected override ISerilogBuilder GetBuilder() => this;

        public IHostingEnvironment Environment { get; }
        public IConfiguration Configuration { get; }
        public ILogger Logger { get; }
        public LoggingLevelSwitch Switch { get; }
        public LoggerConfiguration LoggerConfiguration { get; }

        public global::Serilog.ILogger Build()
        {
            new ConventionComposer(Scanner)
                    .Register(this, typeof(ISerilogConvention), typeof(SerilogConventionDelegate));

            return LoggerConfiguration.CreateLogger();
        }
    }
}
