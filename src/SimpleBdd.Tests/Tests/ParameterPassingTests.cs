using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleBdd.Tests.TestObjects;
using SimpleBdd.Tests.Wrappers;

namespace SimpleBdd.Tests.Tests
{
    [TestClass]
    public class ParameterPassingTests : BaseTestClass
    {
        [TestMethod]
        public void IntBoolDoubleTableStringParametersHavePassedCorrectly()
        {
            ScenarioWrapper scenarioWrapper = new ScenarioWrapper(typeof(MethodSearchTestsClass));

            int expectedIntParam = 100;
            bool expectedBoolParam = true;
            double expectedDoubleParam = 10.10;
            string expectedStringParam = "testString";
            string expectedTableValue = "Value";

            Action action = () =>
                {
                    scenarioWrapper
                        .Given(
                            $"Int={expectedIntParam};Bool={expectedBoolParam};" +
                                $"Double={expectedDoubleParam};Table;String={expectedStringParam};ParametersMethod",
                            new Table(
                                new [] { "Column" },
                                new object[][]
                                    {
                                        new [] { expectedTableValue }
                                    }
                            ))
                        .Run();
                };

            action.ShouldNotThrow();
            GetResult<bool>("IntBoolDoubleTableStringParametersMethod").Should().BeTrue();
            GetResult<int>("intParam").Should().Be(expectedIntParam);
            GetResult<bool>("boolParam").Should().Be(expectedBoolParam);
            GetResult<double>("doubleParam").Should().Be(expectedDoubleParam);
            GetResult<string>("stringParam").Should().Be(expectedStringParam);
            GetResult<string>("tableColumnValue").Should().Be(expectedTableValue);
        }
    }
}