using Microsoft.Extensions.Configuration;
using Rocket.Surgery.Builders;
using Rocket.Surgery.Conventions.Reflection;
using Rocket.Surgery.Hosting;
using Serilog;
using Serilog.Core;

namespace Rocket.Surgery.Extensions.Serilog
{
    /// <summary>
    /// Interface ILoggingConvention
    /// </summary>
    /// TODO Edit XML Comment Template for ILoggingConvention
    public interface ISerilogBuilder : IBuilder
    {
        IAssemblyProvider AssemblyProvider { get; }
        IAssemblyCandidateFinder AssemblyCandidateFinder { get; }
        IHostingEnvironment Environment { get; }
        IConfiguration Configuration { get; }
        LoggingLevelSwitch Switch { get; }
        LoggerConfiguration Logger { get; }
        ISerilogBuilder AddDelegate(SerilogConventionDelegate @delegate);
        ISerilogBuilder AddConvention(ISerilogConvention convention);
    }
}
