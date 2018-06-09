using Microsoft.Extensions.Configuration;
using Rocket.Surgery.Builders;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Conventions.Reflection;

namespace Rocket.Surgery.Extensions.Logging
{
    /// <summary>
    /// Interface ILoggingConvention
    /// </summary>
    /// TODO Edit XML Comment Template for ILoggingConvention
    public interface ILoggingBuilder : IConventionBuilder<ILoggingBuilder, ILoggingConvention, LoggingConventionDelegate> { }
}
