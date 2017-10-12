using System;
using System.Reflection;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleBdd.Tests.Wrappers;
using TestAssembly;

namespace SimpleBdd.Tests.Tests
{
    [TestClass]
    public class ScenarioBuidlerTests
    {
        [TestMethod]
        public void ScenarioCollectsTestMethodsFromAssemblyHolder()
        {
            ScenarioWrapper scenarioWrapper = new ScenarioWrapper(this);

            string[] shouldContainMethods = {
                    "GivenStaticClass",
                    "WhenStaticClass",
                    "ThenStaticClass",
                    "GivenClassStaticMethod",
                    "WhenClassStaticMethod",
                    "ThenClassStaticMethod",
                    "GivenClassProtectedStaticMethod",
                    "WhenClassProtectedStaticMethod",
                    "ThenClassProtectedStaticMethod",
                    "GivenClassPrivateStaticMethod",
                    "WhenClassPrivateStaticMethod",
                    "ThenClassPrivateStaticMethod"
                };

            foreach (string shouldContainMethod in shouldContainMethods)
            {
                scenarioWrapper.TestMethodList.Should().Contain(method =>
                    MethodCheck(method, GetMethodTypeFromName(shouldContainMethod), shouldContainMethod));
            }

            string[] shouldNotContainMethods = {
                    "GivenClass",
                    "WhenClass",
                    "ThenClass",
                    "GivenClassProtectedMethod",
                    "WhenClassProtectedMethod",
                    "ThenClassProtectedMethod",
                    "GivenClassPrivateMethod",
                    "WhenClassPrivateMethod",
                    "ThenClassPrivateMethod"
                };

            foreach (string shouldNotContainMethod in shouldNotContainMethods)
            {
                scenarioWrapper.TestMethodList.Should().NotContain(method =>
                    MethodCheck(method, GetMethodTypeFromName(shouldNotContainMethod), shouldNotContainMethod));
            }
        }

        [TestMethod]
        public void ScenarioCollectsTestMethodsFromPassedAssembly()
        {
            ScenarioWrapper scenarioWrapper =
                new ScenarioWrapper(
                    typeof(AbstractClassOtherAssembly).GetTypeInfo().Assembly
                );

            string[] shouldContainMethods = {
                    "GivenStaticClassOtherAssembly",
                    "WhenStaticClassOtherAssembly",
                    "ThenStaticClassOtherAssembly",
                    "GivenClassOtherAssemblyStaticMethod",
                    "WhenClassOtherAssemblyStaticMethod",
                    "ThenClassOtherAssemblyStaticMethod",
                    "GivenClassOtherAssemblyProtectedStaticMethod",
                    "WhenClassOtherAssemblyProtectedStaticMethod",
                    "ThenClassOtherAssemblyProtectedStaticMethod",
                    "GivenClassOtherAssemblyPrivateStaticMethod",
                    "WhenClassOtherAssemblyPrivateStaticMethod",
                    "ThenClassOtherAssemblyPrivateStaticMethod"
                };

            foreach (string shouldContainMethod in shouldContainMethods)
            {
                scenarioWrapper.TestMethodList.Should().Contain(method =>
                    MethodCheck(method, GetMethodTypeFromName(shouldContainMethod), shouldContainMethod));
            }

            string[] shouldNotContainMethods = {
                    "GivenClassOtherAssembly",
                    "WhenClassOtherAssembly",
                    "ThenClassOtherAssembly",
                    "GivenClassOtherAssemblyProtectedMethod",
                    "WhenClassOtherAssemblyProtectedMethod",
                    "ThenClassOtherAssemblyProtectedMethod",
                    "GivenClassOtherAssemblyPrivateMethod",
                    "WhenClassOtherAssemblyPrivateMethod",
                    "ThenClassOtherAssemblyPrivateMethod",
                    "GivenAbstractClassOtherAssemblyAbstractMethod",
                    "WhenAbstractClassOtherAssemblyAbstractMethod",
                    "ThenAbstractClassOtherAssemblyAbstractMethod"
                };

            foreach (string shouldNotContainMethod in shouldNotContainMethods)
            {
                scenarioWrapper.TestMethodList.Should().NotContain(method =>
                    MethodCheck(method, GetMethodTypeFromName(shouldNotContainMethod), shouldNotContainMethod));
            }
        }

        [TestMethod]
        public void ScenarioCollectsTestMethodsFromPassedSignleType()
        {
            ScenarioWrapper scenarioWrapper = new ScenarioWrapper(typeof(StaticTestClassOtherAssembly));

            string[] shouldContainMethods = {
                    "GivenStaticClassOtherAssembly",
                    "WhenStaticClassOtherAssembly",
                    "ThenStaticClassOtherAssembly"
                };

            foreach (string shouldContainMethod in shouldContainMethods)
            {
                scenarioWrapper.TestMethodList.Should().Contain(method =>
                    MethodCheck(method, GetMethodTypeFromName(shouldContainMethod), shouldContainMethod));
            }

            string[] shouldNotContainMethods = {
                    "GivenClassOtherAssembly",
                    "WhenClassOtherAssembly",
                    "ThenClassOtherAssembly",
                    "GivenClassOtherAssemblyProtectedMethod",
                    "WhenClassOtherAssemblyProtectedMethod",
                    "ThenClassOtherAssemblyProtectedMethod",
                    "GivenClassOtherAssemblyPrivateMethod",
                    "WhenClassOtherAssemblyPrivateMethod",
                    "ThenClassOtherAssemblyPrivateMethod",
                    "GivenAbstractClassOtherAssemblyAbstractMethod",
                    "WhenAbstractClassOtherAssemblyAbstractMethod",
                    "ThenAbstractClassOtherAssemblyAbstractMethod",
                    "GivenClassOtherAssemblyStaticMethod",
                    "WhenClassOtherAssemblyStaticMethod",
                    "ThenClassOtherAssemblyStaticMethod",
                    "GivenClassOtherAssemblyProtectedStaticMethod",
                    "WhenClassOtherAssemblyProtectedStaticMethod",
                    "ThenClassOtherAssemblyProtectedStaticMethod",
                    "GivenClassOtherAssemblyPrivateStaticMethod",
                    "WhenClassOtherAssemblyPrivateStaticMethod",
                    "ThenClassOtherAssemblyPrivateStaticMethod"
                };

            foreach (string shouldNotContainMethod in shouldNotContainMethods)
            {
                scenarioWrapper.TestMethodList.Should().NotContain(method =>
                    MethodCheck(method, GetMethodTypeFromName(shouldNotContainMethod), shouldNotContainMethod));
            }
        }

        private TestMethodType GetMethodTypeFromName([NotNull] string expectedMethodName)
        {
            if (expectedMethodName.StartsWith("Given"))
            {
                return TestMethodType.Given;
            }

            if (expectedMethodName.StartsWith("When"))
            {
                return TestMethodType.When;
            }

            if (expectedMethodName.StartsWith("Then"))
            {
                return TestMethodType.Then;
            }

            throw new ArgumentOutOfRangeException();
        }

        private bool MethodCheck(TestMethod method, TestMethodType expectedMethodType,
            string expectedMethodName)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            return method.MethodInfo != null
                && method.MatchingPattern != null
                && method.TestMethodType == expectedMethodType
                && string.Equals(method.MethodInfo.Name, expectedMethodName, StringComparison.Ordinal);
        }
    }
}
