using System;
using JetBrains.Annotations;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.Serilog.Conventions;
using Serilog;

[assembly: Convention(typeof(SerilogEnrichLoggingConvention))]

namespace Rocket.Surgery.Extensions.Serilog.Conventions
{
    /// <summary>
    /// SerilogEnrichLoggingConvention.
    /// Implements the <see cref="ISerilogConvention" />
    /// </summary>
    /// <seealso cref="ISerilogConvention" />
    [LiveConvention]
    public class SerilogEnrichEnvironmentLoggingConvention : ISerilogConvention
    {
        /// <summary>
        /// Registers the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Register([NotNull] ISerilogConventionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.LoggerConfiguration
               .Enrich.WithEnvironmentUserName()
               .Enrich.WithMachineName()
               .Enrich.WithProcessId()
               .Enrich.WithProcessName()
               .Enrich.WithThreadId();
        }
    }
}