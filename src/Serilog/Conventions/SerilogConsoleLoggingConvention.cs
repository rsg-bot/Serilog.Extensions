using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.Logging;
using Rocket.Surgery.Extensions.Serilog.Conventions;

[assembly: Convention(typeof(SerilogConsoleLoggingConvention))]

namespace Rocket.Surgery.Extensions.Serilog.Conventions
{
    /// <summary>
    ///  SerilogConsoleLoggingConvention.
    /// Implements the <see cref="ILoggingConvention" />
    /// Implements the <see cref="ISerilogConvention" />
    /// </summary>
    /// <seealso cref="ILoggingConvention" />
    /// <seealso cref="ISerilogConvention" />
    public class SerilogConsoleLoggingConvention : ILoggingConvention, ISerilogConvention
    {
        private readonly RocketSerilogOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerilogConsoleLoggingConvention"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public SerilogConsoleLoggingConvention(RocketSerilogOptions options = null)
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
            var serviceDescriptor = context.Services.FirstOrDefault(x =>
                x.ImplementationType?.FullName == "Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider");
            if (serviceDescriptor != null)
            {
                context.Services.Remove(serviceDescriptor);
            }
        }

        private void Register(LoggerSinkConfiguration configuration)
        {
            configuration.Console(
                restrictedToMinimumLevel: LogEventLevel.Verbose,
                outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level:w4} {SourceContext}] {Message}{NewLine}{Exception}",
                theme: AnsiConsoleTheme.Literate
            );
        }
    }
}
