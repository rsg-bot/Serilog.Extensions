using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.Serilog.Conventions;

[assembly: Convention(typeof(EnvironmentLoggingConvention))]

namespace Rocket.Surgery.Extensions.Serilog.Conventions
{
    /// <summary>
    ///  EnvironmentLoggingConvention.
    /// Implements the <see cref="Rocket.Surgery.Extensions.Serilog.ISerilogConvention" />
    /// </summary>
    /// <seealso cref="Rocket.Surgery.Extensions.Serilog.ISerilogConvention" />
    public class EnvironmentLoggingConvention : ISerilogConvention
    {
        /// <summary>
        /// Registers the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Register(ISerilogConventionContext context)
        {
            var environment = context.Environment;
            context.LoggerConfiguration.Enrich.WithProperty(nameof(environment.EnvironmentName), environment.EnvironmentName);
            context.LoggerConfiguration.Enrich.WithProperty(nameof(environment.ApplicationName), environment.ApplicationName);
        }
    }
}
