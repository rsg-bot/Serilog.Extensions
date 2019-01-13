using Serilog;
using Serilog.Configuration;
using System;

namespace Rocket.Surgery.Extensions.Serilog.Conventions
{
    public class SerilogDebugLoggingConvention : ISerilogConvention
    {
        private readonly Func<ISerilogConventionContext, bool> isAsync;

        public SerilogDebugLoggingConvention(Func<ISerilogConventionContext, bool> isAsync)
        {
            this.isAsync = isAsync;
        }

        public void Register(ISerilogConventionContext context)
        {
            if (isAsync(context))
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
            configuration.Debug(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:w4} {SourceContext}] {Message}{NewLine}{Exception}");
        }
    }
}
