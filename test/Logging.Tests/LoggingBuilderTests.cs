using System;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rocket.Surgery.Conventions.Reflection;
using Rocket.Surgery.Conventions.Scanners;
using Rocket.Surgery.Hosting;
using Xunit;

namespace Rocket.Surgery.Extensions.Logging.Tests
{
    public class LoggingBuilderTests
    {
        [Fact]
        public void Constructs()
        {
            var assemblyProvider = new TestAssemblyProvider();
            var assemblyCandidateFinder = A.Fake<IAssemblyCandidateFinder>();
            var servicesCollection = A.Fake<IServiceCollection>();
            var scanner = A.Fake<IConventionScanner>();
            var configuration = A.Fake<IConfiguration>();
            var builder = new LoggingBuilder(
                scanner,
                assemblyProvider,
                assemblyCandidateFinder,
                servicesCollection,
                A.Fake<IHostingEnvironment>(),
                configuration);

            builder.AssemblyProvider.Should().BeSameAs(assemblyProvider);
            builder.AssemblyCandidateFinder.Should().NotBeNull();
            builder.Services.Should().NotBeNull();
            builder.Configuration.Should().BeSameAs(configuration);
            builder.Environment.Should().NotBeNull();
            Action a = () => { builder.AddConvention(A.Fake<ILoggingConvention>()); };
            a.Should().NotThrow();
            a = () => { builder.AddDelegate(delegate { }); };
            a.Should().NotThrow();
        }

        [Fact]
        public void BuildsALogger()
        {
            var assemblyProvider = new TestAssemblyProvider();
            var assemblyCandidateFinder = A.Fake<IAssemblyCandidateFinder>();
            var servicesCollection = A.Fake<IServiceCollection>();
            var scanner = A.Fake<IConventionScanner>();
            var configuration = A.Fake<IConfiguration>();
            var builder = new LoggingBuilder(
                scanner,
                assemblyProvider,
                assemblyCandidateFinder,
                servicesCollection,
                A.Fake<IHostingEnvironment>(),
                configuration);

            Action a = () => builder.Build(A.Fake<ILogger>());
            a.Should().NotThrow();
        }
    }
}