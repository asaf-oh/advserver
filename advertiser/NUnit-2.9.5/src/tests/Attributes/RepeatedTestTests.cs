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
using System.Reflection;
using NUnit.Framework.Api;
using NUnit.Framework.Internal;
using NUnit.TestData.RepeatedTestFixture;
using NUnit.TestUtilities;

namespace NUnit.Framework.Attributes
{
    [TestFixture]
    public class RepeatedTestTests
	{
		private MethodInfo successMethod;
		private MethodInfo failOnFirstMethod;
		private MethodInfo failOnThirdMethod;

		[SetUp]
		public void SetUp()
		{
			Type testType = typeof(RepeatSuccessFixture);
			successMethod = testType.GetMethod ("RepeatSuccess");
			testType = typeof(RepeatFailOnFirstFixture);
			failOnFirstMethod = testType.GetMethod("RepeatFailOnFirst");
			testType = typeof(RepeatFailOnThirdFixture);
			failOnThirdMethod = testType.GetMethod("RepeatFailOnThird");
		}

		private TestResult RunTestOnFixture( object fixture )
		{
			TestSuite suite = TestBuilder.MakeFixture( fixture );
			Assert.AreEqual( 1, suite.Tests.Count, "Test case count" );
            return suite.Run(TestListener.NULL);
		}

		[Test]
		public void RepeatSuccess()
		{
			Assert.IsNotNull (successMethod);
			RepeatSuccessFixture fixture = new RepeatSuccessFixture();
			TestResult result = RunTestOnFixture( fixture );

            Assert.IsTrue(result.ResultState == ResultState.Success);
			Assert.AreEqual(1, fixture.FixtureSetupCount);
			Assert.AreEqual(1, fixture.FixtureTeardownCount);
			Assert.AreEqual(3, fixture.SetupCount);
			Assert.AreEqual(3, fixture.TeardownCount);
			Assert.AreEqual(3, fixture.Count);
		}

		[Test]
		public void RepeatFailOnFirst()
		{
			Assert.IsNotNull (failOnFirstMethod);
			RepeatFailOnFirstFixture fixture = new RepeatFailOnFirstFixture();
			TestResult result = RunTestOnFixture( fixture );

            Assert.IsFalse(result.ResultState == ResultState.Success);
			Assert.AreEqual(1, fixture.SetupCount);
			Assert.AreEqual(1, fixture.TeardownCount);
			Assert.AreEqual(1, fixture.Count);
		}

		[Test]
		public void RepeatFailOnThird()
		{
			Assert.IsNotNull (failOnThirdMethod);
			RepeatFailOnThirdFixture fixture = new RepeatFailOnThirdFixture();
			TestResult result = RunTestOnFixture( fixture );

            Assert.IsFalse(result.ResultState == ResultState.Success);
			Assert.AreEqual(3, fixture.SetupCount);
			Assert.AreEqual(3, fixture.TeardownCount);
			Assert.AreEqual(3, fixture.Count);
		}

		[Test]
		public void IgnoreWorksWithRepeatedTest()
		{
			RepeatedTestWithIgnore fixture = new RepeatedTestWithIgnore();
			RunTestOnFixture( fixture );

			Assert.AreEqual( 0, fixture.SetupCount );
			Assert.AreEqual( 0, fixture.TeardownCount );
			Assert.AreEqual( 0, fixture.Count );
		}

        [Test]
        public void CategoryWorksWithRepeatedTest()
        {
            TestSuite suite = TestBuilder.MakeFixture(typeof(RepeatedTestWithCategory));
            Test test = suite.Tests[0] as Test;
            Assert.IsNotNull(test.Categories);
            Assert.AreEqual(1, test.Categories.Count);
            Assert.AreEqual("SAMPLE", test.Categories[0]);
        }
	}
}
