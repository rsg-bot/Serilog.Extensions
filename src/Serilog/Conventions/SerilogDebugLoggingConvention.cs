using Serilog;
using Serilog.Configuration;
using System;
using System.Linq;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.Logging;
using Rocket.Surgery.Extensions.Serilog.Conventions;

[assembly: Convention(typeof(SerilogDebugLoggingConvention))]

namespace Rocket.Surgery.Extensions.Serilog.Conventions
{
    /// <summary>
    ///  SerilogDebugLoggingConvention.
    /// Implements the <see cref="ILoggingConvention" />
    /// Implements the <see cref="ISerilogConvention" />
    /// </summary>
    /// <seealso cref="ILoggingConvention" />
    /// <seealso cref="ISerilogConvention" />
    public class SerilogDebugLoggingConvention : ILoggingConvention, ISerilogConvention
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

        /// <summary>
        /// Registers the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Register(ILoggingConventionContext context)
        {
            // We remove logging here by default as we don't "take over" all logging duties by default.
            var serviceDescriptors = context.Services.Where(x =>
                x.ImplementationType?.FullName == "Microsoft.Extensions.Logging.Debug.DebugLoggerProvider")
                .ToArray();
            if (serviceDescriptors.Any())
                foreach (var serviceDescriptor in serviceDescriptors)
                    context.Services.Remove(serviceDescriptor);
        }

        private void Register(LoggerSinkConfiguration configuration)
        {
            configuration.Debug(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:w4} {SourceContext}] {Message}{NewLine}{Exception}");
        }
    }
}
