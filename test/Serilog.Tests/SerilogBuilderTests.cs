using System;
using System.Text;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Rocket.Surgery.Conventions.Reflection;
using Rocket.Surgery.Conventions.Scanners;
using Rocket.Surgery.Hosting;
using Serilog;
using Serilog.Core;
using Xunit;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Rocket.Surgery.Extensions.Serilog.Tests
{
    public class SerilogBuilderTests
    {
        [Fact]
        public void Constructs()
        {
            var assemblyProvider = new TestAssemblyProvider();
            var assemblyCandidateFinder = A.Fake<IAssemblyCandidateFinder>();
            var scanner = A.Fake<IConventionScanner>();
            var configuration = A.Fake<IConfiguration>();
            var builder = new SerilogBuilder(
                scanner,
                assemblyProvider,
                assemblyCandidateFinder,
                A.Fake<IHostingEnvironment>(),
                configuration,
                new LoggingLevelSwitch(),
                new LoggerConfiguration());

            builder.AssemblyProvider.Should().BeSameAs(assemblyProvider);
            builder.AssemblyCandidateFinder.Should().NotBeNull();
            builder.Configuration.Should().BeSameAs(configuration);
            builder.Environment.Should().NotBeNull();
            builder.Logger.Should().NotBeNull();
            builder.Switch.Should().NotBeNull();
            Action a = () => { builder.AddConvention(A.Fake<ISerilogConvention>()); };
            a.Should().NotThrow();
            a = () => { builder.AddDelegate(delegate { }); };
            a.Should().NotThrow();
        }

        [Fact]
        public void BuildsLogger()
        {
            var assemblyProvider = new TestAssemblyProvider();
            var assemblyCandidateFinder = A.Fake<IAssemblyCandidateFinder>();
            var scanner = A.Fake<IConventionScanner>();
            var configuration = A.Fake<IConfiguration>();
            var builder = new SerilogBuilder(
                scanner,
                assemblyProvider,
                assemblyCandidateFinder,
                A.Fake<IHostingEnvironment>(),
                configuration,
                new LoggingLevelSwitch(),
                new LoggerConfiguration());

            var seriLogger = builder.Build(A.Fake<ILogger>());
            seriLogger.Should().NotBeNull();
        }
    }
}
