using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.Configuration;
using Rocket.Surgery.Extensions.Serilog.Conventions;
using Serilog;
using Serilog.Extensions.Logging;

[assembly: Convention(typeof(SerilogReadFromConfigurationConvention))]

namespace Rocket.Surgery.Extensions.Serilog.Conventions
{
    /// <summary>
    /// SerilogReadFromConfigurationConvention.
    /// Implements the <see cref="ISerilogConvention" />
    /// </summary>
    /// <seealso cref="ISerilogConvention" />
    public class SerilogReadFromConfigurationConvention : ISerilogConvention, IConfigurationConvention
    {
        /// <inheritdoc />
        public void Register([NotNull] IConfigurationConventionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var applicationLogLevel = context.Configuration.GetValue<LogLevel?>("ApplicationState:LogLevel");
            if (applicationLogLevel.HasValue)
            {
                context.AddInMemoryCollection(
                    new Dictionary<string, string>
                    {
                        {
                            "Serilog:MinimumLevel:Default",
                            LevelConvert.ToSerilogLevel(applicationLogLevel.Value).ToString()
                        }
                    }
                );
            }
        }

        /// <inheritdoc />
        public void Register([NotNull] ISerilogConventionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.LoggerConfiguration.ReadFrom.Configuration(context.Configuration);
        }
    }
}