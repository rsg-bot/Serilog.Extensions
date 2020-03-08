using System;
using JetBrains.Annotations;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.Serilog.Conventions;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

[assembly: Convention(typeof(SerilogConsoleLoggingConvention))]

namespace Rocket.Surgery.Extensions.Serilog.Conventions
{
    /// <summary>
    /// SerilogConsoleLoggingConvention.
    /// Implements the <see cref="ISerilogConvention" />
    /// </summary>
    /// <seealso cref="ISerilogConvention" />
    [LiveConvention]
    public sealed class SerilogConsoleLoggingConvention : SerilogConditionallyAsyncLoggingConvention
    {
        private readonly RocketSerilogOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerilogConsoleLoggingConvention" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public SerilogConsoleLoggingConvention(RocketSerilogOptions? options = null)
            => _options = options ?? new RocketSerilogOptions();

        /// <inheritdoc />
        protected override void Register([NotNull] LoggerSinkConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            configuration.Console(
                LogEventLevel.Verbose,
                _options.ConsoleMessageTemplate,
                theme: AnsiConsoleTheme.Literate
            );
        }
    }
}