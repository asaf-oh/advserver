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

using System.Collections;

namespace NUnit.Framework.Internal
{
	/// <summary>
	/// Class that can build a tree of automatic namespace
	/// suites from a group of fixtures.
	/// </summary>
	public class NamespaceTreeBuilder
	{
		#region Instance Variables

		/// <summary>
		/// NamespaceDictionary of all test suites we have created to represent 
        /// namespaces. Used to locate namespace parent suites for fixtures.
		/// </summary>
		NamespaceDictionary namespaceSuites  = new NamespaceDictionary();

		/// <summary>
		/// The root of the test suite being created by this builder.
		/// </summary>
		TestSuite rootSuite;

		#endregion

		#region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NamespaceTreeBuilder"/> class.
        /// </summary>
        /// <param name="rootSuite">The root suite.</param>
		public NamespaceTreeBuilder( TestSuite rootSuite )
		{
			this.rootSuite = rootSuite;
		}

		#endregion

		#region Properties

        /// <summary>
        /// Gets the root entry in the tree created by the NamespaceTreeBuilder.
        /// </summary>
        /// <value>The root suite.</value>
		public TestSuite RootSuite
		{
			get { return rootSuite; }
		}

		#endregion

		#region Public Methods

        /// <summary>
        /// Adds the specified fixtures to the tree.
        /// </summary>
        /// <param name="fixtures">The fixtures to be added.</param>
		public void Add( IList fixtures )
		{
            foreach (TestSuite fixture in fixtures)
                //if (fixture is SetUpFixture)
                //    Add(fixture as SetUpFixture);
                //else
                    Add( fixture );
		}

        /// <summary>
        /// Adds the specified fixture to the tree.
        /// </summary>
        /// <param name="fixture">The fixture to be added.</param>
		public void Add( TestSuite fixture )
		{
            string ns = fixture.FullName;
            int index = ns.IndexOf("[");
            if (index >= 0) ns = ns.Substring(0, index);
            index = ns.LastIndexOf( '.' );
            ns = index > 0 ? ns.Substring( 0, index ) : string.Empty;
			TestSuite containingSuite = BuildFromNameSpace( ns );

            if (fixture is SetUpFixture)
            {
                // The SetUpFixture must replace the namespace suite
                // in which it is "contained". 
                //
                // First, add the old suite's children
                foreach (TestSuite child in containingSuite.Tests)
                    fixture.Add(child);

                // Make the parent of the containing suite point to this
                // fixture instead
                // TODO: Get rid of this somehow?
                TestSuite parent = (TestSuite)containingSuite.Parent;
                if (parent == null)
                {
                    fixture.Name = rootSuite.Name;
                    rootSuite = fixture;
                }
                else
                {
                    parent.Tests.Remove(containingSuite);
                    parent.Add(fixture);
                }

                // Update the dictionary
                namespaceSuites[ns] = fixture;
            }
            else
			    containingSuite.Add( fixture );
		}

		#endregion

		#region Helper Method

		private TestSuite BuildFromNameSpace( string ns )
		{
			if( ns == null || ns  == "" ) return rootSuite;

            TestSuite suite = namespaceSuites.ContainsKey(ns)
                ? namespaceSuites[ns]
                : null;
            
            if (suite != null)
                return suite;

            int index = ns.LastIndexOf(".");
			if( index == -1 )
			{
				suite = new TestSuite( ns );
				if ( rootSuite == null )
					rootSuite = suite;
				else
					rootSuite.Add(suite);
			}
			else
			{
				string parentNamespace = ns.Substring( 0,index );
				TestSuite parent = BuildFromNameSpace( parentNamespace );
				string suiteName = ns.Substring( index+1 );
				suite = new TestSuite( parentNamespace, suiteName );
				parent.Add( suite );
			}

            namespaceSuites[ns] = suite;
            return suite;
		}

		#endregion

        #region NamespaceDictionary Type
#if CLR_2_0
        class NamespaceDictionary : System.Collections.Generic.Dictionary<string, TestSuite> { }
#else
        class NamespaceDictionary : DictionaryBase
        {
            public TestSuite this[string key]
            {
                get { return (TestSuite)Dictionary[key]; }
                set { Dictionary[key] = value; }
            }

            public bool ContainsKey(string key)
            {
                return Dictionary.Contains(key);
            }
        }
#endif
        #endregion
    }
}
