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
using System.Collections;
using System.Reflection;
using NUnit.Framework.Api;

namespace NUnit.Framework.Internal
{
	/// <summary>
	/// Represents a test suite constructed from a type that has a static Suite property
	/// </summary>
	public class LegacySuite : TestSuite
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="LegacySuite"/> class.
        /// </summary>
        /// <param name="fixtureType">Type of the fixture.</param>
		public LegacySuite( Type fixtureType ) : base( fixtureType )
		{
            this.fixtureSetUpMethods = GetSetUpTearDownMethods( typeof(NUnit.Framework.TestFixtureSetUpAttribute) );
            this.fixtureTearDownMethods = GetSetUpTearDownMethods( typeof(NUnit.Framework.TestFixtureTearDownAttribute) );
        }

        private MethodInfo[] GetSetUpTearDownMethods(Type attrType)
        {
            MethodInfo[] methods = Reflect.GetMethodsWithAttribute(FixtureType, attrType, true);

            foreach (MethodInfo method in methods)
                if (method.IsAbstract ||
                     !method.IsPublic && !method.IsFamily ||
                     method.GetParameters().Length > 0 ||
                     !method.ReturnType.Equals(typeof(void)))
                {
                    this.IgnoreReason = string.Format("Invalid signature for SetUp or TearDown method: {0}", method.Name);
                    this.RunState = RunState.NotRunnable;
                    break;
                }

            return methods;
        }
    }
}
