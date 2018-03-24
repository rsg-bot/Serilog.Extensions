using Microsoft.Extensions.Configuration;
#if NET451 || NETSTANDARD1_3
using Microsoft.Extensions.DependencyInjection;
#endif
using Rocket.Surgery.Builders;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Conventions.Reflection;
using Rocket.Surgery.Hosting;

namespace Rocket.Surgery.Extensions.Logging
{
    /// <summary>
    /// Interface ILoggingConvention
    /// </summary>
    /// TODO Edit XML Comment Template for ILoggingConvention
    public interface ILoggingBuilder : IConventionBuilder<ILoggingBuilder, ILoggingConvention, LoggingConventionDelegate>, ILoggingConventionContext    {    }
}
