using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace EventStore.API.Test.Integration
{
    public class TestCaseOrderer : ITestCaseOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            var cases = new List<Tuple<int, TTestCase>>();

            foreach (TTestCase testCase in testCases)
            {

                var attribute = testCase.TestMethod.Method.GetCustomAttributes(typeof(TestPriorityAttribute)).FirstOrDefault();
                var priority = attribute?.GetNamedArgument<int>("Priority") ?? 0;

                cases.Add(Tuple.Create(priority, testCase));
            }

            cases.Sort((x, y) => x.Item1 - y.Item1);
            return cases.Select(c => c.Item2);
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class TestPriorityAttribute : Attribute
    {
        public int Priority { get; private set; }

        public TestPriorityAttribute(int priority)
        {
            Priority = priority;
        }
    }
}
