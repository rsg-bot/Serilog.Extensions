using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.Serilog.Conventions;

namespace Rocket.Surgery.Extensions.Serilog
{
    public static class SerilogBuilderExtensions
    {
        public static IConventionHostBuilder UseSerilog(
            this IConventionHostBuilder container,
            SerilogOptions options = null)
        {
            options = options ?? new SerilogOptions();
            container.Scanner.PrependConvention(new EnvironmentLoggingConvention());
            container.Scanner.PrependConvention(new SerilogConsoleLoggingConvention(options.IsAsync));
            container.Scanner.PrependConvention(new SerilogDebugLoggingConvention(options.IsAsync));
            container.Scanner.PrependConvention(new SerilogEnrichLoggingConvention());
            container.Scanner.PrependConvention(new SerilogServiceConvention(container.Scanner, container.DiagnosticSource, options));
            return container;
        }
    }
}
