using Rocket.Surgery.Conventions;

namespace Rocket.Surgery.Extensions.Serilog
{
    /// <summary>
    /// ILoggingConvention
    /// Implements the <see cref="Rocket.Surgery.Conventions.IConvention{Rocket.Surgery.Extensions.Serilog.ISerilogConventionContext}" />
    /// </summary>
    /// <seealso cref="Rocket.Surgery.Conventions.IConvention{Rocket.Surgery.Extensions.Serilog.ISerilogConventionContext}" />
    public interface ISerilogConvention : IConvention<ISerilogConventionContext> { }
}
