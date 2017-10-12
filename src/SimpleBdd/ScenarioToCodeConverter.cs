using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace SimpleBdd
{
    public static class ScenarioToCodeConverter
    {
        private const string METHOD_TEMPLATE =
@"[{attributeName}(""{regexPattern}"")]
public static void {methodName}()
{
    throw new NotImplementedException();
}";

        private static readonly Regex _andRegEx = new Regex(@"\.And\(""(.+?)""");
        private static readonly Regex _methodNameInvalidCharReplacement = new Regex("[^A-z0-9 ]");
        private static readonly char[] _methodNameSplitArray = { ' ' };

        public static string Convert([NotNull] string scenarioAsString)
        {
            StringBuilder builder = new StringBuilder();
            HashSet<string> methodNameCache = new HashSet<string>();
            FindAndAddMethods("Given", scenarioAsString, builder, methodNameCache);
            FindAndAddMethods("When", scenarioAsString, builder, methodNameCache);
            FindAndAddMethods("Then", scenarioAsString, builder, methodNameCache);

            return builder.ToString().Trim();
        }

        private static void FindAndAddMethods([NotNull] string attributeName, [NotNull] string scenarioAsString,
            [NotNull] StringBuilder builder, [NotNull] HashSet<string> methodNameCache)
        {
            Regex regex = new Regex($@"\.{attributeName}\(""(.+?)""\)(\s*\.And\(.+?\))*(\s*?(\.|;))",
                RegexOptions.Singleline | RegexOptions.Multiline);

            MatchCollection matches = regex.Matches(scenarioAsString);

            foreach (Match match in matches)
            {
                string regexPattern = match.Groups[1].Value;

                string loweredRegexPattern = regexPattern.ToLowerInvariant();
                string methodName;
                string method;
                if (!methodNameCache.Contains(loweredRegexPattern))
                {
                    methodNameCache.Add(loweredRegexPattern);

                    methodName = $"{attributeName}{GetMethodNameFromString(regexPattern)}";
                    method = GetMethodFromTemplate(attributeName, regexPattern, methodName);
                    builder.AppendLine(method);
                    builder.AppendLine();
                }

                string andSection = match.Groups[0].Value;
                if (!string.IsNullOrWhiteSpace(andSection))
                {
                    MatchCollection andMatches = _andRegEx.Matches(andSection);

                    foreach (Match andMatch in andMatches)
                    {
                        regexPattern = andMatch.Groups[1].Value;
                        loweredRegexPattern = regexPattern.ToLowerInvariant();
                        if (!methodNameCache.Contains(loweredRegexPattern))
                        {
                            methodNameCache.Add(loweredRegexPattern);

                            methodName = $"{attributeName}{GetMethodNameFromString(regexPattern)}";
                            method = GetMethodFromTemplate(attributeName, regexPattern, methodName);
                            builder.AppendLine(method);
                            builder.AppendLine();
                        }
                    }
                }
            }
        }

        public static string GetMethodNameFromString([NotNull] string str)
        {
            StringBuilder builder = new StringBuilder(str.Length);

            str = _methodNameInvalidCharReplacement.Replace(str, string.Empty);

            string[] splittedString = str.Split(_methodNameSplitArray, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in splittedString)
            {
                builder.Append(char.ToUpper(s[0]));

                for (int index = 1; index < s.Length; index++)
                {
                    builder.Append(s[index]);
                }
            }

            return builder.ToString();
        }

        [NotNull]
        private static string GetMethodFromTemplate([NotNull] string attributeName,
            [NotNull] string regexPattern,
            [NotNull] string methodName)
        {
            return METHOD_TEMPLATE
                .Replace("{attributeName}", attributeName)
                .Replace("{regexPattern}", regexPattern)
                .Replace("{methodName}", methodName);
        }
    }
}