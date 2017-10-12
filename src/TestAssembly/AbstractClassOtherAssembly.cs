using SimpleBdd.Attributes;

namespace TestAssembly
{
    public abstract class AbstractClassOtherAssembly
    {
        [Given("GivenAbstractClassOtherAssemblyAbstractMethod")]
        protected abstract void GivenAbstractClassOtherAssemblyAbstractMethod();

        [When("WhenAbstractClassOtherAssemblyAbstractMethod")]
        protected abstract void WhenAbstractClassOtherAssemblyAbstractMethod();

        [Then("ThenAbstractClassOtherAssemblyAbstractMethod")]
        protected abstract void ThenAbstractClassOtherAssemblyAbstractMethod();
    }
}