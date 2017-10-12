using JetBrains.Annotations;

namespace SimpleBdd
{
    public class ChainedTestMethod
    {
        [NotNull]
        public TestMethod TestMethod { get; }

        [NotNull]
        public string TestCase { get; }

        [NotNull]
        public object[] Parameters { get; }


        public ChainedTestMethod([NotNull] TestMethod testMethod,
            [NotNull] string testCase,
            [NotNull] object[] parameters)
        {
            TestMethod = testMethod;
            TestCase = testCase;
            Parameters = parameters;
        }
    }
}