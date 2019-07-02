﻿using System;
using System.Text;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Rocket.Surgery.Conventions.Reflection;
using Rocket.Surgery.Extensions.Testing;
using Xunit;
using Xunit.Abstractions;

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
            Action a = () => { builder.PrependConvention(A.Fake<ISerilogConvention>()); };
            a.Should().NotThrow();
            a = () => { builder.PrependDelegate(delegate { }); };
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
