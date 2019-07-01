using Microsoft.Extensions.Logging;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.Serilog.AspNetCore.Conventions;
using Rocket.Surgery.Extensions.Serilog.Conventions;

namespace Rocket.Surgery.Extensions.Serilog.AspNetCore
{
    public static class RequestLoggingSerilogExtensions
    {
        public static IConventionHostBuilder UseSerilogRequestLogging(
            this IConventionHostBuilder container)
        {
            container.Scanner.PrependConvention(new RequestLoggingConvention());
            return container;
        }
    }
}
