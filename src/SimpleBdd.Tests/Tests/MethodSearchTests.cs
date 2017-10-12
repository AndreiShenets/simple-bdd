using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleBdd.Exceptions;
using SimpleBdd.Tests.TestObjects;
using SimpleBdd.Tests.Wrappers;

namespace SimpleBdd.Tests.Tests
{
    [TestClass]
    public class MethodSearchTests : BaseTestClass
    {
        [TestMethod]
        public void GivenMethodFoundForRegExPattern()
        {
            ScenarioWrapper scenarioWrapper = new ScenarioWrapper(typeof(MethodSearchTestsClass));

            Action action = () =>
                {
                    scenarioWrapper.Given("GivenStaticMethodSearchTestsClass");
                };

            action.ShouldNotThrow();
        }

        [TestMethod]
        public void ExceptionThrownWhenAmbiguousGivenMethodFound()
        {
            ScenarioWrapper scenarioWrapper = new ScenarioWrapper(typeof(MethodSearchTestsClass));

            Action action = () =>
                {
                    scenarioWrapper.Given("GivenStaticMethodSearchTestsClassDuplicated");
                };

            action.ShouldThrow<AmbiguousTestMethodFoundException>();
        }

        [TestMethod]
        public void WhenMethodFoundForRegExPattern()
        {
            ScenarioWrapper scenarioWrapper = new ScenarioWrapper(typeof(MethodSearchTestsClass));

            Action action = () =>
            {
                scenarioWrapper.When("WhenStaticMethodSearchTestsClass");
            };

            action.ShouldNotThrow();
        }

        [TestMethod]
        public void ExceptionThrownWhenAmbiguousWhenMethodFound()
        {
            ScenarioWrapper scenarioWrapper = new ScenarioWrapper(typeof(MethodSearchTestsClass));

            Action action = () =>
                {
                    scenarioWrapper.When("WhenStaticMethodSearchTestsClassDuplicate");
                };

            action.ShouldThrow<AmbiguousTestMethodFoundException>();

        }

        [TestMethod]
        public void ThenMethodFoundForRegExPattern()
        {
            ScenarioWrapper scenarioWrapper = new ScenarioWrapper(typeof(MethodSearchTestsClass));


            Action action = () =>
                {
                    scenarioWrapper.Then("ThenStaticMethodSearchTestsClass");
                };

            action.ShouldNotThrow();
        }

        [TestMethod]
        public void ExceptionThrownWhenAmbiguousThenMethodFound()
        {
            ScenarioWrapper scenarioWrapper = new ScenarioWrapper(typeof(MethodSearchTestsClass));

            Action action = () =>
                {
                    scenarioWrapper.Then("ThenStaticMethodSearchTestsClassDuplicate");
                };

            action.ShouldThrow<AmbiguousTestMethodFoundException>();
        }

        [TestMethod]
        public void ExceptionThrownIfMethodNotFound()
        {
            ScenarioWrapper scenarioWrapper = new ScenarioWrapper(typeof(MethodSearchTestsClass));

            Action action = () =>
                {
                    scenarioWrapper.Given("NonExistingGivenMethod");
                };

            action.ShouldThrow<TestMethodNotFoundException>();

            action = () =>
                {
                    scenarioWrapper.When("NonExistingWhenMethod");
                };

            action.ShouldThrow<TestMethodNotFoundException>();

            action = () =>
                {
                    scenarioWrapper.Then("NonExistingThenMethod");
                };

            action.ShouldThrow<TestMethodNotFoundException>();
        }
    }
}