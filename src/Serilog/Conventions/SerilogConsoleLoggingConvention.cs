using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.Serilog.Conventions;

[assembly: Convention(typeof(SerilogConsoleLoggingConvention))]

namespace Rocket.Surgery.Extensions.Serilog.Conventions
{
    /// <summary>
    ///  SerilogConsoleLoggingConvention.
    /// Implements the <see cref="ISerilogConvention" />
    /// </summary>
    /// <seealso cref="ISerilogConvention" />
    public sealed class SerilogConsoleLoggingConvention : SerilogConditionallyAsyncLoggingConvention
    {
        private readonly RocketSerilogOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerilogConsoleLoggingConvention"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public SerilogConsoleLoggingConvention(RocketSerilogOptions? options = null)
        {
            _options = options ?? new RocketSerilogOptions();
        }

        /// <inheritdoc />
        protected override void Register(LoggerSinkConfiguration configuration) =>
            configuration.Console(
                restrictedToMinimumLevel: LogEventLevel.Verbose,
                outputTemplate: _options.ConsoleMessageTemplate,
                theme: AnsiConsoleTheme.Literate
            );
    }
}
