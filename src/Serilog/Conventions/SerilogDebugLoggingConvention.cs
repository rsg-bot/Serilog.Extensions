using Serilog;
using Serilog.Configuration;
using System;
using System.Linq;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.Serilog.Conventions;

[assembly: Convention(typeof(SerilogDebugLoggingConvention))]

namespace Rocket.Surgery.Extensions.Serilog.Conventions
{
    /// <summary>
    ///  SerilogDebugLoggingConvention.
    /// Implements the <see cref="ISerilogConvention" />
    /// </summary>
    /// <seealso cref="ISerilogConvention" />
    public class SerilogDebugLoggingConvention : ISerilogConvention
    {
        private readonly RocketSerilogOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerilogDebugLoggingConvention"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public SerilogDebugLoggingConvention(RocketSerilogOptions? options = null)
        {
            _options = options ?? new RocketSerilogOptions();
        }

        /// <summary>
        /// Registers the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Register(ISerilogConventionContext context)
        {
            if (_options.IsAsync(context))
            {
                context.LoggerConfiguration.WriteTo.Async(Register);
            }
            else
            {
                Register(context.LoggerConfiguration.WriteTo);
            }
        }

        private void Register(LoggerSinkConfiguration configuration)
        {
            configuration.Debug(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:w4} {SourceContext}] {Message}{NewLine}{Exception}");
        }
    }
}
