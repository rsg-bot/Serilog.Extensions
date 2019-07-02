using Microsoft.Extensions.Configuration;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Conventions.Reflection;
using Serilog;
using Serilog.Core;

namespace Rocket.Surgery.Extensions.Serilog
{
    /// <summary>
    /// ILoggingConvention
    /// Implements the <see cref="Rocket.Surgery.Conventions.IConventionBuilder{Rocket.Surgery.Extensions.Serilog.ISerilogBuilder, Rocket.Surgery.Extensions.Serilog.ISerilogConvention, Rocket.Surgery.Extensions.Serilog.SerilogConventionDelegate}" />
    /// </summary>
    /// <seealso cref="Rocket.Surgery.Conventions.IConventionBuilder{Rocket.Surgery.Extensions.Serilog.ISerilogBuilder, Rocket.Surgery.Extensions.Serilog.ISerilogConvention, Rocket.Surgery.Extensions.Serilog.SerilogConventionDelegate}" />
    public interface ISerilogBuilder : IConventionBuilder<ISerilogBuilder, ISerilogConvention, SerilogConventionDelegate> { }
}
