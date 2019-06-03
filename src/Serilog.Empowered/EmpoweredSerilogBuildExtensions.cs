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
        public static IConventionHostBuilder UseEmpoweredSerilog(
            this IConventionHostBuilder container,
            EmpoweredSerilogOptions options)
        {
            container.Scanner.AppendConvention(new EnvironmentLoggingConvention());
            container.Scanner.AppendConvention(new SerilogConsoleLoggingConvention(options.IsAsync));
            container.Scanner.AppendConvention(new SerilogDebugLoggingConvention(options.IsAsync));
            container.Scanner.AppendConvention(new SerilogEnrichLoggingConvention());
            container.Scanner.AppendConvention(new SerilogServiceConvention(container.Scanner, container.DiagnosticSource, options));
            return container;
        }
    }
}
