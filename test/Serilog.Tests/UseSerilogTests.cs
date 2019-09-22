using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Conventions.Reflection;
using Rocket.Surgery.Conventions.Scanners;
using Rocket.Surgery.Extensions.DependencyInjection;
using Rocket.Surgery.Extensions.Logging;
using Rocket.Surgery.Extensions.Testing;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Rocket.Surgery.Extensions.Serilog.Tests
{
    public class UseSerilogTests : AutoTestBase
    {
        public UseSerilogTests(ITestOutputHelper outputHelper) : base(outputHelper, LogLevel.Trace)
        {
        }

        class HostBuilder : ConventionHostBuilder<HostBuilder>
        {
            public HostBuilder(IConventionScanner scanner, IAssemblyCandidateFinder assemblyCandidateFinder, IAssemblyProvider assemblyProvider, DiagnosticSource diagnosticSource, IServiceProviderDictionary serviceProperties) : base(scanner, assemblyCandidateFinder, assemblyProvider, diagnosticSource, serviceProperties)
            {
            }
        }


        [Fact]
        public void AddsAsAConvention()
        {
            var properties = new ServiceProviderDictionary();
            AutoFake.Provide<IServiceProviderDictionary>(properties);
            AutoFake.Provide<IDictionary<object, object?>>(properties);
            AutoFake.Provide<IServiceProvider>(properties);
            var configuration = new ConfigurationBuilder().Build();
            AutoFake.Provide<IConfiguration>(configuration);
            var finder = AutoFake.Resolve<IAssemblyCandidateFinder>();

            A.CallTo(() => finder.GetCandidateAssemblies(A<IEnumerable<string>>._))
                .Returns(new[] { typeof(LoggingServiceConvention).Assembly, typeof(SerilogHostBuilderExtensions).Assembly });

            properties[typeof(ILogger)] = Logger;
            var scanner = AutoFake.Resolve<SimpleConventionScanner>();
            AutoFake.Provide<IConventionScanner>(scanner);
            var services = new ServiceCollection();
            AutoFake.Provide<IServiceCollection>(services);

            var builder = AutoFake.Resolve<HostBuilder>();
            var sb = AutoFake.Resolve<ServicesBuilder>();
            sb.Services.AddLogging(x => x.AddConsole().AddDebug());

            var sp = sb.Build();

            sp.GetService<ILoggerFactory>().Should().NotBeNull().And.BeOfType<SerilogLoggerFactory>();
        }

        [Fact]
        public void AddsLogging()
        {
            var properties = new ServiceProviderDictionary();
            AutoFake.Provide<IServiceProviderDictionary>(properties);
            AutoFake.Provide<IDictionary<object, object?>>(properties);
            AutoFake.Provide<IServiceProvider>(properties);
            var configuration = new ConfigurationBuilder().Build();
            AutoFake.Provide<IConfiguration>(configuration);

            properties[typeof(ILogger)] = Logger;
            var scanner = AutoFake.Resolve<SimpleConventionScanner>();
            AutoFake.Provide<IConventionScanner>(scanner);
            var services = new ServiceCollection();
            AutoFake.Provide<IServiceCollection>(services);

            var builder = AutoFake.Resolve<HostBuilder>();
            var sb = AutoFake.Resolve<ServicesBuilder>();
            sb.Services.AddLogging(x => x.AddConsole().AddDebug());

            Func<ILoggingConventionContext, LogLevel?> @delegate = x => LogLevel.Error;

            builder.UseLogging(new RocketLoggingOptions() { GetLogLevel = @delegate }).UseSerilog();

            var sp = sb.Build();

            sp.GetService<ILoggerFactory>().Should().NotBeNull().And.BeOfType<SerilogLoggerFactory>();
        }

#if false
        [Fact]
        public async Task FlushesWhenApplicationShutsdown()
        {
            var properties = new ServiceProviderDictionary();
            AutoFake.Provide<IServiceProviderDictionary>(properties);
            AutoFake.Provide<IDictionary<object, object?>>(properties);
            AutoFake.Provide<IServiceProvider>(properties);
            var configuration = new ConfigurationBuilder().Build();
            AutoFake.Provide<IConfiguration>(configuration);
            var finder = AutoFake.Resolve<IAssemblyCandidateFinder>();

            A.CallTo(() => finder.GetCandidateAssemblies(A<IEnumerable<string>>._))
                .Returns(new[] { typeof(LoggingServiceConvention).Assembly, typeof(RequestLoggingSerilogExtensions).Assembly, typeof(SerilogHostBuilderExtensions).Assembly });

            properties[typeof(ILogger)] = Logger;
            var scanner = AutoFake.Resolve<SimpleConventionScanner>();
            AutoFake.Provide<IConventionScanner>(scanner);

            var hostB = Host.CreateDefaultBuilder();
            hostB.ConfigureServices(services =>
                {
                    AutoFake.Provide(services);
                })
                .UseServiceProviderFactory(c =>
                {
                    var serviceProviderFactory = A.Fake<IServiceProviderFactory<IServiceCollection>>();
                    A.CallTo(() => serviceProviderFactory.CreateBuilder(A<IServiceCollection>._))
                        .Returns(AutoFake.Resolve<IServiceCollection>());
                    A.CallTo(() => serviceProviderFactory.CreateServiceProvider(A<IServiceCollection>._))
                        .Returns(AutoFake.Resolve<ServicesBuilder>().Build());
                    return serviceProviderFactory;
                });

            global::Serilog.ILogger logger;

            using (var host = hostB.Build())
            {
                await host.StartAsync();
                logger = host.Services.GetRequiredService<global::Serilog.ILogger>();
                await host.StopAsync();
            }

            Log.Logger.Should().NotBe(logger);
        }
#endif
    }
}
