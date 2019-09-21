using System;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.Serilog;
using Rocket.Surgery.Extensions.Serilog.Conventions;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;

// ReSharper disable once CheckNamespace
namespace Rocket.Surgery.Conventions
{
    /// <summary>
    ///  SerilogHostBuilderExtensions.
    /// </summary>
    public static class SerilogHostBuilderExtensions
    {
        /// <summary>
        /// Uses the serilog.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="options">The options.</param>
        /// <returns>IConventionHostBuilder.</returns>
        public static IConventionHostBuilder UseSerilog(
            this IConventionHostBuilder container,
            RocketSerilogOptions? options = null)
        {
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
        public static ISerilogConventionContext WriteToAsyncConditionally(this ISerilogConventionContext context, Action<LoggerSinkConfiguration> register)
        {
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
