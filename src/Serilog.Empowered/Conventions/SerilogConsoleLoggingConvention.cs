using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rocket.Surgery.Extensions.Serilog.Conventions
{

    public class SerilogConsoleLoggingConvention : ISerilogConvention
    {
        private readonly bool async;

        public SerilogConsoleLoggingConvention(bool async)
        {
            this.async = async;
        }

        public void Register(ISerilogConventionContext context)
        {
            if (async)
            {
                context.LoggerConfiguration.WriteTo.Async(Register);
            }
            else
            {
                Register(context.LoggerConfiguration.WriteTo);
            }
        }

        private void Register(LoggerSinkConfiguration configuration)
        {
            configuration.Console(
                restrictedToMinimumLevel: LogEventLevel.Verbose,
                outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level:w4} {SourceContext}] {Message}{NewLine}{Exception}",
                theme: AnsiConsoleTheme.Literate
            );
        }
    }
}
