using Microsoft.Extensions.Logging;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.Serilog.Conventions;

namespace Rocket.Surgery.Extensions.Serilog.AspNetCore
{
    public static class RequestLoggingSerilogExtensions
    {
        public static IConventionHostBuilder UseRequestLoggingSerilog(
            this IConventionHostBuilder container)
        {
            container.Scanner.AppendConvention(new RequestLoggingConvention());
            return container;
        }
    }
}
