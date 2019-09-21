using Microsoft.Extensions.Configuration;

namespace Rocket.Surgery.Extensions.Serilog
{
    internal static class ConfigurationAsyncHelper
    {
        public static bool IsAsync(IConfiguration configuration) => configuration.GetValue("ApplicationState:IsDefaultCommand", true);
    }
}
