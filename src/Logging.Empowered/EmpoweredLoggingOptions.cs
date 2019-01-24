using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Rocket.Surgery.Conventions;

namespace Rocket.Surgery.Extensions.Logging
{
    public class EmpoweredLoggingOptions
    {
        /// <summary>
        /// Determines how the loglevel is captured, defautlts to the value that can be set into the configuraiton
        /// IApplicationState:LogLevel
        /// </summary>
        public Func<ILoggingConventionContext, LogLevel> GetLogLevel { get; set; } = context => context.Configuration.GetValue("ApplicationState:LogLevel", LogLevel.Information);
    }
}
