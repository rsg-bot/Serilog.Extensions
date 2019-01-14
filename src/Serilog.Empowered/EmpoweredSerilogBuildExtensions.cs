using Microsoft.Extensions.Logging;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.Serilog.Conventions;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;

namespace Rocket.Surgery.Extensions.Serilog.Empowered
{
    public static class EmpoweredSerilogExtensions
    {
        public static T UseEmpoweredSerilog<T>(
            this T container,
            EmpoweredSerilogOptions options)
            where T : IConventionHostBuilder
        {
            container.AppendConvention(new EnvironmentLoggingConvention());
            container.AppendConvention(new RequestLoggingConvention());
            container.AppendConvention(new SerilogConsoleLoggingConvention(options.IsAsync));
            container.AppendConvention(new SerilogDebugLoggingConvention(options.IsAsync));
            container.AppendConvention(new SerilogEnrichLoggingConvention());
            container.AppendConvention(new SerilogServiceConvention(container.Scanner, container.DiagnosticSource, options));
            return container;
        }
    }
}
