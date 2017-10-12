using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleBdd.Tests.Tests
{
    [TestClass]
    public class ScenarioConverterTests
    {
        [TestMethod]
        public void ScenarioIsParsedCorrectly()
        {
            const string SCENARIO =
@"            new Scenario(typeof(ScenarioRunTests))
                .Given(""Given test method puts into context 10 for key 'GivenKey'"")
                    .And(""And given test method puts into context 'and' for key 'GivenAsAndKey'"")
                    .And(""And given test method puts table value from column 'Value' into context with 'TableKey' key"",
                        new Table(
                            new [] { ""Value"" },
                            new []
                                {
                                    new object[] { 1.1 }
                                }
                            )
                        )
                .When(""When test method joins 'GivenKey' and 'GivenAsAndKey' values as string to 'WhenResultKey' as result"")
                    .And(""And when test method concats 'TableKey' value to 'WhenResultKey'"")
                .Then(""Value with key 'WhenResultKey' in the result context is a string"")
                    .And(""Value with key 'WhenResultKey' in the result context should be equal '10and1.1'"")
                .Run();";

            string result = ScenarioToCodeConverter.Convert(SCENARIO);

            const string EXPECTED_RESULT =
                @"[Given(""Given test method puts into context 10 for key 'GivenKey'"")]
public static void GivenGivenTestMethodPutsIntoContext10ForKeyGivenKey()
{
    throw new NotImplementedException();
}

[Given(""And given test method puts into context 'and' for key 'GivenAsAndKey'"")]
public static void GivenAndGivenTestMethodPutsIntoContextAndForKeyGivenAsAndKey()
{
    throw new NotImplementedException();
}

[Given(""And given test method puts table value from column 'Value' into context with 'TableKey' key"")]
public static void GivenAndGivenTestMethodPutsTableValueFromColumnValueIntoContextWithTableKeyKey()
{
    throw new NotImplementedException();
}

[When(""When test method joins 'GivenKey' and 'GivenAsAndKey' values as string to 'WhenResultKey' as result"")]
public static void WhenWhenTestMethodJoinsGivenKeyAndGivenAsAndKeyValuesAsStringToWhenResultKeyAsResult()
{
    throw new NotImplementedException();
}

[When(""And when test method concats 'TableKey' value to 'WhenResultKey'"")]
public static void WhenAndWhenTestMethodConcatsTableKeyValueToWhenResultKey()
{
    throw new NotImplementedException();
}

[Then(""Value with key 'WhenResultKey' in the result context is a string"")]
public static void ThenValueWithKeyWhenResultKeyInTheResultContextIsAString()
{
    throw new NotImplementedException();
}

[Then(""Value with key 'WhenResultKey' in the result context should be equal '10and1.1'"")]
public static void ThenValueWithKeyWhenResultKeyInTheResultContextShouldBeEqual10and11()
{
    throw new NotImplementedException();
}";

            result.Should().BeEquivalentTo(EXPECTED_RESULT);
        }
    }
}