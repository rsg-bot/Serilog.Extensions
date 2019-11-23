using JetBrains.Annotations;

namespace Rocket.Surgery.Extensions.Serilog
{
    /// <summary>
    /// Delegate SerilogConventionDelegate
    /// </summary>
    /// <param name="context">The context.</param>
    [PublicAPI] public delegate void SerilogConventionDelegate(ISerilogConventionContext context);
}