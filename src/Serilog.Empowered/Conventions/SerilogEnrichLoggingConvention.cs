using Serilog;

namespace Rocket.Surgery.Extensions.Serilog.Conventions
{
    public class SerilogEnrichLoggingConvention : ISerilogConvention
    {
        public void Register(ISerilogConventionContext context)
        {
            context.LoggerConfiguration
                .Enrich.FromLogContext()
                .Enrich.WithDemystifiedStackTraces()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .Enrich.WithThreadId();
        }
    }
}
