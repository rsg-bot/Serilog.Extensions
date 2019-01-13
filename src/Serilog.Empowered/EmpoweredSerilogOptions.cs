using Microsoft.Extensions.Logging;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.Logging;
using Serilog;
using Serilog.Core;
using System;

namespace Rocket.Surgery.Extensions.Serilog.Empowered
{
    public class EmpoweredSerilogOptions
    {
        public Func<ISerilogConventionContext, bool> IsAsync { get; set; } = (context) => true;
        public LoggingLevelSwitch LoggingLevelSwitch { get; set; } = new LoggingLevelSwitch();
        public LoggerConfiguration LoggerConfiguration { get; set; } = new LoggerConfiguration();
        public Func<IConventionContext, LogLevel> GetLogLevel { get; set; } = (context) => LogLevel.Information;
    }
}
