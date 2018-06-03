using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Conventions.Reflection;

namespace Rocket.Surgery.Extensions.Logging
{
    public interface ILoggingConventionContext : IConventionContext, Microsoft.Extensions.Logging.ILoggingBuilder
    {
        IAssemblyProvider AssemblyProvider { get; }
        IAssemblyCandidateFinder AssemblyCandidateFinder { get; }
        IHostingEnvironment Environment { get; }
        IConfiguration Configuration { get; }
    }
}
