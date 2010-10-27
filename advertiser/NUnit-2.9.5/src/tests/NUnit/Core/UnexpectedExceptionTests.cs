// ***********************************************************************
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
using NUnit.Framework;
using NUnit.Framework.Api;
using NUnit.Framework.Internal;
using NUnit.Framework.Builders;
using NUnit.TestData.UnexpectedExceptionFixture;
using NUnit.TestUtilities;

namespace NUnit.Framework.Tests
{
	[TestFixture]
	public class UnexpectedExceptionTests
	{
        [Test]
        public void FailRecordsInnerException()
        {
            string expectedMessage = 
                "System.Exception : Outer Exception" + Environment.NewLine + "  ----> System.Exception : Inner Exception";

            ITestResult result = TestBuilder.RunTestCase(
                typeof(UnexpectedExceptionFixture), 
                "ThrowsWithInnerException");

            Assert.AreEqual(ResultState.Error, result.ResultState);
            Assert.AreEqual(expectedMessage, result.Message);
        }

        [Test]
        public void BadStackTraceIsHandled()
        {
            TestResult result = TestBuilder.RunTestCase(
                typeof(UnexpectedExceptionFixture), 
                "ThrowsWithBadStackTrace");

            Assert.AreEqual(ResultState.Error, result.ResultState);
            Assert.AreEqual("NUnit.TestData.UnexpectedExceptionFixture.ExceptionWithBadStackTrace : thrown by me", result.Message);
            Assert.AreEqual("No stack trace available", result.StackTrace);
        }

        [Test]
        public void CustomExceptionIsHandled()
        {
            ITestResult result = TestBuilder.RunTestCase(
                typeof(UnexpectedExceptionFixture), 
                "ThrowsCustomException");

            Assert.AreEqual(ResultState.Error, result.ResultState);
            Assert.AreEqual("NUnit.TestData.UnexpectedExceptionFixture.CustomException : message", result.Message);
        }
    }
}
