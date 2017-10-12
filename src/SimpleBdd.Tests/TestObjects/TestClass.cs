using SimpleBdd.Attributes;

namespace SimpleBdd.Tests.TestObjects
{
    public class TestClass
    {
        [Given("GivenClassStaticMethod")]
        public static void GivenClassStaticMethod()
        {
        }

        [Given("GivenClassStaticMethod")]
        public void GivenClass()
        {
        }

        [When("WhenClassStaticMethod")]
        public static void WhenClassStaticMethod()
        {
        }

        [When("WhenClass")]
        public void WhenClass()
        {
        }

        [Then("ThenClassStaticMethod")]
        public static void ThenClassStaticMethod()
        {
        }

        [Then("ThenClass")]
        public void ThenClass()
        {
        }

        [Given("GivenClassProtectedStaticMethod")]
        protected static void GivenClassProtectedStaticMethod()
        {
        }

        [When("WhenClassProtectedStaticMethod")]
        protected static void WhenClassProtectedStaticMethod()
        {
        }

        [Then("ThenClassProtectedStaticMethod")]
        protected static void ThenClassProtectedStaticMethod()
        {
        }

        [Given("GivenClassPrivateStaticMethod")]
        private static void GivenClassPrivateStaticMethod()
        {
        }

        [When("WhenClassPrivateStaticMethod")]
        private static void WhenClassPrivateStaticMethod()
        {
        }

        [Then("ThenClassPrivateStaticMethod")]
        private static void ThenClassPrivateStaticMethod()
        {
        }

        [Given("GivenClassProtectedMethod")]
        protected void GivenClassProtectedMethod()
        {
        }

        [When("WhenClassProtectedMethod")]
        protected void WhenClassProtectedMethod()
        {
        }

        [Then("ThenClassProtectedMethod")]
        protected void ThenClassProtectedMethod()
        {
        }

        [Given("GivenClassPrivateMethod")]
        private void GivenClassPrivateMethod()
        {
        }

        [When("WhenClassPrivateMethod")]
        private void WhenClassPrivateMethod()
        {
        }

        [Then("ThenClassPrivateMethod")]
        private void ThenClassPrivateMethod()
        {
        }
    }
}
