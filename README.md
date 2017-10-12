# Cucumber-like bdd tests
Simple cucumber-like test framework to use on Universal Windows App platform or any other project because of .net standard 1.1 as target

The goal of this project is to create some simple BDD framework.
The reasons:

1. The SpecFlow team isn't going to support UWP. At least now. They might in future but I need something for now. I didn't find anything usable
2. Try and learn some features and .net standard project
3. Just for fun

Unfortunately Microsoft Universal Windows Platform has poor support of unit testing at all. There are some restrictions. So as result this solution is also going to have restrictions. But in any case I'll try to do it in the handiest and usable way.

In this solution I'm goint to use Gerkins-like syntax. If you don't know what it means then google it :)

## Examples of usage

Can be found [here](https://github.com/AndreiShenets/simple-bdd/blob/master/src/SimpleBdd.Tests/Tests/ScenarioRunTests.cs)

```csharp
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
                .And("Value with key 'WhenResultKey' in the result context should be equal '10and1.1'")
            .Run();
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

    [Then("Value with key '(.+)' in the result context should be equal '(.+)'")]
    private static void ThenValueWithKeyInTheResultContextIsAString(string resultKey, string expectedValue)
    {
        GetResult<string>(resultKey).Should().Be(expectedValue);
    }
}
```

```csharp
[TestClass]
public class ScenarioRunTests
{
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
}
```

## ScenarioContext class restrictions
ScenarioContext.Current is thread safe but it is shared between scenarios. So it is quite possible that when you create some sequential tests you can override values or see unexpected values in current context. So please *clear* context before running scenario if you use ScenarioContext.Current and see some strange behavior.

As alternative you can declare ScenarioContext parameter in your test method and ScenarioContext will be passed to you method. At this time ScenarioContext is shared only between methods of your scenario.

## Tool to convert scenarios to methods

There is a simple tool to convert scenarios to test methods. Clone solution, build and check.


## Example of alternative way of using

Create a scenario class like this

```csharp
public partial class Scenario
{
    private readonly ScenarioContext _scenarioContext = new ScenarioContext();

    public Scenario Given => this;
    public Scenario When => this;
    public Scenario Then => this;
    public Scenario And => this;
    public Scenario Arrange => this;
    public Scenario Act => this;
    public Scenario Assert => this;


    protected void Put<T>(T instance, string key = null, int? instanceIndex = null)
    {
        _scenarioContext.Put(instance, key, instanceIndex);
    }

    protected void PutResult<T>(T instance, string key = null, int? instanceIndex = null)
    {
        _scenarioContext.PutResult(instance, key, instanceIndex);
    }

    protected T Get<T>(string key = null, int? instanceIndex = null)
    {
        return _scenarioContext.Get<T>(key, instanceIndex);
    }

    protected T GetResult<T>(string key = null, int? instanceIndex = null)
    {
        return _scenarioContext.GetResult<T>(key, instanceIndex);
    }
}
```

In a second file(s) create another class(es) like this

```csharp
public partial class Scenario
{
    public Scenario Do_some_initialization()
    {
        Put<string>("Initialization", "Key");

        return this;
    }

    public Scenario Do_other_initialization()
    {
        Put<string>("OtherInitialization", "OtherKey");

        return this;
    }

    public Scenario Do_some_useful_work()
    {
        PutResult<string>(Get<string>("Key").ToUpper(), "ResultKey");

        return this;
    }

    public Scenario Do_some_other_useful_work()
    {
        PutResult<string>(Get<string>("OtherKey").ToUpper(), "OtherResultKey");

        return this;
    }

    public Scenario Do_some_checks()
    {
        GetResult<string>("ResultKey")
            .Should()
            .Be("INITIALIZATION");

        return this;
    }

    public Scenario Do_other_checks()
    {
        GetResult<string>("OtherResultKey")
            .Should()
            .Be("OTHERINITIALIZATION");

        return this;
    }
}
```

And example of usage is

```csharp
[TestFixture]
public class MyTests
{
    [Test]
    public void Test()
    {
        new Scenario()
            .Given.Do_some_initialization()
                .And.Do_other_initialization()
            .When.Do_some_useful_work()
                .And.Do_some_other_useful_work()
            .Then.Do_some_checks()
                .And.Do_other_checks();
    }
}
```
