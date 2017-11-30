using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Conventions.Reflection;
using Rocket.Surgery.Hosting;
using Serilog;
using Serilog.Core;

namespace Rocket.Surgery.Extensions.Serilog
{
    public interface ISerilogConventionContext : IConventionContext
#if NET461 || NETSTANDARD2_0
        , Microsoft.Extensions.Logging.ILoggingBuilder
#endif
    {
        IAssemblyProvider AssemblyProvider { get; }
        IAssemblyCandidateFinder AssemblyCandidateFinder { get; }
        IHostingEnvironment Environment { get; }
        IConfiguration Configuration { get; }
#if NET451 || NETSTANDARD1_3
        IServiceCollection Services { get; }
#endif
        LoggingLevelSwitch Switch { get; }
        LoggerConfiguration Logger { get; }
    }
}

