// ***********************************************************************
// Copyright (c) 2007 Charlie Poole
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
using System.Collections;
using NUnit.Framework.Api;
using NUnit.Framework.Internal;
using NUnit.Framework.Builders;
using NUnit.TestData;
using NUnit.TestData.LegacySuiteData;

namespace NUnit.Framework.Tests
{
	/// <summary>
	/// Summary description for LegacySuiteTests.
	/// </summary>
	[TestFixture]
	public class LegacySuiteTests
	{
        private LegacySuiteBuilder builder = new LegacySuiteBuilder();

        [TestFixtureSetUp]
        public void InitializeBuilders()
        {
            if (!CoreExtensions.Host.Initialized)
                CoreExtensions.Host.Initialize();
        }
        
        [Test]
        public void SuiteReturningTestSuite()
        {
            TestSuite suite = (TestSuite)builder.BuildFrom(typeof(LegacySuiteReturningTestSuite));
            Assert.AreEqual(RunState.Runnable, suite.RunState);
            Assert.AreEqual(3, suite.Tests.Count);
            Assert.AreEqual(5, suite.TestCaseCount);
        }

        [Test]
        public void SuiteReturningFixtures()
        {
            TestSuite suite = (TestSuite)builder.BuildFrom(typeof(LegacySuiteReturningFixtures));
            Assert.AreEqual(RunState.Runnable, suite.RunState);
            Assert.AreEqual(3, suite.Tests.Count);
            Assert.AreEqual(5, suite.TestCaseCount);
        }

        [Test]
        public void SuiteReturningTypes()
        {
            TestSuite suite = (TestSuite)builder.BuildFrom(typeof(LegacySuiteReturningTypes));
            Assert.AreEqual(RunState.Runnable, suite.RunState);
            Assert.AreEqual(3, suite.Tests.Count);
            Assert.AreEqual(5, suite.TestCaseCount);
        }

        [Test]
		public void SetUpAndTearDownAreCalled()
		{
            LegacySuiteWithSetUpAndTearDown.SetupCount = LegacySuiteWithSetUpAndTearDown.TeardownCount = 0;
			Test suite = builder.BuildFrom( typeof( LegacySuiteWithSetUpAndTearDown ) );
            Assert.AreEqual(RunState.Runnable, suite.RunState);
            suite.Run(TestListener.NULL);
            Assert.AreEqual(1, LegacySuiteWithSetUpAndTearDown.SetupCount);
            Assert.AreEqual(1, LegacySuiteWithSetUpAndTearDown.TeardownCount);
		}

        [Test]
        public void SuitePropertyWithInvalidType()
        {
            Test suite = builder.BuildFrom(typeof(LegacySuiteWithInvalidPropertyType));
            Assert.AreEqual(RunState.NotRunnable, suite.RunState);
        }
	}
}
