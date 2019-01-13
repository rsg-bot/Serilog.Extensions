using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rocket.Surgery.Builders;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Conventions.Reflection;
using Rocket.Surgery.Extensions.Logging;

namespace Rocket.Surgery.Extensions.Logging
{
    public static class EmpoweredLoggingExtensions
    {
        public static T UseEmpoweredLogging<T>(
            this T container,
            EmpoweredLoggingOptions options)
            where T : IConventionHostBuilder
        {
            container.AppendConvention(new LoggingServiceConvention(container.Scanner, container.DiagnosticSource, options));
            return container;
        }
    }
}
