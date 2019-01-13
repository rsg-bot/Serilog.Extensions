using Rocket.Surgery.Extensions.Serilog.Conventions;

namespace Rocket.Surgery.Extensions.Serilog.Empowered
{
    public static class EmpoweredSerilogBuildExtensions
    {
        public static ISerilogBuilder UseEmpoweredSerilog(this ISerilogBuilder builder, bool async = true)
        {
            builder.AppendConvention(new EnvironmentLoggingConvention());
            builder.AppendConvention(new SerilogConsoleLoggingConvention(async));
            builder.AppendConvention(new SerilogDebugLoggingConvention(async));
            builder.AppendConvention(new SerilogEnrichLoggingConvention());
            return builder;
        }
    }
}
