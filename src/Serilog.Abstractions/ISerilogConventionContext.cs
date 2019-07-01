using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Conventions.Reflection;
using Serilog;
using Serilog.Core;

namespace Rocket.Surgery.Extensions.Serilog
{
    public interface ISerilogConventionContext : IConventionContext
    {
        IAssemblyProvider AssemblyProvider { get; }
        IAssemblyCandidateFinder AssemblyCandidateFinder { get; }
        IConfiguration Configuration { get; }
        LoggingLevelSwitch Switch { get; }
        LoggerConfiguration LoggerConfiguration { get; }
        ILoggingBuilder LoggingBuilder { get;  }

        /// <summary>
        /// The environment that this convention is running
        ///
        /// Based on IHostEnvironment / IHostingEnvironment
        /// </summary>
        IRocketEnvironment Environment { get; }
    }
}

