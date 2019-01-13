using System;
using Microsoft.Extensions.Logging;
using Rocket.Surgery.Conventions;

namespace Rocket.Surgery.Extensions.Logging
{
    public class EmpoweredLoggingOptions
    {
        public Func<IConventionContext, LogLevel> GetLogLevel { get; set; } = (context) => LogLevel.Information;
    }
}
