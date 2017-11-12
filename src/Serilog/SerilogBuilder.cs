using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rocket.Surgery.Builders;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Conventions.Reflection;
using Rocket.Surgery.Conventions.Scanners;
using Rocket.Surgery.Hosting;
using Serilog;
using Serilog.Core;

namespace Rocket.Surgery.Extensions.Serilog
{
    public class SerilogBuilder : Builder, ISerilogBuilder, ISerilogConventionContext
    {
        private readonly IConventionScanner _scanner;

        public SerilogBuilder(
            IConventionScanner scanner,
            IAssemblyProvider assemblyProvider,
            IAssemblyCandidateFinder assemblyCandidateFinder,
            IHostingEnvironment environment,
            IConfiguration configuration,
            LoggingLevelSwitch @switch,
            LoggerConfiguration logger)
        {
            _scanner = scanner;
            AssemblyProvider = assemblyProvider;
            AssemblyCandidateFinder = assemblyCandidateFinder;
            Environment = environment;
            Configuration = configuration;
            Switch = @switch;
            Logger = logger;
        }

        public IAssemblyProvider AssemblyProvider { get; }
        public IAssemblyCandidateFinder AssemblyCandidateFinder { get; }
        public IHostingEnvironment Environment { get; }
        public IConfiguration Configuration { get; }
        public LoggingLevelSwitch Switch { get; }
        public LoggerConfiguration Logger { get; }

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

        public ILogger Build(Microsoft.Extensions.Logging.ILogger logger)
        {
            new ConventionComposer(_scanner, logger)
                    .Register(this, typeof(ISerilogConventionContext), typeof(SerilogConventionDelegate));

            return Logger.CreateLogger();
        }
    }
}
