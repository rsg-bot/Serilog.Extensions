using Serilog;
using Serilog.Configuration;

namespace Rocket.Surgery.Extensions.Serilog.Conventions
{
    public class SerilogDebugLoggingConvention : ISerilogConvention
    {
        private readonly bool async;

        public SerilogDebugLoggingConvention(bool async)
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
            configuration.Debug(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:w4} {SourceContext}] {Message}{NewLine}{Exception}");
        }
    }
}
