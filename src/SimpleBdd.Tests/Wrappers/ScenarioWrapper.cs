using System;
using System.Reflection;
using JetBrains.Annotations;

namespace SimpleBdd.Tests.Wrappers
{
    public class ScenarioWrapper : Scenario
    {
        public TestMethod[] TestMethodList => TestMethods;


        public ScenarioWrapper([NotNull] [ItemNotNull] params Assembly[] assemblies) : base(assemblies)
        {
        }

        public ScenarioWrapper([NotNull] object assemblyHolder) : base(assemblyHolder)
        {
        }

        public ScenarioWrapper([NotNull] params Type[] typesForSearch) : base(typesForSearch)
        {
        }
    }
}