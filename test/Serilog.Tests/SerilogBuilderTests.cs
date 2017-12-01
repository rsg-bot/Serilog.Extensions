using System;
using System.Text;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Rocket.Surgery.Conventions.Reflection;
using Rocket.Surgery.Conventions.Scanners;
using Rocket.Surgery.Extensions.Testing;
using Rocket.Surgery.Hosting;
using Serilog;
using Serilog.Core;
using Xunit;
using Xunit.Abstractions;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Rocket.Surgery.Extensions.Serilog.Tests
{
    public class SerilogBuilderTests : AutoTestBase
    {
        public SerilogBuilderTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }

        [Fact]
        public void Constructs()
        {
            var assemblyProvider = AutoFake.Provide<IAssemblyProvider>(new TestAssemblyProvider());
            var builder = AutoFake.Resolve<SerilogBuilder>();

            builder.AssemblyProvider.Should().BeSameAs(assemblyProvider);
            builder.AssemblyCandidateFinder.Should().NotBeNull();
            builder.Configuration.Should().NotBeNull();
            builder.Environment.Should().NotBeNull();
            builder.LoggerConfiguration.Should().NotBeNull();
            builder.Switch.Should().NotBeNull();
            Action a = () => { builder.AddConvention(A.Fake<ISerilogConvention>()); };
            a.Should().NotThrow();
            a = () => { builder.AddDelegate(delegate { }); };
            a.Should().NotThrow();
        }

        [Fact]
        public void BuildsLogger()
        {
            AutoFake.Provide<IAssemblyProvider>(new TestAssemblyProvider());
            var builder = AutoFake.Resolve<SerilogBuilder>();

            var seriLogger = builder.Build();
            seriLogger.Should().NotBeNull();
        }
    }
}
