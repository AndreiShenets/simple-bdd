using SimpleBdd.Attributes;

namespace SimpleBdd.Tests.TestObjects
{
    public abstract class AbstractClass
    {
        [Given("GivenAbstractClassAbstractMethod")]
        protected abstract void GivenAbstractClassAbstractMethod();

        [When("WhenAbstractClassAbstractMethod")]
        protected abstract void WhenAbstractClassAbstractMethod();

        [Then("ThenAbstractClassAbstractMethod")]
        protected abstract void ThenAbstractClassAbstractMethod();
    }
}