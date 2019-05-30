using Microsoft.Extensions.Logging;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.Serilog.Conventions;

namespace Rocket.Surgery.Extensions.Serilog.AspNetCore
{
    public static class RequestLoggingSerilogExtensions
    {
        public static T UseRequestLoggingSerilog<T>(
            this T container)
            where T : IConventionHostBuilder
        {
            container.AppendConvention(new RequestLoggingConvention());
            return container;
        }
    }
}
