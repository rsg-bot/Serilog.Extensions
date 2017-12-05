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
    public class LoggingBuilder : Builder, ILoggingBuilder
    {
        private readonly IConventionScanner _scanner;

        public LoggingBuilder(
            IConventionScanner scanner,
            IAssemblyProvider assemblyProvider,
            IAssemblyCandidateFinder assemblyCandidateFinder,
            IServiceCollection services,
            IHostingEnvironment envionment,
            IConfiguration configuration,
            ILogger logger)
        {
            AssemblyProvider = assemblyProvider ?? throw new ArgumentNullException(nameof(assemblyProvider));
            AssemblyCandidateFinder = assemblyCandidateFinder ?? throw new ArgumentNullException(nameof(assemblyCandidateFinder));
            _scanner = scanner ?? throw new ArgumentNullException(nameof(scanner));
            Services = services ?? throw new ArgumentNullException(nameof(services));
            Environment = envionment ?? throw new ArgumentNullException(nameof(envionment));
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IAssemblyProvider AssemblyProvider { get; }
        public IAssemblyCandidateFinder AssemblyCandidateFinder { get; }
        public IServiceCollection Services { get; }
        public IHostingEnvironment Environment { get; }
        public IConfiguration Configuration { get; }
        public ILogger Logger { get; }

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

        public void Build()
        {
            new ConventionComposer(_scanner).Register(
                this,
                typeof(ILoggingConvention),
                typeof(LoggingConventionDelegate)
            );
        }
    }
}
