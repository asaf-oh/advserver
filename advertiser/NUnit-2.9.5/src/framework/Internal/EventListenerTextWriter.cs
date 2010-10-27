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

using NUnit.Framework.Api;

namespace NUnit.Framework.Internal
{
	using System;
	using System.IO;
	using System.Text;

    /// <summary>
    /// EventListenerTextWriter is a TextWriter that channels output to
    /// the TestOutput method of an ITestListener.
    /// </summary>
	public class EventListenerTextWriter : TextWriter
	{
		private ITestListener eventListener;
		private TestOutputType type;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventListenerTextWriter"/> class.
        /// </summary>
        /// <param name="eventListener">The event listener.</param>
        /// <param name="type">The type.</param>
		public EventListenerTextWriter( ITestListener eventListener, TestOutputType type )
		{
			this.eventListener = eventListener;
			this.type = type;
		}

        /// <summary>
        /// Writes the specified char.
        /// </summary>
        /// <param name="aChar">A char.</param>
		override public void Write(char aChar)
		{
			this.eventListener.TestOutput( new TestOutput( aChar.ToString(), this.type ) );
		}

        /// <summary>
        /// Writes the specified string.
        /// </summary>
        /// <param name="aString">A string.</param>
		override public void Write(string aString)
		{
			this.eventListener.TestOutput( new TestOutput( aString, this.type ) );
		}

        /// <summary>
        /// Writes the specified string followed by a NewLine.
        /// </summary>
        /// <param name="aString">A string.</param>
		override public void WriteLine(string aString)
		{
			this.eventListener.TestOutput( new TestOutput( aString + this.NewLine, this.type ) );
		}

        /// <summary>
        /// When overridden in a derived class, returns the <see cref="T:System.Text.Encoding"/> in which the output is written.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The Encoding in which the output is written.
        /// </returns>
		override public System.Text.Encoding Encoding
		{
			get { return Encoding.Default; }
		}
	}

    ///// <summary>
    ///// This wrapper adds buffering to improve cross-domain performance.
    ///// </summary>
    //public class BufferedEventListenerTextWriter : TextWriter
    //{
    //    private ITestListener listener;
    //    private TestOutputType type;
    //    private const int MAX_BUFFER = 1024;
    //    private StringBuilder sb = new StringBuilder( MAX_BUFFER );

    //    public BufferedEventListenerTextWriter( ITestListener listener, TestOutputType type )
    //    {
    //        this.listener = listener;
    //        this.type = type;
    //    }

    //    public override Encoding Encoding
    //    {
    //        get
    //        {
    //            return Encoding.Default;
    //        }
    //    }
	
    //    override public void Write(char ch)
    //    {
    //        lock( sb )
    //        {
    //            sb.Append( ch );
    //            this.CheckBuffer();
    //        }
    //    }

    //    override public void Write(string str)
    //    {
    //        lock( sb )
    //        {
    //            sb.Append( str );
    //            this.CheckBuffer();
    //        }
    //    }

    //    override public void WriteLine(string str)
    //    {
    //        lock( sb )
    //        {
    //            sb.Append( str );
    //            sb.Append( base.NewLine );
    //            this.CheckBuffer();
    //        }
    //    }

    //    override public void Flush()
    //    {
    //        if ( sb.Length > 0 )
    //        {
    //            lock( sb )
    //            {
    //                TestOutput output = new TestOutput(sb.ToString(), this.type);
    //                this.listener.TestOutput( output );
    //                sb.Length = 0;
    //            }
    //        }
    //    }

    //    private void CheckBuffer()
    //    {
    //        if ( sb.Length >= MAX_BUFFER )
    //            this.Flush();
    //    }
    //}
}
