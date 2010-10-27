﻿// ***********************************************************************
// Copyright (c) 2009 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Api;
using NUnit.Framework.Builders;
using NUnit.Framework.Extensibility;
using NUnit.Framework.Internal;

namespace NUnit.TestUtilities
{
    /// <summary>
    /// Utility Class used to build NUnit tests for use as test data
    /// </summary>
    public class TestBuilder
    {
        private static NUnitTestFixtureBuilder fixtureBuilder = new NUnitTestFixtureBuilder();
        private static NUnitTestCaseBuilder testBuilder = new NUnitTestCaseBuilder();

        static TestBuilder()
        {
            if (!CoreExtensions.Host.Initialized)
                CoreExtensions.Host.Initialize();
        }

        public static TestSuite MakeFixture(Type type)
        {
            return (TestSuite)fixtureBuilder.BuildFrom(type);
        }

        public static TestSuite MakeFixture(object fixture)
        {
            TestSuite suite = (TestSuite)fixtureBuilder.BuildFrom(fixture.GetType());
            suite.Fixture = fixture;
            return suite;
        }

        public static TestSuite MakeParameterizedMethodSuite(Type type, string methodName)
        {
            return (TestSuite)MakeTestCase(type, methodName);
        }

        public static Test MakeTestCase(Type type, string methodName)
        {
            MethodInfo method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (method == null)
                Assert.Fail("Method not found: " + methodName);
            return testBuilder.BuildFrom(method);
        }

        public static Test MakeTestCase(object fixture, string methodName)
        {
            Test test = MakeTestCase(fixture.GetType(), methodName);
            test.Fixture = fixture;
            return test;
        }

        public static TestSuiteResult RunTestFixture(Type type)
        {
            return (TestSuiteResult)MakeFixture(type).Run(TestListener.NULL);
        }

        public static TestSuiteResult RunTestFixture(object fixture)
        {
            return (TestSuiteResult)MakeFixture(fixture).Run(TestListener.NULL);
        }

        public static TestResult RunTestCase(Type type, string methodName)
        {
            return MakeTestCase(type, methodName).Run(TestListener.NULL);
        }

        private TestBuilder() { }
    }
}
