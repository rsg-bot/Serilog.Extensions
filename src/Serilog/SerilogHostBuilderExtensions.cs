using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.Serilog.Conventions;
using Serilog;
using Serilog.Core;

namespace Rocket.Surgery.Extensions.Serilog
{
    /// <summary>
    ///  SerilogHostBuilderExtensions.
    /// </summary>
    public static class SerilogHostBuilderExtensions
    {
        /// <summary>
        /// Uses the serilog.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="options">The options.</param>
        /// <returns>IConventionHostBuilder.</returns>
        public static IConventionHostBuilder UseSerilog(
            this IConventionHostBuilder container,
            RocketSerilogOptions options = null)
        {
            container.ServiceProperties[typeof(RocketSerilogOptions)] = options ?? new RocketSerilogOptions();
            container.Scanner.PrependConvention<SerilogServiceConvention>();
            container.Scanner.PrependConvention<SerilogReadFromConfigurationConvention>();
            container.Scanner.PrependConvention<SerilogEnrichLoggingConvention>();
            container.Scanner.PrependConvention<SerilogConsoleLoggingConvention>();
            container.Scanner.PrependConvention<SerilogDebugLoggingConvention>();
            container.Scanner.PrependConvention<EnvironmentLoggingConvention>();
            return container;
        }
    }
}
