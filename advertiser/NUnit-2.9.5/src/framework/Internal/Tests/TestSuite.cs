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
using System.Text;
using System.Threading;
using System.Collections;
using System.Reflection;
using NUnit.Framework.Api;

namespace NUnit.Framework.Internal
{
    /// <summary>
    /// TestSuite represents a composite test, which contains other tests.
    /// </summary>
	public class TestSuite : Test
	{
		#region Fields
		/// <summary>
		/// Our collection of child tests
		/// </summary>
		private TestCollection tests = new TestCollection();

        /// <summary>
        /// The fixture setup methods for this suite
        /// </summary>
        protected MethodInfo[] fixtureSetUpMethods;

        /// <summary>
        /// The fixture teardown methods for this suite
        /// </summary>
        protected MethodInfo[] fixtureTearDownMethods;

        /// <summary>
        /// The setup methods for this suite
        /// </summary>
        protected MethodInfo[] setUpMethods;

        /// <summary>
        /// The teardown methods for this suite
        /// </summary>
        protected MethodInfo[] tearDownMethods;

        /// <summary>
        /// Set to true to suppress sorting this suite's contents
        /// </summary>
        protected bool maintainTestOrder;

        /// <summary>
        /// Arguments for use in creating a parameterized fixture
        /// </summary>
        internal object[] arguments;

        /// <summary>
        /// The System.Type of the fixture for this test suite, if there is one
        /// </summary>
        private Type fixtureType;

        /// <summary>
        /// The fixture object, if it has been created
        /// </summary>
        private object fixture;

        #endregion

		#region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TestSuite"/> class.
        /// </summary>
        /// <param name="name">The name of the suite.</param>
		public TestSuite( string name ) 
			: base( name ) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestSuite"/> class.
        /// </summary>
        /// <param name="parentSuiteName">Name of the parent suite.</param>
        /// <param name="name">The name of the suite.</param>
		public TestSuite( string parentSuiteName, string name ) 
			: base( parentSuiteName, name ) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestSuite"/> class.
        /// </summary>
        /// <param name="fixtureType">Type of the fixture.</param>
        public TestSuite(Type fixtureType)
            : this(fixtureType, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestSuite"/> class.
        /// </summary>
        /// <param name="fixtureType">Type of the fixture.</param>
        /// <param name="arguments">The arguments.</param>
        public TestSuite(Type fixtureType, object[] arguments)
            : base(fixtureType.FullName)
        {
            string name = TypeHelper.GetDisplayName(fixtureType, arguments);
            this.Name = name;
            
            this.FullName = name;
            string nspace = fixtureType.Namespace;
            if (nspace != null && nspace != "")
                this.FullName = nspace + "." + name;
            this.fixtureType = fixtureType;
            this.arguments = arguments;
        }
        #endregion

		#region Public Methods
        /// <summary>
        /// Sorts tests under this suite.
        /// </summary>
		public void Sort()
		{
            if (!maintainTestOrder)
            {
                this.tests.Sort();

                foreach (Test test in Tests)
                {
                    TestSuite suite = test as TestSuite;
                    if (suite != null)
                        suite.Sort();
                }
            }
		}

#if false
        /// <summary>
        /// Sorts tests under this suite using the specified comparer.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        public void Sort(IComparer comparer)
        {
			this.tests.Sort(comparer);

			foreach( Test test in Tests )
			{
				TestSuite suite = test as TestSuite;
				if ( suite != null )
					suite.Sort(comparer);
			}
		}
#endif

        /// <summary>
        /// Adds a test to the suite.
        /// </summary>
        /// <param name="test">The test.</param>
		public void Add( Test test ) 
		{
//			if( test.RunState == RunState.Runnable )
//			{
//				test.RunState = this.RunState;
//				test.IgnoreReason = this.IgnoreReason;
//			}
			test.Parent = this;
			tests.Add(test);
		}

#if !NUNITLITE
        /// <summary>
        /// Adds a pre-constructed test fixture to the suite.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
		public void Add( object fixture )
		{
			Test test = TestFixtureBuilder.BuildFrom( fixture );
			if ( test != null )
				Add( test );
		}
#endif

		#endregion

		#region Properties
        /// <summary>
        /// Gets this test's child tests
        /// </summary>
        /// <value></value>
		public IList Tests 
		{
			get { return tests; }
		}

        /// <summary>
        /// Indicates whether this test is a suite
        /// </summary>
        /// <value></value>
		public override bool IsTestCase
		{
			get { return false; }
		}

        /// <summary>
        /// Gets a count of test cases represented by
        /// or contained under this test.
        /// </summary>
        /// <value></value>
		public override int TestCaseCount
		{
			get
			{
				int count = 0;

				foreach(Test test in Tests)
				{
					count += test.TestCaseCount;
				}
				return count;
			}
		}

        /// <summary>
        /// Gets the Type of the fixture used in running this test
        /// </summary>
        /// <value></value>
        public override Type FixtureType
        {
            get { return fixtureType; }
        }

        /// <summary>
        /// Gets or sets a fixture object for running this test
        /// </summary>
        /// <value></value>
        public override object Fixture
        {
            get { return fixture; }
            set { fixture = value; }
        }

        /// <summary>
        /// Gets the set up methods.
        /// </summary>
        /// <returns></returns>
        public MethodInfo[] GetSetUpMethods()
        {
            return setUpMethods;
        }

        /// <summary>
        /// Gets the tear down methods.
        /// </summary>
        /// <returns></returns>
        public MethodInfo[] GetTearDownMethods()
        {
            return tearDownMethods;
        }
        #endregion

		#region Test Overrides

        ///// <summary>
        ///// Gets a count of test cases that would be run using
        ///// the specified filter.
        ///// </summary>
        ///// <param name="filter"></param>
        ///// <returns></returns>
        //public override int CountTestCases(TestFilter filter)
        //{
        //    int count = 0;

        //    if(filter.Pass(this)) 
        //    {
        //        foreach(Test test in Tests)
        //        {
        //            count += test.CountTestCases(filter);
        //        }
        //    }
        //    return count;
        //}

        /// <summary>
        /// Creates a TestSuiteResult.
        /// </summary>
        /// <returns>The new TestSuiteResult.</returns>
        public override TestResult MakeTestResult()
        {
            return new TestSuiteResult(this);
        }
        /// <summary>
        /// Runs the suite under a particular filter, sending
        /// notifications to a listener.
        /// </summary>
        /// <param name="listener">An event listener to receive notifications</param>
        /// <returns></returns>
		public override TestResult Run(ITestListener listener)
		{
            // Derived classes must return a result derived from TestSuiteResult
            TestSuiteResult suiteResult = (TestSuiteResult)this.MakeTestResult();

			listener.TestStarted( this );
			long startTime = DateTime.Now.Ticks;

			switch (this.RunState)
			{
				case RunState.Runnable:
				case RunState.Explicit:
#if !NUNITLITE
                    if (RequiresThread || ApartmentState != GetCurrentApartment())
                        new TestSuiteThread(this).Run(suiteResult, listener);
                    else
#endif
                        Run(suiteResult, listener);
					break;

				default:
                case RunState.Skipped:
			        SkipAllTests(suiteResult, listener);
                    break;
                case RunState.NotRunnable:
                    MarkAllTestsInvalid( suiteResult, listener);
                    break;
                case RunState.Ignored:
                    IgnoreAllTests(suiteResult, listener);
                    break;
			}

			long stopTime = DateTime.Now.Ticks;
			double time = ((double)(stopTime - startTime)) / (double)TimeSpan.TicksPerSecond;
			suiteResult.Time = time;

			listener.TestFinished(suiteResult);
			return suiteResult;
		}

        /// <summary>
        /// Runs the suite under a particular filter, sending
        /// notifications to a listener.
        /// </summary>
        /// <param name="suiteResult">The suite result.</param>
        /// <param name="listener">The listener.</param>
        public void Run(TestSuiteResult suiteResult, ITestListener listener)
        {
#if !NUNITLITE
            TestContext context = new TestContext();
#endif
            try
            {
                suiteResult.SetResult(ResultState.Success); // Assume success
                DoOneTimeSetUp(suiteResult);

#if !NUNITLITE
                if (this.Properties["_SETCULTURE"] != null)
                    TestContext.CurrentCulture =
                        new System.Globalization.CultureInfo((string)Properties["_SETCULTURE"]);

                if (this.Properties["_SETUICULTURE"] != null)
                    TestContext.CurrentUICulture =
                        new System.Globalization.CultureInfo((string)Properties["_SETUICULTURE"]);
#endif

                switch (suiteResult.ResultState.Status)
                {
                        // TODO: Handle Cancellation Better
                    case TestStatus.Failed:
                        string msg = string.Format("TestFixtureSetUp failed in {0}", this.FixtureType.Name);
#if !NUNITLITE
                    if (this is SetUpFixture)
                        msg = string.Format("Parent SetUp failed in {0}", this.FixtureType.Name);
#endif
                        MarkTestsFailed(this.tests, msg, suiteResult, listener);
                        break;
                    default:
                        try
                        {
                            RunAllTests(suiteResult, listener);
                        }
                        finally
                        {
                            DoOneTimeTearDown(suiteResult);
                        }
                        break;
                }
            }
            finally
            {
#if !NUNITLITE
                context.Dispose();
#endif
            }
        }
		#endregion

		#region Virtual Methods
        /// <summary>
        /// Does the one time set up.
        /// </summary>
        /// <param name="suiteResult">The suite result.</param>
        protected virtual void DoOneTimeSetUp(TestResult suiteResult)
        {
            if (FixtureType != null)
            {
                try
                {
					// In case TestFixture was created with fixture object
					if (Fixture == null && !IsStaticClass( FixtureType ) )
						CreateUserFixture();

                    if (this.fixtureSetUpMethods != null)
                        foreach (MethodInfo fixtureSetUp in fixtureSetUpMethods)
                            Reflect.InvokeMethod(fixtureSetUp, fixtureSetUp.IsStatic ? null : Fixture);

#if !NUNITLITE
                    TestContext.Update();
#endif
                }
                catch (Exception ex)
                {
                    if (ex is NUnitException || ex is System.Reflection.TargetInvocationException)
                        ex = ex.InnerException;

                    suiteResult.RecordException(ex);

                    // TODO: Is this needed?
                    if (ex is NUnit.Framework.IgnoreException)
                    {
                        this.RunState = RunState.Ignored;
                        this.IgnoreReason = ex.Message;
                    }
                }
            }
        }

        /// <summary>
        /// Creates the user fixture.
        /// </summary>
		protected virtual void CreateUserFixture()
		{
            if (arguments != null && arguments.Length > 0)
                Fixture = Reflect.Construct(FixtureType, arguments);
            else
			    Fixture = Reflect.Construct(FixtureType);
		}

        /// <summary>
        /// Does the one time tear down.
        /// </summary>
        /// <param name="suiteResult">The suite result.</param>
        protected virtual void DoOneTimeTearDown(TestResult suiteResult)
        {
            if ( this.FixtureType != null)
            {
                try
                {
                    if (this.fixtureTearDownMethods != null)
                    {
                        int index = fixtureTearDownMethods.Length;
                        while (--index >= 0 )
                        {
                            MethodInfo fixtureTearDown = fixtureTearDownMethods[index];
                            Reflect.InvokeMethod(fixtureTearDown, fixtureTearDown.IsStatic ? null : Fixture);
                        }
                    }

					IDisposable disposable = Fixture as IDisposable;
					if (disposable != null)
						disposable.Dispose();
				}
                catch (Exception ex)
                {
					// Error in TestFixtureTearDown or Dispose causes the
					// suite to be marked as a error, even if
					// all the contained tests passed.
					NUnitException nex = ex as NUnitException;
					if (nex != null)
						ex = nex.InnerException;

                    // TODO: Can we move this logic into TestResult itself?
                    string message = "TearDown : " + ExceptionHelper.BuildMessage(ex);
                    if (suiteResult.Message != null)
                        message = suiteResult.Message + NUnit.Env.NewLine + message;

#if !NETCF_1_0
                    string stackTrace = "--TearDown" + NUnit.Env.NewLine + ExceptionHelper.BuildStackTrace(ex);
                    if (suiteResult.StackTrace != null)
                        stackTrace = suiteResult.StackTrace + NUnit.Env.NewLine + stackTrace;

                    // TODO: What about ignore exceptions in teardown?
                    suiteResult.SetResult(ResultState.Error, message, stackTrace);
#else
                    suiteResult.SetResult(ResultState.Error, message);
#endif
                }

                this.Fixture = null;
            }
        }

        #endregion

        #region Helper Methods

        private bool IsStaticClass(Type type)
        {
            return type.IsAbstract && type.IsSealed;
        }

        private void RunAllTests(
			TestSuiteResult suiteResult, ITestListener listener )
		{
#if !NUNITLITE
            if (Properties.Contains("Timeout"))
                TestContext.TestCaseTimeout = (int)Properties["Timeout"];
#endif

            foreach (Test test in ArrayList.Synchronized(tests))
            {
                if (test.RunState != RunState.Explicit)
                {
                    RunState saveRunState = test.RunState;

                    if (test.RunState == RunState.Runnable && this.RunState != RunState.Runnable && this.RunState != RunState.Explicit )
                    {
                        test.RunState = this.RunState;
                        test.IgnoreReason = this.IgnoreReason;
                    }

                    TestResult result = test.Run(listener);

                    suiteResult.AddResult(result);

                    if (saveRunState != test.RunState)
                    {
                        test.RunState = saveRunState;
                        test.IgnoreReason = null;
                    }

                    if (result.ResultState == ResultState.Cancelled)
                        break;
                }
            }
		}

        private void SkipAllTests(TestSuiteResult suiteResult, ITestListener listener)
        {
            suiteResult.SetResult(ResultState.Skipped, this.IgnoreReason);
            MarkTestsNotRun(this.tests, ResultState.Skipped, this.IgnoreReason, suiteResult, listener);
        }

        private void IgnoreAllTests(TestSuiteResult suiteResult, ITestListener listener)
        {
            suiteResult.SetResult(ResultState.Ignored, this.IgnoreReason);
            MarkTestsNotRun(this.tests, ResultState.Ignored, this.IgnoreReason, suiteResult, listener);
        }

        private void MarkAllTestsInvalid(TestSuiteResult suiteResult, ITestListener listener)
        {
            suiteResult.SetResult(ResultState.NotRunnable, this.IgnoreReason);
            MarkTestsNotRun(this.tests, ResultState.NotRunnable, this.IgnoreReason, suiteResult, listener);
        }
       
        private void MarkTestsNotRun(
            TestCollection tests, ResultState resultState, string ignoreReason, TestSuiteResult suiteResult, ITestListener listener)
        {
            foreach (Test test in ArrayList.Synchronized(tests))
            {
                if (test.RunState != RunState.Explicit)
                    MarkTestNotRun(test, resultState, ignoreReason, suiteResult, listener);
            }
        }

        private void MarkTestNotRun(
            Test test, ResultState resultState, string ignoreReason, TestSuiteResult suiteResult, ITestListener listener)
        {
            listener.TestStarted(test);

            TestResult result = test.MakeTestResult(); 
            
            TestSuite suite = test as TestSuite;
            if (suite != null)
                MarkTestsNotRun(suite.tests, resultState, ignoreReason, suiteResult, listener);

            result.SetResult(resultState, ignoreReason);
            suiteResult.AddResult(result);
            listener.TestFinished(result);
        }

        private void MarkTestsFailed(
            TestCollection tests, string msg, TestSuiteResult suiteResult, ITestListener listener)
        {
            foreach (Test test in ArrayList.Synchronized(tests))
                if (test.RunState != RunState.Explicit)
                    MarkTestFailed(test, msg, suiteResult, listener);
        }

        private void MarkTestFailed(
            Test test, string msg, TestSuiteResult suiteResult, ITestListener listener)
        {
            listener.TestStarted(test);

            TestResult result = test.MakeTestResult();

            TestSuite suite = test as TestSuite;
            if (suite != null)
                MarkTestsFailed(suite.tests, msg, suiteResult, listener);

            result.SetResult(ResultState.Failure, msg);
            suiteResult.AddResult(result);
            listener.TestFinished(result);
        }
        #endregion

#if CLR_2_0 && !NETCF
        private class TestCollection : System.Collections.Generic.List<Test> { }
#else
        private class TestCollection : ArrayList { }
#endif
    }
}
