using Microsoft.Extensions.Logging;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.Serilog.AspNetCore.Conventions;
using Rocket.Surgery.Extensions.Serilog.Conventions;

namespace Rocket.Surgery.Extensions.Serilog.AspNetCore
{
    /// <summary>
    ///  RequestLoggingSerilogExtensions.
    /// </summary>
    public static class RequestLoggingSerilogExtensions
    {
        /// <summary>
        /// Uses the serilog request logging.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>IConventionHostBuilder.</returns>
        public static IConventionHostBuilder UseSerilogRequestLogging(
            this IConventionHostBuilder container)
        {
            container.Scanner.PrependConvention<RequestLoggingConvention>();
            return container;
        }
    }
}
