using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleBdd.Tests.Tests
{
    [TestClass]
    public class ContextTests
    {
        [TestMethod]
        public void GetWorksAfterPut()
        {
            ScenarioContext.Current.Clear();

            const int value = 1010;
            const string key = "TestKey";

            ScenarioContext.Current.Put(value);
            ScenarioContext.Current.Put(value, key);
            ScenarioContext.Current.Put(value, instanceIndex: value);
            ScenarioContext.Current.Put(value, key, value);

            ScenarioContext.Current.Get<int>().Should().Be(value);
            ScenarioContext.Current.Get<int>(key).Should().Be(value);
            ScenarioContext.Current.Get<int>(instanceIndex: value).Should().Be(value);
            ScenarioContext.Current.Get<int>(key, value).Should().Be(value);
        }

        [TestMethod]
        public void GetFailesWithoutPut()
        {
            ScenarioContext.Current.Clear();

            Action action = () =>
                {
                    ScenarioContext.Current.Get<int>();
                };

            action.ShouldThrow<KeyNotFoundException>();
        }

        [TestMethod]
        public void GetResultWorksAfterPutResult()
        {
            ScenarioContext.Current.Clear();

            const int value = 1010;
            const string key = "TestKey";

            ScenarioContext.Current.PutResult(value);
            ScenarioContext.Current.PutResult(value, key);
            ScenarioContext.Current.PutResult(value, instanceIndex: value);
            ScenarioContext.Current.PutResult(value, key, value);

            ScenarioContext.Current.GetResult<int>().Should().Be(value);
            ScenarioContext.Current.GetResult<int>(key).Should().Be(value);
            ScenarioContext.Current.GetResult<int>(instanceIndex: value).Should().Be(value);
            ScenarioContext.Current.GetResult<int>(key, value).Should().Be(value);
        }

        [TestMethod]
        public void GetResultFailesWithoutPutResult()
        {
            ScenarioContext.Current.Clear();

            Action action = () =>
                {
                    ScenarioContext.Current.GetResult<int>();
                };

            action.ShouldThrow<KeyNotFoundException>();
        }

        [TestMethod]
        public void GetResultFailesIfPutMade()
        {
            ScenarioContext.Current.Clear();

            ScenarioContext.Current.Put(10);

            Action action = () =>
                {
                    ScenarioContext.Current.GetResult<int>();
                };

            action.ShouldThrow<KeyNotFoundException>();
        }

        [TestMethod]
        public void GetFailesIfPutResultMade()
        {
            ScenarioContext.Current.Clear();

            ScenarioContext.Current.PutResult(10);

            Action action = () =>
                {
                    ScenarioContext.Current.Get<int>();
                };

            action.ShouldThrow<KeyNotFoundException>();
        }
    }
}