using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SimpleBdd.Attributes;
using SimpleBdd.Exceptions;
using SimpleBdd.Loggers;

namespace SimpleBdd
{
    /// <summary>
    /// Initial point for scenario test builder
    /// </summary>
    public class Scenario
    {
        private static readonly ConcurrentDictionary<Assembly, TestMethod[]> _testMethodCache =
            new ConcurrentDictionary<Assembly, TestMethod[]>();

        private readonly List<Tuple<TestMethod, Match>> _foundTestMethods = new List<Tuple<TestMethod, Match>>();

        private readonly ScenarioContext _scenarioContext = new ScenarioContext();

        protected readonly TestMethod[] TestMethods;

        protected readonly List<ChainedTestMethod> TestChain = new List<ChainedTestMethod>();

        [CanBeNull]
        public IScenarioLogger ScenarioLogger { get; set; } = new ScenarioLogger();


        /// <summary>
        /// Builder to construct scenario with search for following attributes
        /// <see cref="T:SimpleBdd.Attributes.GivenAttribute"/>
        /// , <see cref="T:SimpleBdd.Attributes.WhenAttribute"/>
        /// , <see cref="T:SimpleBdd.Attributes.ThenAttribute"/>
        /// in restricted list of types
        /// </summary>
        /// <param name="typesForSearch">An array of types to search in</param>
        public Scenario([NotNull] [ItemNotNull] params Type[] typesForSearch)
        {
            TypeInfo[] typeInfos = typesForSearch
                .Select(type => type.GetTypeInfo())
                .ToArray();

            TestMethods = InitializeTestMethodList(typeInfos)
                .ToArray();
        }

        /// <summary>
        /// Because it's not possible to get all assemblies in UWP anymore it's required to pass an object to extract
        /// assembly from. This assembly will be used to search for methods marked with following attributes:
        /// <see cref="T:SimpleBdd.Attributes.GivenAttribute"/>
        /// , <see cref="T:SimpleBdd.Attributes.WhenAttribute"/>
        /// , <see cref="T:SimpleBdd.Attributes.ThenAttribute"/>
        /// Assembly will be extracted by assemblyHolder.GetType().GetTypeInfo().Assembly
        /// </summary>
        /// <param name="assemblyHolder"></param>
        /// <remarks>Scenario caches testing method for supplied assembly</remarks>
        public Scenario([NotNull] object assemblyHolder)
        {
            Assembly assembly = assemblyHolder.GetType().GetTypeInfo().Assembly;
            TestMethods = InitializeTestMethodList(assembly).ToArray();
        }

        /// <summary>
        /// Because it's not possible to get all assemblies in UWP anymore it's required to pass a list of assemblies
        /// to search for methods marked with following attributes:
        /// <see cref="T:SimpleBdd.Attributes.GivenAttribute"/>
        /// , <see cref="T:SimpleBdd.Attributes.WhenAttribute"/>
        /// , <see cref="T:SimpleBdd.Attributes.ThenAttribute"/>
        /// Assembly can be extracted by typeof(MyMagicType).GetTypeInfo().Assembly
        /// </summary>
        /// <param name="assemblies">Array of assemblies to search in</param>
        /// <remarks>Scenario caches testing method for supplied assemblies</remarks>
        public Scenario([NotNull] [ItemNotNull] params Assembly[] assemblies)
        {
            TestMethods = InitializeTestMethodList(assemblies).ToArray();
        }


        /// <summary>
        /// Adds test case to test chain
        /// </summary>
        /// <param name="testCase">Test case in </param>
        /// <returns>Returns the same scenario to make chaining possible</returns>
        /// <exception cref="T:SimpleBdd.Exceptions.AmbiguousTestMethodFoundException"></exception>
        /// <exception cref="T:SimpleBdd.Exceptions.TestMethodNotFoundException"></exception>
        public Scenario Given([NotNull] string testCase)
        {
            TestChain.Add(FindTestMethod(TestMethodType.Given, testCase, null));
            return this;
        }

        /// <summary>
        /// Adds test case to test chain
        /// </summary>
        /// <param name="testCase">Test case in </param>
        /// <param name="table">Table of user values to pass to test method</param>
        /// <returns>Returns the same scenario to make chaining possible</returns>
        /// <exception cref="T:SimpleBdd.Exceptions.AmbiguousTestMethodFoundException"></exception>
        /// <exception cref="T:SimpleBdd.Exceptions.TestMethodNotFoundException"></exception>
        public Scenario Given([NotNull] string testCase, [NotNull] Table table)
        {
            TestChain.Add(FindTestMethod(TestMethodType.Given, testCase, table));
            return this;
        }

        /// <summary>
        /// Adds test case to test chain
        /// </summary>
        /// <param name="testCase">Test case in </param>
        /// <returns>Returns the same scenario to make chaining possible</returns>
        /// <exception cref="T:SimpleBdd.Exceptions.AmbiguousTestMethodFoundException"></exception>
        /// <exception cref="T:SimpleBdd.Exceptions.TestMethodNotFoundException"></exception>
        public Scenario When([NotNull] string testCase)
        {
            TestChain.Add(FindTestMethod(TestMethodType.When, testCase, null));
            return this;
        }


        /// <summary>
        /// Adds test case to test chain
        /// </summary>
        /// <param name="testCase">Test case in </param>
        /// <param name="table">Table of user values to pass to test method</param>
        /// <returns>Returns the same scenario to make chaining possible</returns>
        /// <exception cref="T:SimpleBdd.Exceptions.AmbiguousTestMethodFoundException"></exception>
        /// <exception cref="T:SimpleBdd.Exceptions.TestMethodNotFoundException"></exception>
        public Scenario When([NotNull] string testCase, [NotNull] Table table)
        {
            TestChain.Add(FindTestMethod(TestMethodType.When, testCase, table));
            return this;
        }

        /// <summary>
        /// Adds test case to test chain
        /// </summary>
        /// <param name="testCase">Test case in </param>
        /// <returns>Returns the same scenario to make chaining possible</returns>
        /// <exception cref="T:SimpleBdd.Exceptions.AmbiguousTestMethodFoundException"></exception>
        /// <exception cref="T:SimpleBdd.Exceptions.TestMethodNotFoundException"></exception>
        public Scenario Then([NotNull] string testCase)
        {
            TestChain.Add(FindTestMethod(TestMethodType.Then, testCase, null));
            return this;
        }

        /// <summary>
        /// Adds test case to test chain
        /// </summary>
        /// <param name="testCase">Test case in </param>
        /// <param name="table">Table of user values to pass to test method</param>
        /// <returns>Returns the same scenario to make chaining possible</returns>
        /// <exception cref="T:SimpleBdd.Exceptions.AmbiguousTestMethodFoundException"></exception>
        /// <exception cref="T:SimpleBdd.Exceptions.TestMethodNotFoundException"></exception>
        public Scenario Then([NotNull] string testCase, [NotNull] Table table)
        {
            TestChain.Add(FindTestMethod(TestMethodType.Then, testCase, table));
            return this;
        }

        /// <summary>
        /// Adds test case with type of previous added test case to test chain
        /// </summary>
        /// <param name="testCase">Test case in </param>
        /// <returns>Returns the same scenario to make chaining possible</returns>
        /// <exception cref="T:SimpleBdd.Exceptions.AmbiguousTestMethodFoundException"></exception>
        /// <exception cref="T:SimpleBdd.Exceptions.TestMethodNotFoundException"></exception>
        /// <exception cref="T:System.InvalidOperationException"></exception>
        public Scenario And([NotNull] string testCase)
        {
            return InternalAnd(testCase, null);
        }

        /// <summary>
        /// Adds test case with type of previous added test case to test chain
        /// </summary>
        /// <param name="testCase">Test case in </param>
        /// <param name="table">Table of user values to pass to test method</param>
        /// <returns>Returns the same scenario to make chaining possible</returns>
        /// <exception cref="T:SimpleBdd.Exceptions.AmbiguousTestMethodFoundException"></exception>
        /// <exception cref="T:SimpleBdd.Exceptions.TestMethodNotFoundException"></exception>
        /// <exception cref="T:System.InvalidOperationException"></exception>
        public Scenario And([NotNull] string testCase, [NotNull] Table table)
        {
            return InternalAnd(testCase, table);
        }

        private Scenario InternalAnd([NotNull] string testCase, Table table)
        {
            if (!TestChain.Any())
            {
                throw new InvalidOperationException("There is no action to add to.");
            }

            ChainedTestMethod lastChainedTestMethod = TestChain.Last();

            switch (lastChainedTestMethod.TestMethod.TestMethodType)
            {
                case TestMethodType.Given:
                    return Given(testCase, table);

                case TestMethodType.When:
                    return When(testCase, table);

                case TestMethodType.Then:
                    return Then(testCase, table);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Run()
        {
            foreach (ChainedTestMethod chainedTestMethod in TestChain)
            {
                ScenarioLogger?.Log($"---> {chainedTestMethod.TestMethod.TestMethodType} {chainedTestMethod.TestCase}");
                try
                {
                    AsyncStateMachineAttribute asyncAttribute = chainedTestMethod.TestMethod.MethodInfo
                        .GetCustomAttribute<AsyncStateMachineAttribute>();
                    if (asyncAttribute != null)
                    {
                        Task task = (Task)chainedTestMethod.TestMethod.MethodInfo
                            .Invoke(null, chainedTestMethod.Parameters);
                        task.Wait();
                        if (task.Exception != null)
                        {
                            throw new AggregateException(task.Exception);
                        }
                    }
                    else
                    {
                        chainedTestMethod.TestMethod.MethodInfo.Invoke(null, chainedTestMethod.Parameters);
                    }
                    ScenarioLogger?.Log("Done");
                }
                catch (Exception e)
                {
                    ScenarioLogger?.Log(e.Message);
                    ScenarioLogger?.Log(e.StackTrace);
                    throw new AggregateException(e);
                }
            }
        }

        protected ChainedTestMethod FindTestMethod(TestMethodType testMethodType, [NotNull] string testCase,
            [CanBeNull] Table table)
        {
            _foundTestMethods.Clear();

            foreach (TestMethod testMethod in TestMethods)
            {
                if (testMethod.TestMethodType == testMethodType)
                {
                    Match match = testMethod.MatchingPattern.Match(testCase);
                    if (match.Success)
                    {
                        _foundTestMethods.Add(new Tuple<TestMethod, Match>(testMethod, match));
                    }
                }
            }

            if (_foundTestMethods.Count == 0)
            {
                throw new TestMethodNotFoundException($"Test method for test case '{testCase}' hasn't been found");
            }

            if (_foundTestMethods.Count > 1)
            {
                string matchedTestMethods = string.Join(", ",
                    _foundTestMethods.Select(item => $"'{item.Item1.RegexPattern}'"));

                throw new AmbiguousTestMethodFoundException(
                    $"Following test methods match to '{testCase}':{Environment.NewLine}{matchedTestMethods}");
            }

            Tuple<TestMethod, Match> matchingTestMethod = _foundTestMethods.First();
            object[] parameters = BuildParameters(matchingTestMethod.Item1, matchingTestMethod.Item2, table).ToArray();
            ChainedTestMethod chainedTestMethod = new ChainedTestMethod(matchingTestMethod.Item1, testCase, parameters);
            return chainedTestMethod;
        }

        private IEnumerable<object> BuildParameters(TestMethod testMethod, Match match, Table table)
        {
            int index = 1; //We processing groups beginning from first because zero group is a matching test case
            foreach (ParameterInfo parameterInfo in testMethod.MethodInfo.GetParameters())
            {
                if (parameterInfo.ParameterType == typeof(Table))
                {
                    yield return table;
                }
                else if (parameterInfo.ParameterType == typeof(ScenarioContext))
                {
                    yield return _scenarioContext;
                }
                else
                {
                    string matchingGroupValue = match.Groups[index].Value;
                    yield return Convert.ChangeType(matchingGroupValue, parameterInfo.ParameterType);
                    ++index;
                }
            }
        }

        [NotNull]
        [ItemNotNull]
        protected static IEnumerable<TestMethod> InitializeTestMethodList(
            [NotNull] [ItemNotNull] params Assembly[] assemblies)
        {
            foreach (Assembly assembly in assemblies)
            {
                TestMethod[] testMethods = _testMethodCache.GetOrAdd(assembly, ExtractTestMethods);
                foreach (TestMethod testMethod in testMethods)
                {
                    yield return testMethod;
                }
            }
        }

        [NotNull]
        [ItemNotNull]
        protected static IEnumerable<TestMethod> InitializeTestMethodList(
            [NotNull] [ItemNotNull] params TypeInfo[] types)
        {
            return types.SelectMany(ExtractTestMethods);
        }

        [NotNull]
        [ItemNotNull]
        protected static TestMethod[] ExtractTestMethods([NotNull] Assembly assembly)
        {
            return assembly.DefinedTypes.SelectMany(ExtractTestMethods).ToArray();
        }

        [NotNull]
        [ItemNotNull]
        protected static IEnumerable<TestMethod> ExtractTestMethods([NotNull] TypeInfo type)
        {
            return type.DeclaredMethods
                .Where(methodInfo => !methodInfo.IsAbstract && methodInfo.IsStatic)
                .SelectMany(methodInfo =>
                    methodInfo.GetCustomAttributes<GivenAttribute>()
                        .Select(attribute => new TestMethod(TestMethodType.Given, methodInfo, attribute.RegExPattern))
                        .Concat(methodInfo.GetCustomAttributes<WhenAttribute>()
                            .Select(attribute => new TestMethod(TestMethodType.When, methodInfo,
                                attribute.RegExPattern)))
                        .Concat(methodInfo.GetCustomAttributes<ThenAttribute>()
                            .Select(attribute => new TestMethod(TestMethodType.Then, methodInfo,
                                attribute.RegExPattern))));
        }
    }
}
