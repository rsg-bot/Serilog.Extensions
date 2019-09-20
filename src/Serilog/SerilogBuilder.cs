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
    /// <summary>
    ///  SerilogBuilder.
    /// Implements the <see cref="ConventionBuilder{ISerilogBuilder, ISerilogConvention, SerilogConventionDelegate}" />
    /// Implements the <see cref="ISerilogBuilder" />
    /// Implements the <see cref="ISerilogConventionContext" />
    /// </summary>
    /// <seealso cref="ConventionBuilder{ISerilogBuilder, ISerilogConvention, SerilogConventionDelegate}" />
    /// <seealso cref="ISerilogBuilder" />
    /// <seealso cref="ISerilogConventionContext" />
    public class SerilogBuilder : ConventionBuilder<ISerilogBuilder, ISerilogConvention, SerilogConventionDelegate>, ISerilogBuilder, ISerilogConventionContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerilogBuilder"/> class.
        /// </summary>
        /// <param name="scanner">The scanner.</param>
        /// <param name="assemblyProvider">The assembly provider.</param>
        /// <param name="assemblyCandidateFinder">The assembly candidate finder.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="switch">The switch.</param>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="diagnosticSource">The diagnostic source.</param>
        /// <param name="properties">The properties.</param>
        /// <exception cref="ArgumentNullException">
        /// environment
        /// or
        /// configuration
        /// or
        /// loggingBuilder
        /// or
        /// diagnosticSource
        /// or
        /// switch
        /// or
        /// loggerConfiguration
        /// </exception>
        public SerilogBuilder(
            IConventionScanner scanner,
            IAssemblyProvider assemblyProvider,
            IAssemblyCandidateFinder assemblyCandidateFinder,
            IRocketEnvironment environment,
            IConfiguration configuration,
            LoggingLevelSwitch @switch,
            LoggerConfiguration loggerConfiguration,
            ILogger diagnosticSource,
            IDictionary<object, object?> properties) : base(scanner, assemblyProvider, assemblyCandidateFinder, properties)
        {
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Logger = diagnosticSource ?? throw new ArgumentNullException(nameof(diagnosticSource));
            Switch = @switch ?? throw new ArgumentNullException(nameof(@switch));
            LoggerConfiguration = loggerConfiguration ?? throw new ArgumentNullException(nameof(loggerConfiguration));
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// A logger that is configured to work with each convention item
        /// </summary>
        /// <value>The logger.</value>
        public ILogger Logger { get; }

        /// <summary>
        /// Gets the switch.
        /// </summary>
        /// <value>The switch.</value>
        public LoggingLevelSwitch Switch { get; }

        /// <summary>
        /// Gets the logger configuration.
        /// </summary>
        /// <value>The logger configuration.</value>
        public LoggerConfiguration LoggerConfiguration { get; }

        /// <summary>
        /// The environment that this convention is running
        /// Based on IHostEnvironment / IHostingEnvironment
        /// </summary>
        /// <value>The environment.</value>
        public IRocketEnvironment Environment { get; }

        /// <summary>
        /// Builds this instance.
        /// </summary>
        /// <returns>Serilog.ILogger.</returns>
        public global::Serilog.ILogger Build()
        {
            return Configure().CreateLogger();
        }

        /// <summary>
        /// Builds this instance.
        /// </summary>
        /// <returns>Serilog.ILogger.</returns>
        public LoggerConfiguration Configure()
        {
            Composer.Register(
                Scanner,
                this,
                typeof(ISerilogConvention),
                typeof(SerilogConventionDelegate)
            );

            return LoggerConfiguration;
        }
    }
}
