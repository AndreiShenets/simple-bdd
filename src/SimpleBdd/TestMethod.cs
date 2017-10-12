using System.Reflection;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace SimpleBdd
{
    public class TestMethod
    {
        public TestMethodType TestMethodType { get; }

        [NotNull]
        public MethodInfo MethodInfo { get; }

        [NotNull]
        public string RegexPattern { get;  }

        [NotNull]
        public Regex MatchingPattern { get; }


        public TestMethod(TestMethodType testMethodType,
            [NotNull] MethodInfo methodInfo,
            [NotNull] string regexPattern)
        {
            TestMethodType = testMethodType;
            MethodInfo = methodInfo;
            RegexPattern = regexPattern;
            MatchingPattern = new Regex(regexPattern);
        }
    }
}