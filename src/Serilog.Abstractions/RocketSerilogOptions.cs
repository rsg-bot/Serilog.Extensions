using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Rocket.Surgery.Extensions.Serilog
{
    /// <summary>
    ///  RocketSerilogOptions.
    /// </summary>
    public class RocketSerilogOptions
    {
        /// <summary>
        /// Determines how the loglevel is captured, defaults to the value that can be set into the configuration
        /// ApplicationState:IsDefaultCommand
        /// </summary>
        /// <value>The is asynchronous.</value>
        public Func<ISerilogConventionContext, bool> IsAsync { get; set; } = context =>
            context.Configuration.GetValue("ApplicationState:IsDefaultCommand", true);

        /// <summary>
        /// Determines how the loglevel is captured, defaults to the value that can be set into the configuration
        /// IApplicationState:LogLevel
        /// </summary>
        /// <value>The get log level.</value>
        public Func<ISerilogConventionContext, LogEventLevel> GetLogLevel { get; set; }
    }
}
