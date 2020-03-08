using System;
using JetBrains.Annotations;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.Serilog.Conventions;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

[assembly: Convention(typeof(SerilogDebugLoggingConvention))]

namespace Rocket.Surgery.Extensions.Serilog.Conventions
{
    /// <summary>
    /// SerilogDebugLoggingConvention.
    /// Implements the <see cref="ISerilogConvention" />
    /// </summary>
    /// <seealso cref="ISerilogConvention" />
    [LiveConvention]
    public sealed class SerilogDebugLoggingConvention : SerilogConditionallyAsyncLoggingConvention
    {
        private readonly RocketSerilogOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerilogDebugLoggingConvention" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public SerilogDebugLoggingConvention(RocketSerilogOptions? options = null)
            => _options = options ?? new RocketSerilogOptions();

        /// <inheritdoc />
        protected override void Register([NotNull] LoggerSinkConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            configuration.Debug(
                LogEventLevel.Verbose,
                _options.DebugMessageTemplate
            );
        }
    }
}