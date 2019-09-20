using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rocket.Surgery.Conventions.Scanners;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using System.Diagnostics;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.Logging;
using Rocket.Surgery.Extensions.Serilog.Conventions;
using Serilog.Core;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Rocket.Surgery.Extensions.DependencyInjection;

[assembly: Convention(typeof(LoggingLevelSwitchConvention))]

namespace Rocket.Surgery.Extensions.Serilog.Conventions
{
    /// <summary>
    ///  LoggingLevelSwitchConvention.
    /// Implements the <see cref="ILoggingConvention" />
    /// </summary>
    /// <seealso cref="ILoggingConvention" />
    public class LoggingLevelSwitchConvention : ILoggingConvention, IServiceConvention, ISerilogConvention
    {
        private readonly RocketSerilogOptions _serilogOptions;
        private readonly RocketLoggingOptions _loggingOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingLevelSwitchConvention"/> class.
        /// </summary>
        /// <param name="serilogOptions">The serilog options.</param>
        /// <param name="loggingOptions">The logging options.</param>
        public LoggingLevelSwitchConvention(
            RocketSerilogOptions? serilogOptions = null,
            RocketLoggingOptions? loggingOptions = null)
        {
            _serilogOptions = serilogOptions ?? new RocketSerilogOptions();
            _loggingOptions = loggingOptions ?? new RocketLoggingOptions();
        }

        /// <summary>
        /// Registers the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Register(IServiceConventionContext context)
        {
            context.Services.AddHostedService<SerilogFinalizerHostedService>();
        }

        /// <summary>
        /// Registers the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Register(ILoggingConventionContext context)
        {
            var loggingLevelSwitch = context.Get<LoggingLevelSwitch>() ?? new LoggingLevelSwitch();
            context.Services.AddSingleton(loggingLevelSwitch);
            loggingLevelSwitch.MinimumLevel = _serilogOptions.GetLogLevel == null ?
                LevelConvert.ToSerilogLevel(_loggingOptions.GetLogLevel(context)) :
                loggingLevelSwitch.MinimumLevel;
        }

        /// <summary>
        /// Registers the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Register(ISerilogConventionContext context)
        {
            var loggingLevelSwitch = context.Get<LoggingLevelSwitch>() ?? new LoggingLevelSwitch();
            loggingLevelSwitch.MinimumLevel = _serilogOptions.GetLogLevel?.Invoke(context) ?? loggingLevelSwitch.MinimumLevel;
        }
    }
}
