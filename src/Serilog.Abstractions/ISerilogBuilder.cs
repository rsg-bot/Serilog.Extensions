using Microsoft.Extensions.Configuration;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Conventions.Reflection;
using Serilog;
using Serilog.Core;

namespace Rocket.Surgery.Extensions.Serilog
{
    /// <summary>
    /// Implements the <see cref="IConventionBuilder{ISerilogBuilder, ISerilogConvention, SerilogConventionDelegate}" />
    /// </summary>
    /// <seealso cref="IConventionBuilder{ISerilogBuilder, ISerilogConvention, SerilogConventionDelegate}" />
    public interface ISerilogBuilder : IConventionBuilder<ISerilogBuilder, ISerilogConvention, SerilogConventionDelegate> { }
}
