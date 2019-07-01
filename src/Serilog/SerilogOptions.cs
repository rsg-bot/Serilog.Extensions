using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;

namespace Rocket.Surgery.Extensions.Serilog
{
    public class SerilogOptions
    {
        public Func<ISerilogConventionContext, bool> IsAsync { get; set; } = context => context.Configuration.GetValue("ApplicationState:IsDefaultCommand", true);
        public LoggingLevelSwitch LoggingLevelSwitch { get; set; } = new LoggingLevelSwitch();
        public LoggerConfiguration LoggerConfiguration { get; set; } = new LoggerConfiguration();
        /// <summary>
        /// Determines how the loglevel is captured, defautlts to the value that can be set into the configuraiton
        /// IApplicationState:LogLevel
        /// </summary>
        public Func<ISerilogConventionContext, LogLevel> GetLogLevel { get; set; } = context => context.Configuration.GetValue("ApplicationState:LogLevel", LogLevel.Information);
    }
}
