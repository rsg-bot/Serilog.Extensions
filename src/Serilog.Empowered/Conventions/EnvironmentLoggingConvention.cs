namespace Rocket.Surgery.Extensions.Serilog.Conventions
{
    public class EnvironmentLoggingConvention : ISerilogConvention
    {
        public void Register(ISerilogConventionContext context)
        {
            var environment = context.Environment;
            context.LoggerConfiguration.Enrich.WithProperty(nameof(environment.EnvironmentName), environment.EnvironmentName);
            context.LoggerConfiguration.Enrich.WithProperty(nameof(environment.ApplicationName), environment.ApplicationName);
        }
    }
}
