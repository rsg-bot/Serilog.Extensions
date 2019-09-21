using Rocket.Surgery.Conventions;

namespace Rocket.Surgery.Extensions.Serilog
{
    /// <summary>
    /// Implements the <see cref="IConvention{ISerilogConventionContext}" />
    /// </summary>
    /// <seealso cref="IConvention{ISerilogConventionContext}" />
    public interface ISerilogConvention : IConvention<ISerilogConventionContext> { }
}
