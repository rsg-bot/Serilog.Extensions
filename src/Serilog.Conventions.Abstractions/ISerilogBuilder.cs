using JetBrains.Annotations;
using Rocket.Surgery.Conventions;

namespace Rocket.Surgery.Extensions.Serilog
{
    /// <summary>
    /// Implements the <see cref="IConventionBuilder{TBuilder,TConvention,TDelegate}" />
    /// </summary>
    /// <seealso cref="IConventionBuilder{ISerilogBuilder, ISerilogConvention, SerilogConventionDelegate}" />
    [PublicAPI]
    public interface
        ISerilogBuilder : IConventionBuilder<ISerilogBuilder, ISerilogConvention, SerilogConventionDelegate> { }
}