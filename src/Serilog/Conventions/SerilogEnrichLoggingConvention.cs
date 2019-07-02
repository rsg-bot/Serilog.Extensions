using Microsoft.Extensions.Options;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.Serilog.Conventions;
using Serilog;
using Serilog.Core;

[assembly: Convention(typeof(SerilogEnrichLoggingConvention))]

namespace Rocket.Surgery.Extensions.Serilog.Conventions
{
    /// <summary>
    ///  SerilogEnrichLoggingConvention.
    /// Implements the <see cref="Rocket.Surgery.Extensions.Serilog.ISerilogConvention" />
    /// </summary>
    /// <seealso cref="Rocket.Surgery.Extensions.Serilog.ISerilogConvention" />
    public class SerilogEnrichLoggingConvention : ISerilogConvention
    {
        /// <summary>
        /// Registers the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
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
