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
using NUnit.Framework.Api;
using NUnit.Framework.Internal;
using NUnit.TestData;
using NUnit.TestUtilities;

namespace NUnit.Framework.Tests
{
	/// <summary>
	/// Tests for MaxTime decoration.
	/// </summary>
    [TestFixture]
    public class MaxTimeTests
	{
		[Test,MaxTime(1000)]
		public void MaxTimeNotExceeded()
		{
		}

        // TODO: We need a way to simulate the clock reliably
        [Test]
        public void MaxTimeExceeded()
        {
            Test test = TestBuilder.MakeFixture(typeof(MaxTimeFixture));
            TestSuiteResult suiteResult = (TestSuiteResult)test.Run(TestListener.NULL);
            Assert.AreEqual(ResultState.Failure, suiteResult.ResultState);
            TestResult result = (TestResult)suiteResult.Children[0];
            StringAssert.IsMatch(@"Elapsed time of \d*ms exceeds maximum of 1ms", result.Message);
        }

        [Test, MaxTime(1000)]
        [ExpectedException(typeof(AssertionException), ExpectedMessage = "Intentional Failure")]
        public void FailureReport()
        {
            Assert.Fail("Intentional Failure");
        }

        [Test]
        public void FailureReportHasPriorityOverMaxTime()
		{
            Test test = TestBuilder.MakeFixture(typeof(MaxTimeFixtureWithFailure));
            TestResult result = test.Run(TestListener.NULL);
            Assert.AreEqual(ResultState.Failure, result.ResultState);
            result = (TestResult)result.Children[0];
            Assert.AreEqual(ResultState.Failure, result.ResultState);
            StringAssert.IsMatch("Intentional Failure", result.Message);
        }

        [Test, MaxTime(1000), ExpectedException]
        public void ErrorReport()
        {
            throw new Exception();
        }

        [Test]
        public void ErrorReportHasPriorityOverMaxTime()
        {
            Test test = TestBuilder.MakeFixture(typeof(MaxTimeFixtureWithError));
            TestResult result = test.Run(TestListener.NULL);
            Assert.AreEqual(ResultState.Failure, result.ResultState);
            result = (TestResult)result.Children[0];
            Assert.AreEqual(ResultState.Error, result.ResultState);
            StringAssert.IsMatch("Exception message", result.Message);
        }
    }
}
