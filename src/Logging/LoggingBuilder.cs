using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rocket.Surgery.Builders;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Conventions.Reflection;
using Rocket.Surgery.Conventions.Scanners;
using Rocket.Surgery.Extensions.Logging;
using Rocket.Surgery.Hosting;

namespace Rocket.Surgery.Extensions.Logging
{
    /// <summary>
    /// Logging Builder
    /// </summary>
    public class LoggingBuilder : Builder, ILoggingBuilder, ILoggingConventionContext
    {
        private readonly IConventionScanner _scanner;

        public LoggingBuilder(
            IConventionScanner scanner,
            IAssemblyProvider assemblyProvider,
            IAssemblyCandidateFinder assemblyCandidateFinder,
            IServiceCollection services,
            IHostingEnvironment envionment,
            IConfiguration configuration)
        {
            AssemblyProvider = assemblyProvider;
            AssemblyCandidateFinder = assemblyCandidateFinder;
            _scanner = scanner;
            Services = services;
            Environment = envionment;
            Configuration = configuration;
        }

        public IAssemblyProvider AssemblyProvider { get; }
        public IAssemblyCandidateFinder AssemblyCandidateFinder { get; }
        public IServiceCollection Services { get; }
        public IHostingEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public ILoggingBuilder AddDelegate(LoggingConventionDelegate @delegate)
        {
            _scanner.AddDelegate(@delegate);
            return this;
        }

        public ILoggingBuilder AddConvention(ILoggingConvention convention)
        {
            _scanner.AddConvention(convention);
            return this;
        }

        public void Build(ILogger logger)
        {
            new ConventionComposer(_scanner, logger).Register(
                this,
                typeof(ILoggingConventionContext),
                typeof(LoggingConventionDelegate)
            );
        }
    }
}
