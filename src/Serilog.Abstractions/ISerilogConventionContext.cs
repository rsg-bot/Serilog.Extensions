using Microsoft.Extensions.Configuration;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Conventions.Reflection;
using Rocket.Surgery.Hosting;
using Serilog;
using Serilog.Core;

namespace Rocket.Surgery.Extensions.Serilog
{
    public interface ISerilogConventionContext : IConventionContext
    {
        IAssemblyProvider AssemblyProvider { get; }
        IAssemblyCandidateFinder AssemblyCandidateFinder { get; }
        IHostingEnvironment Environment { get; }
        IConfiguration Configuration { get; }
        LoggingLevelSwitch Switch { get; }
        LoggerConfiguration Logger { get; }
    }
}

