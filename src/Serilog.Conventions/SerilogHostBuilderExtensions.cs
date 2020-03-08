using System;
using JetBrains.Annotations;
using Rocket.Surgery.Extensions.Serilog;
using Rocket.Surgery.Extensions.Serilog.Conventions;
using Serilog;
using Serilog.Configuration;

// ReSharper disable once CheckNamespace
namespace Rocket.Surgery.Conventions
{
    /// <summary>
    /// SerilogHostBuilderExtensions.
    /// </summary>
    [PublicAPI]
    public static class SerilogHostBuilderExtensions
    {
        /// <summary>
        /// Uses the serilog.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="options">The options.</param>
        /// <returns>IConventionHostBuilder.</returns>
        public static IConventionHostBuilder UseSerilog(
            [NotNull] this IConventionHostBuilder container,
            RocketSerilogOptions? options = null
        )
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.ServiceProperties[typeof(RocketSerilogOptions)] = options ?? new RocketSerilogOptions();
            container.Scanner.PrependConvention<SerilogExtensionsConvention>();
            container.Scanner.PrependConvention<SerilogReadFromConfigurationConvention>();
            container.Scanner.PrependConvention<SerilogEnrichLoggingConvention>();
            container.Scanner.PrependConvention<SerilogConsoleLoggingConvention>();
            container.Scanner.PrependConvention<SerilogDebugLoggingConvention>();
            container.Scanner.PrependConvention<EnvironmentLoggingConvention>();
            return container;
        }

        /// <summary>
        /// Write to the log an async sink when running the default command (or web server / hosted process).
        /// Write to a sync sink when not running the default command.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="register">The action to register the sink.</param>
        public static ISerilogConventionContext WriteToAsyncConditionally(
            [NotNull] this ISerilogConventionContext context,
            [NotNull] Action<LoggerSinkConfiguration> register
        )
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (register == null)
            {
                throw new ArgumentNullException(nameof(register));
            }

            if (ConfigurationAsyncHelper.IsAsync(context.Configuration))
            {
                context.LoggerConfiguration.WriteTo.Async(register);
            }
            else
            {
                register(context.LoggerConfiguration.WriteTo);
            }

            return context;
        }
    }
}