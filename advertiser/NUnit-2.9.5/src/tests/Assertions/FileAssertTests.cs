// ***********************************************************************
// Copyright (c) 2006 Charlie Poole
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
using System.IO;
using System.Reflection;
using System.Net.Sockets;

namespace NUnit.Framework.Assertions
{
	public class TestFile : IDisposable
	{
		private bool _disposedValue = false;
		private string _resourceName;
		private string _fileName;

		#region Nested TestFile Utility Class
		public TestFile(string fileName, string resourceName)
		{
			_resourceName = "NUnit.Framework.Tests." + resourceName;
			_fileName = fileName;

			Assembly a = Assembly.GetExecutingAssembly();
			using (Stream s = a.GetManifestResourceStream(_resourceName))
			{
				if (s == null) throw new Exception("Manifest Resource Stream " + _resourceName + " was not found.");

				byte[] buffer = new byte[1024];
				using (FileStream fs = File.Create(_fileName))
				{
					while(true)
					{
						int count = s.Read(buffer, 0, buffer.Length);
						if(count == 0) break;
						fs.Write(buffer, 0, count);
					}
				}
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this._disposedValue)
			{
				if (disposing)
				{
					if(File.Exists(_fileName))
					{
						File.Delete(_fileName);
					}
				}
			}
			this._disposedValue = true;
		}

		#region IDisposable Members

		public void Dispose()
		{
			// Do not change this code.  Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion
	}
	#endregion

	/// <summary>
	/// Summary description for FileAssertTests.
	/// </summary>
	[TestFixture]
	public class FileAssertTests : MessageChecker
	{
		#region AreEqual

		#region Success Tests
		[Test]
		public void AreEqualPassesWhenBothAreNull()
		{
			FileStream expected = null;
			FileStream actual = null;
			FileAssert.AreEqual( expected, actual );
		}

        [Test]
        public void AreEqualPassesWithSameStream()
        {
            Stream exampleStream = new MemoryStream(new byte[] { 1, 2, 3 });
            Assert.That(exampleStream, Is.EqualTo(exampleStream));
        }

        [Test]
		public void AreEqualPassesWithEqualStreams()
		{
            using (TestFile tf1 = new TestFile("Test1.jpg", "TestImage1.jpg"))
            using (TestFile tf2 = new TestFile("Test2.jpg", "TestImage1.jpg"))
            {
                using (FileStream expected = File.OpenRead("Test1.jpg"))
                {
                    using (FileStream actual = File.OpenRead("Test2.jpg"))
                    {
                        FileAssert.AreEqual(expected, actual);
                    }
                }
            }
        }

        [Test, ExpectedException(typeof(ArgumentException),
            ExpectedMessage = "not readable", MatchType = MessageMatch.Contains)]
        public void NonReadableStreamGivesException()
        {
            using (TestFile tf1 = new TestFile("Test1.jpg", "TestImage1.jpg"))
            using (TestFile tf2 = new TestFile("Test2.jpg", "TestImage1.jpg"))
            {
                using (FileStream expected = File.OpenRead("Test1.jpg"))
                {
                    using (FileStream actual = File.OpenWrite("Test2.jpg"))
                    {
                        FileAssert.AreEqual(expected, actual);
                    }
                }
            }
        }

        [Test, ExpectedException(typeof(ArgumentException),
            ExpectedMessage = "not seekable", MatchType = MessageMatch.Contains)]
        public void NonSeekableStreamGivesException()
        {
            using (TestFile tf1 = new TestFile("Test1.jpg", "TestImage1.jpg"))
            {
                using (FileStream expected = File.OpenRead("Test1.jpg"))
                {
                    using (FakeStream actual = new FakeStream())
                    {
                        FileAssert.AreEqual(expected, actual);
                    }
                }
            }
        }

        private class FakeStream : MemoryStream
        {
            public override bool CanSeek
            {
                get { return false; }
            }
        }

        [Test]
		public void AreEqualPassesWithFiles()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","TestImage1.jpg"))
			using(TestFile tf2 = new TestFile("Test2.jpg","TestImage1.jpg"))
			{
				FileAssert.AreEqual( "Test1.jpg", "Test2.jpg", "Failed using file names" );
			}
		}

		[Test]
		public void AreEqualPassesUsingSameFileTwice()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","TestImage1.jpg"))
			{
				FileAssert.AreEqual( "Test1.jpg", "Test1.jpg" );
			}
		}

		[Test]
		public void AreEqualPassesWithFileInfos()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","TestImage1.jpg"))
			using(TestFile tf2 = new TestFile("Test2.jpg","TestImage1.jpg"))
			{
				FileInfo expected = new FileInfo( "Test1.jpg" );
				FileInfo actual = new FileInfo( "Test2.jpg" );
				FileAssert.AreEqual( expected, actual );
				FileAssert.AreEqual( expected, actual );
			}
		}

		[Test]
		public void AreEqualPassesWithTextFiles()
		{
			using(TestFile tf1 = new TestFile("Test1.txt","TestText1.txt"))
			{
				using(TestFile tf2 = new TestFile("Test2.txt","TestText1.txt"))
				{
					FileAssert.AreEqual( "Test1.txt", "Test2.txt" );
				}
			}
		}
		#endregion

		#region Failure Tests
		[Test,ExpectedException(typeof(AssertionException))]
		public void AreEqualFailsWhenOneIsNull()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","TestImage1.jpg"))
			{
				using(FileStream expected = File.OpenRead("Test1.jpg"))
				{
					expectedMessage = 
						"  Expected: <System.IO.FileStream>" + Environment.NewLine +
						"  But was:  null" + Environment.NewLine;
					FileAssert.AreEqual( expected, null );
				}
			}
		}

		[Test,ExpectedException(typeof(AssertionException))]
		public void AreEqualFailsWithStreams()
		{
			string expectedFile = "Test1.jpg";
			string actualFile = "Test2.jpg";
			using(TestFile tf1 = new TestFile(expectedFile,"TestImage1.jpg"))
			{
				using(TestFile tf2 = new TestFile(actualFile,"TestImage2.jpg"))
				{
					using(FileStream expected = File.OpenRead(expectedFile))
					{
						using(FileStream actual = File.OpenRead(actualFile))
						{
							expectedMessage =
								string.Format("  Expected Stream length {0} but was {1}." + Environment.NewLine,
									new FileInfo(expectedFile).Length, new FileInfo(actualFile).Length);
							FileAssert.AreEqual( expected, actual);
						}
					}
				}
			}
		}

		[Test,ExpectedException(typeof(AssertionException))]
		public void AreEqualFailsWithFileInfos()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","TestImage1.jpg"))
			{
				using(TestFile tf2 = new TestFile("Test2.jpg","TestImage2.jpg"))
				{
					FileInfo expected = new FileInfo( "Test1.jpg" );
					FileInfo actual = new FileInfo( "Test2.jpg" );
					expectedMessage =
						string.Format("  Expected Stream length {0} but was {1}." + Environment.NewLine,
							expected.Length, actual.Length);
					FileAssert.AreEqual( expected, actual );
				}
			}
		}


		[Test,ExpectedException(typeof(AssertionException))]
		public void AreEqualFailsWithFiles()
		{
			string expected = "Test1.jpg";
			string actual = "Test2.jpg";
			using(TestFile tf1 = new TestFile(expected,"TestImage1.jpg"))
			{
				using(TestFile tf2 = new TestFile(actual,"TestImage2.jpg"))
				{
					expectedMessage =
						string.Format("  Expected Stream length {0} but was {1}." + Environment.NewLine,
							new FileInfo(expected).Length, new FileInfo(actual).Length);
					FileAssert.AreEqual( expected, actual );
				}
			}
		}

		[Test,ExpectedException(typeof(AssertionException))]
		public void AreEqualFailsWithTextFilesAfterReadingBothFiles()
		{
			using(TestFile tf1 = new TestFile("Test1.txt","TestText1.txt"))
			{
				using(TestFile tf2 = new TestFile("Test2.txt","TestText2.txt"))
				{
					expectedMessage =
						"  Stream lengths are both 65600. Streams differ at offset 65597." + Environment.NewLine;
					FileAssert.AreEqual( "Test1.txt", "Test2.txt" );
				}
			}
		}
		#endregion

		#endregion

		#region AreNotEqual

		#region Success Tests
		[Test]
		public void AreNotEqualPassesIfOneIsNull()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","TestImage1.jpg"))
			{
				using(FileStream expected = File.OpenRead("Test1.jpg"))
				{
					FileAssert.AreNotEqual( expected, null );
				}
			}
		}

		[Test]
		public void AreNotEqualPassesWithStreams()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","TestImage1.jpg"))
			{
				using(TestFile tf2 = new TestFile("Test2.jpg","TestImage2.jpg"))
				{
					using(FileStream expected = File.OpenRead("Test1.jpg"))
					{
						using(FileStream actual = File.OpenRead("Test2.jpg"))
						{
							FileAssert.AreNotEqual( expected, actual);
						}
					}
				}
			}
		}

		[Test]
		public void AreNotEqualPassesWithFiles()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","TestImage1.jpg"))
			{
				using(TestFile tf2 = new TestFile("Test2.jpg","TestImage2.jpg"))
				{
					FileAssert.AreNotEqual( "Test1.jpg", "Test2.jpg" );
				}
			}
		}

		[Test]
		public void AreNotEqualPassesWithFileInfos()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","TestImage1.jpg"))
			{
				using(TestFile tf2 = new TestFile("Test2.jpg","TestImage2.jpg"))
				{
					FileInfo expected = new FileInfo( "Test1.jpg" );
					FileInfo actual = new FileInfo( "Test2.jpg" );
					FileAssert.AreNotEqual( expected, actual );
				}
			}
		}

		[Test]
		public void AreNotEqualIteratesOverTheEntireFile()
		{
			using(TestFile tf1 = new TestFile("Test1.txt","TestText1.txt"))
			{
				using(TestFile tf2 = new TestFile("Test2.txt","TestText2.txt"))
				{
					FileAssert.AreNotEqual( "Test1.txt", "Test2.txt" );
				}
			}
		}
		#endregion

		#region Failure Tests
		[Test, ExpectedException(typeof(AssertionException))]
		public void AreNotEqualFailsWhenBothAreNull()
		{
			FileStream expected = null;
			FileStream actual = null;
			expectedMessage =
				"  Expected: not null" + Environment.NewLine +
				"  But was:  null" + Environment.NewLine;
			FileAssert.AreNotEqual( expected, actual );
		}

		[Test,ExpectedException(typeof(AssertionException))]
		public void AreNotEqualFailsWithStreams()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","TestImage1.jpg"))
			using(TestFile tf2 = new TestFile("Test2.jpg","TestImage1.jpg"))
			using(FileStream expected = File.OpenRead("Test1.jpg"))
			using(FileStream actual = File.OpenRead("Test2.jpg"))
			{
				expectedMessage = 
					"  Expected: not <System.IO.FileStream>" + Environment.NewLine +
					"  But was:  <System.IO.FileStream>" + Environment.NewLine;
				FileAssert.AreNotEqual( expected, actual );
			}
		}

		[Test,ExpectedException(typeof(AssertionException))]
		public void AreNotEqualFailsWithFileInfos()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","TestImage1.jpg"))
			{
				using(TestFile tf2 = new TestFile("Test2.jpg","TestImage1.jpg"))
				{
					FileInfo expected = new FileInfo( "Test1.jpg" );
					FileInfo actual = new FileInfo( "Test2.jpg" );
					expectedMessage = 
						"  Expected: not <System.IO.FileStream>" + Environment.NewLine +
						"  But was:  <System.IO.FileStream>" + Environment.NewLine;
					FileAssert.AreNotEqual( expected, actual );
				}
			}
		}

		[Test,ExpectedException(typeof(AssertionException))]
		public void AreNotEqualFailsWithFiles()
		{
			using(TestFile tf1 = new TestFile("Test1.jpg","TestImage1.jpg"))
			{
				expectedMessage = 
					"  Expected: not <System.IO.FileStream>" + Environment.NewLine +
					"  But was:  <System.IO.FileStream>" + Environment.NewLine;
				FileAssert.AreNotEqual( "Test1.jpg", "Test1.jpg" );
			}
		}

		[Test,ExpectedException(typeof(AssertionException))]
		public void AreNotEqualIteratesOverTheEntireFileAndFails()
		{
			using(TestFile tf1 = new TestFile("Test1.txt","TestText1.txt"))
			{
				using(TestFile tf2 = new TestFile("Test2.txt","TestText1.txt"))
				{
					expectedMessage = 
						"  Expected: not <System.IO.FileStream>" + Environment.NewLine +
						"  But was:  <System.IO.FileStream>" + Environment.NewLine;
					FileAssert.AreNotEqual( "Test1.txt", "Test2.txt" );
				}
			}
		}
		#endregion

		#endregion
	}
}
