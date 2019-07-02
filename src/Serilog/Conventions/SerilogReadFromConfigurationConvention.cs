using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.Serilog.Conventions;
using Serilog;

[assembly:Convention(typeof(SerilogReadFromConfigurationConvention))]

namespace Rocket.Surgery.Extensions.Serilog.Conventions
{
    /// <summary>
    ///  SerilogReadFromConfigurationConvention.
    /// Implements the <see cref="Rocket.Surgery.Extensions.Serilog.ISerilogConvention" />
    /// </summary>
    /// <seealso cref="Rocket.Surgery.Extensions.Serilog.ISerilogConvention" />
    public class SerilogReadFromConfigurationConvention : ISerilogConvention
    {
        /// <summary>
        /// Registers the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Register(ISerilogConventionContext context)
        {
            context.LoggerConfiguration.ReadFrom.Configuration(context.Configuration);
        }
    }
}
