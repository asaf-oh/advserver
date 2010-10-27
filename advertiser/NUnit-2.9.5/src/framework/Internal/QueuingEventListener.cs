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

namespace NUnit.Framework.Internal
{
	/// <summary>
	/// QueuingEventListener uses an EventQueue to store any
	/// events received on its EventListener interface.
	/// </summary>
	public class QueuingEventListener : ITestListener
	{
		private EventQueue events = new EventQueue();

		/// <summary>
		/// The EvenQueue created and filled by this listener
		/// </summary>
		public EventQueue Events
		{
			get { return events; }
		}

		#region EventListener Methods
		/// <summary>
		/// A test has started
		/// </summary>
		/// <param name="test">The test that is starting</param>
		public void TestStarted(ITest test)
		{
			events.Enqueue( new TestStartedEvent( test ) );
		}

		/// <summary>
		/// A test case finished
		/// </summary>
		/// <param name="result">Result of the test case</param>
		public void TestFinished(ITestResult result)
		{
			events.Enqueue( new TestFinishedEvent( result ) );
		}

        /// <summary>
        /// The test created some text output.
        /// </summary>
        /// <param name="output">A TestOutput message</param>
        public void TestOutput(TestOutput output)
		{
			events.Enqueue( new OutputEvent( output ) );
		}
		#endregion
	}
}
