using Microsoft.Extensions.Configuration;
#if NET451 || NETSTANDARD1_3
using Microsoft.Extensions.DependencyInjection;
#endif
using Rocket.Surgery.Builders;
using Rocket.Surgery.Conventions.Reflection;
using Rocket.Surgery.Hosting;

namespace Rocket.Surgery.Extensions.Logging
{
    /// <summary>
    /// Interface ILoggingConvention
    /// </summary>
    /// TODO Edit XML Comment Template for ILoggingConvention
    public interface ILoggingBuilder : IBuilder
#if NET461 || NETSTANDARD2_0
        , Microsoft.Extensions.Logging.ILoggingBuilder
#endif
    {
        IAssemblyProvider AssemblyProvider { get; }
        IAssemblyCandidateFinder AssemblyCandidateFinder { get; }
#if NET451 || NETSTANDARD1_3
        IServiceCollection Services { get; }
#endif
        IHostingEnvironment Environment { get; }
        IConfiguration Configuration { get; }
        ILoggingBuilder AddDelegate(LoggingConventionDelegate @delegate);
        ILoggingBuilder AddConvention(ILoggingConvention convention);
    }
}
