using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Conventions.Reflection;
using Serilog;
using Serilog.Core;

namespace Rocket.Surgery.Extensions.Serilog
{
    /// <summary>
    ///  ISerilogConventionContext
    /// Implements the <see cref="IConventionContext" />
    /// </summary>
    /// <seealso cref="IConventionContext" />
    public interface ISerilogConventionContext : IConventionContext
    {
        /// <summary>
        /// Gets the assembly provider.
        /// </summary>
        /// <value>The assembly provider.</value>
        IAssemblyProvider AssemblyProvider { get; }

        /// <summary>
        /// Gets the assembly candidate finder.
        /// </summary>
        /// <value>The assembly candidate finder.</value>
        IAssemblyCandidateFinder AssemblyCandidateFinder { get; }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        IConfiguration Configuration { get; }

        /// <summary>
        /// Gets the switch.
        /// </summary>
        /// <value>The switch.</value>
        LoggingLevelSwitch Switch { get; }

        /// <summary>
        /// Gets the logger configuration.
        /// </summary>
        /// <value>The logger configuration.</value>
        LoggerConfiguration LoggerConfiguration { get; }

        /// <summary>
        /// Gets the logging builder.
        /// </summary>
        /// <value>The logging builder.</value>
        ILoggingBuilder LoggingBuilder { get;  }

        /// <summary>
        /// The environment that this convention is running
        /// Based on IHostEnvironment / IHostingEnvironment
        /// </summary>
        /// <value>The environment.</value>
        IRocketEnvironment Environment { get; }
    }
}

