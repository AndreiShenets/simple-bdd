using System.Linq;
using SimpleBdd.Attributes;

namespace SimpleBdd.Tests.TestObjects
{
    public class MethodSearchTestsClass
    {
        [Given("GivenStaticMethodSearchTestsClass")]
        public static void GivenStaticMethodSearchTestsClass()
        {
            ScenarioContext.Current.PutResult(true, "GivenStaticMethodSearchTestsClass");
        }

        [When("GivenStaticMethodSearchTestsClass")]
        public static void WhenFakeGivenStaticMethodSearchTestsClass()
        {
            ScenarioContext.Current.PutResult(true, "GivenStaticMethodSearchTestsClass");
        }

        [Given("GivenStaticMethodSearchTestsClassDuplicated")]
        public static void GivenStaticMethodSearchTestsClassDuplicate1()
        {
            ScenarioContext.Current.PutResult(true, "GivenStaticMethodSearchTestsClass");
        }

        [Given("GivenStaticMethodSearchTestsClassDuplicated")]
        public static void GivenStaticMethodSearchTestsClassDuplicate2()
        {
            ScenarioContext.Current.PutResult(true, "GivenStaticMethodSearchTestsClass");
        }

        [Given("GivenSecondStaticMethodSearchTestsClass")]
        public static void GivenSecondStaticMethodSearchTestsClass()
        {
        }

        [When("WhenStaticMethodSearchTestsClass")]
        public static void WhenStaticMethodSearchTestsClass()
        {
            ScenarioContext.Current.PutResult(true, "WhenStaticMethodSearchTestsClass");
        }

        [Then("WhenStaticMethodSearchTestsClass")]
        public static void ThenFakeWhenStaticMethodSearchTestsClass()
        {
            ScenarioContext.Current.PutResult(true, "WhenStaticMethodSearchTestsClass");
        }

        [When("WhenStaticMethodSearchTestsClassDuplicate")]
        public static void WhenStaticMethodSearchTestsClass1()
        {
            ScenarioContext.Current.PutResult(true, "WhenStaticMethodSearchTestsClass");
        }

        [When("WhenStaticMethodSearchTestsClassDuplicate")]
        public static void WhenStaticMethodSearchTestsClass2()
        {
            ScenarioContext.Current.PutResult(true, "WhenStaticMethodSearchTestsClass");
        }

        [When("WhenSecondStaticMethodSearchTestsClass")]
        public static void WhenSecondStaticMethodSearchTestsClass()
        {
        }

        [Then("ThenStaticMethodSearchTestsClass")]
        public static void ThenStaticMethodSearchTestsClass()
        {
            ScenarioContext.Current.PutResult(true, "ThenStaticMethodSearchTestsClass");
        }

        [When("ThenStaticMethodSearchTestsClass")]
        public static void WhenFakeThenStaticMethodSearchTestsClass()
        {
            ScenarioContext.Current.PutResult(true, "ThenStaticMethodSearchTestsClass");
        }

        [Then("ThenStaticMethodSearchTestsClassDuplicated")]
        public static void ThenStaticMethodSearchTestsClassDuplicate1()
        {
            ScenarioContext.Current.PutResult(true, "ThenStaticMethodSearchTestsClass");
        }

        [Then("ThenStaticMethodSearchTestsClassDuplicate")]
        public static void ThenStaticMethodSearchTestsClassDuplicate2()
        {
            ScenarioContext.Current.PutResult(true, "ThenStaticMethodSearchTestsClass");
        }

        [Then("ThenSecondStaticMethodSearchTestsClass")]
        public static void ThenSecondStaticMethodSearchTestsClass()
        {
        }

        [Given("NonExistingWhenMethod")]
        public void GivenNonExistingWhenMethod()
        {
        }

        [Given("NonExistingThenMethod")]
        public void GivenNonExistingThenMethod()
        {
        }

        [When("NonExistingGivenMethod")]
        public void WhenNonExistingGivenMethod()
        {
        }

        [When("NonExistingThenMethod")]
        public void WhenNonExistingThenMethod()
        {
        }

        [Then("NonExistingGivenMethod")]
        public void ThenNonExistingGivenMethod()
        {
        }

        [Then("NonExistingWhenMethod")]
        public void ThenNonExistingWhenMethod()
        {
        }

        [Given("Int=(\\d+);Bool=(.+?);Double=(.+?);Table;String=(.+?);ParametersMethod")]
        public static void IntBoolDoubleTableStringParametersMethod(int intParam, Table table, bool boolParam, double doubleParam,
            string stringParam)
        {
            ScenarioContext.Current.PutResult(true, "IntBoolDoubleTableStringParametersMethod");

            ScenarioContext.Current.PutResult(intParam, nameof(intParam));
            ScenarioContext.Current.PutResult(boolParam, nameof(boolParam));
            ScenarioContext.Current.PutResult(doubleParam, nameof(doubleParam));
            ScenarioContext.Current.PutResult(stringParam, nameof(stringParam));
            ScenarioContext.Current.PutResult(table.Rows.First().GetValue<string>("Column"), "tableColumnValue");
        }
    }
}