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
        /// The default console message template
        /// </summary>
        public string ConsoleMessageTemplate { get; set; } = "[{Timestamp:HH:mm:ss} {Level:w4}] {Message}{NewLine}{Exception}";

        /// <summary>
        /// The default debug message template
        /// </summary>
        public string DebugMessageTemplate { get; set; } = "[{Timestamp:HH:mm:ss} {Level:w4}] {Message}{NewLine}{Exception}";

        /// <summary>
        /// Base option from the serilog package
        /// </summary>
        public bool WriteToProviders { get; set; } = true;

        /// <summary>
        /// Base option from the serilog package
        /// </summary>
        public bool PreserveStaticLogger { get; set; }
    }
}
