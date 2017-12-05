using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Rocket.Surgery.Builders;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Conventions.Reflection;
using Rocket.Surgery.Conventions.Scanners;
using Rocket.Surgery.Hosting;
using Serilog;
using Serilog.Core;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Rocket.Surgery.Extensions.Serilog
{
    public class SerilogBuilder : Builder, ISerilogBuilder
    {
        private readonly IConventionScanner _scanner;

        public SerilogBuilder(
            IConventionScanner scanner,
            IAssemblyProvider assemblyProvider,
            IAssemblyCandidateFinder assemblyCandidateFinder,
            IHostingEnvironment environment,
            IConfiguration configuration,
            ILogger logger,
            LoggingLevelSwitch @switch,
            LoggerConfiguration loggerConfiguration)
        {
            _scanner = scanner;
            AssemblyProvider = assemblyProvider;
            AssemblyCandidateFinder = assemblyCandidateFinder;
            Environment = environment;
            Configuration = configuration;
            Logger = logger;
            Switch = @switch;
            LoggerConfiguration = loggerConfiguration;
        }

        public IAssemblyProvider AssemblyProvider { get; }
        public IAssemblyCandidateFinder AssemblyCandidateFinder { get; }
        public IHostingEnvironment Environment { get; }
        public IConfiguration Configuration { get; }
        public ILogger Logger { get; }
        public LoggingLevelSwitch Switch { get; }
        public LoggerConfiguration LoggerConfiguration { get; }

        public ISerilogBuilder AddDelegate(SerilogConventionDelegate @delegate)
        {
            _scanner.AddDelegate(@delegate);
            return this;
        }

        public ISerilogBuilder AddConvention(ISerilogConvention convention)
        {
            _scanner.AddConvention(convention);
            return this;
        }

        public global::Serilog.ILogger Build()
        {
            new ConventionComposer(_scanner)
                    .Register(this, typeof(ISerilogConvention), typeof(SerilogConventionDelegate));

            return LoggerConfiguration.CreateLogger();
        }
    }
}
