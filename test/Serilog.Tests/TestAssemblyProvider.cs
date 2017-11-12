using System.Collections.Generic;
using System.Reflection;
using Rocket.Surgery.Conventions.Reflection;

namespace Rocket.Surgery.Extensions.Serilog.Tests
{
    class TestAssemblyProvider : IAssemblyProvider
    {
        public IEnumerable<Assembly> GetAssemblies()
        {
            return new[]
            {
                typeof(SerilogBuilder).GetTypeInfo().Assembly,
                typeof(TestAssemblyProvider).GetTypeInfo().Assembly
            };
        }
    }
}