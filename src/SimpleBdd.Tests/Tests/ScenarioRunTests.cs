using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleBdd.Attributes;

namespace SimpleBdd.Tests.Tests
{
    [TestClass]
    public class ScenarioRunTests : BaseTestClass
    {
        [TestMethod]
        public void ScenarioRunsTestWithAllStatementsWithoutExceptions()
        {
            new Scenario(typeof(ScenarioRunTests))
                .Given("Given test method puts into context 10 for key 'GivenKey'")
                    .And("And given test method puts into context 'and' for key 'GivenAsAndKey'")
                    .And("And given test method puts table value from column 'Value' into context with 'TableKey' key",
                        new Table(
                            new [] { "Value" },
                            new []
                                {
                                    new object[] { 1.1 }
                                }
                            )
                        )
                .When("When test method joins 'GivenKey' and 'GivenAsAndKey' values as string to 'WhenResultKey' as result")
                    .And("And when test method concats 'TableKey' value to 'WhenResultKey'")
                .Then("Value with key 'WhenResultKey' in the result context is a string")
                    .And("Value with key 'WhenResultKey' in the result context should be equal '10and1.1' or '10and1,1'")
                .Run();
        }

        [TestMethod]
        public void ScenarioAwaitesAwaitableMethodsForAllStatements()
        {
            new Scenario(typeof(ScenarioRunTests))
                .Given("Awaitable method post time to 'Given' key and time to 'GivenAfterNSeconds' key after 3 seconds")
                .When("Awaitable method post time to 'When' key and time to 'WhenAfterNSeconds' key after 3 seconds")
                .Then("Awaitable method post time to 'Then' key and time to 'ThenAfterNSeconds' key after 3 seconds")
                    .And("Time in 'GivenAfterNSeconds' is greater or equals to time in 'Given' by 3 seconds")
                    .And("Time in 'WhenAfterNSeconds' is greater or equals to time in 'When' by 3 seconds")
                    .And("Time in 'ThenAfterNSeconds' is greater or equals to time in 'Then' by 3 seconds")
                .Run();
        }

        [Given("Awaitable method post time to '(.+)' key and time to '(.+)' key after (.+) seconds")]
        [Then("Awaitable method post time to '(.+)' key and time to '(.+)' key after (.+) seconds")]
        private static async Task GivenAwaitableMethodPostTimeToKeyAndTimeToKeyAfterSeconds(ScenarioContext context,
            string key, string secondKey, int secondToWait)
        {
            context.PutResult(DateTime.UtcNow, key);
            await Task.Delay(TimeSpan.FromSeconds(secondToWait));
            context.PutResult(DateTime.UtcNow, secondKey);
        }

        [When("Awaitable method post time to '(.+)' key and time to '(.+)' key after (.+) seconds")]
        private static async Task<int> WhenAwaitableMethodPostTimeToKeyAndTimeToKeyAfterSeconds(
            ScenarioContext context, string key, string secondKey, int secondToWait)
        {
            context.PutResult(DateTime.UtcNow, key);
            await Task.Delay(TimeSpan.FromSeconds(secondToWait));
            context.PutResult(DateTime.UtcNow, secondKey);
            return 1;
        }

        [Then("Time in '(.+)' is greater or equals to time in '(.+)' by (.+) seconds")]
        private static void TimeInIsGreaterOrEqualsToTimeInBySeconds(ScenarioContext context, string secondKey,
            string key, int expectedDifferenceInSeconds)
        {
            DateTime secondTime = context.GetResult<DateTime>(secondKey);
            DateTime firstTime = context.GetResult<DateTime>(key);

            TimeSpan difference = secondTime - firstTime;

            difference.TotalSeconds.Should().BeGreaterOrEqualTo(expectedDifferenceInSeconds);
        }

        [Given(@"Given test method puts into context (\d+) for key '(.+)'")]
        [Given(@"And given test method puts into context '(.+)' for key '(.+)'")]
        private static void GivenTestMethodPutsIntoContextValueForKey(object value, string key)
        {
            Put(value, key);
        }

        [Given(@"And given test method puts table value from column '(.+)' into context with '(.+)' key")]

        private static void GivenAndTestMethodPutsTableValueFromColumnIntoContextWithKey(Table table,
            string columnName, string key)
        {
            Put(table.Rows.First().GetValue<double>(columnName), key);
        }

        [When(@"When test method joins '(.+)' and '(.+)' values as string to '(.+)' as result")]
        private static void WhenTestMethodPutsIntoContextValueForKey(string firstKey, string secondKey,
            string resultKey)
        {
            PutResult($"{Get<object>(firstKey)}{Get<object>(secondKey)}", resultKey);
        }

        [When(@"And when test method concats '(.+)' value to '(.+)'")]
        private static void WhenAndTestMethodConcatsValueFromKeyToResultValueFromKey(string key, string resultKey)
        {
            PutResult($"{GetResult<object>(resultKey)}{Get<object>(key)}", resultKey);
        }

        [Then("Value with key '(.+)' in the result context is a string")]
        private static void ThenValueWithKeyInTheResultContextIsAString(string resultKey)
        {
            (GetResult<object>(resultKey) is string).Should().BeTrue();
        }

        [Then("Value with key '(.+)' in the result context should be equal '(.+)' or '(.+)'")]
        private static void ThenValueWithKeyInTheResultContextIsAString(string resultKey, string expectedValue,
            string alternativeExpectedValue)
        {
            string result = GetResult<string>(resultKey);

            result.Should().BeOneOf(expectedValue, alternativeExpectedValue);
        }
    }
}