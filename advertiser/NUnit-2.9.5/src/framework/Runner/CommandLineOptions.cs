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
using System.Collections;
#if CLR_2_0
using System.Collections.Generic;
#endif

namespace NUnitLite.Runner
{
    /// <summary>
    /// The CommandLineOptions class parses and holds the values of
    /// any options entered at the command line.
    /// </summary>
    public class CommandLineOptions
    {
        private string optionChars;
        private static string NL = NUnit.Env.NewLine;

        private bool wait = false;
        private bool nologo = false;
        private bool listprops = false;
        private bool help = false;
        private bool full = false;

        private bool error = false;

#if CLR_2_0
        private List<string> tests = new List<string>();
        private List<string> invalidOptions = new List<string>();
        private List<string> parameters = new List<string>();
#else
        private ArrayList tests = new ArrayList();
        private ArrayList invalidOptions = new ArrayList();
        private ArrayList parameters = new ArrayList();
#endif

        /// <summary>
        /// Gets a value indicating whether the 'wait' option was used.
        /// </summary>
        public bool Wait
        {
            get { return wait; }
        }

        /// <summary>
        /// Gets a value indicating whether the 'nologo' option was used.
        /// </summary>
        public bool Nologo
        {
            get { return nologo; }
        }

        /// <summary>
        /// Gets a value indicating whether the 'listprops' option was used.
        /// </summary>
        public bool ListProperties
        {
            get { return listprops; }
        }

        /// <summary>
        /// Gets a value indicating whether the 'help' option was used.
        /// </summary>
        public bool Help
        {
            get { return help; }
        }

        /// <summary>
        /// Gets a list of all tests specified on the command line
        /// </summary>
        public string[] Tests
        {
            get { return (string[])tests.ToArray(); }
        }

        /// <summary>
        /// Gets a value indicating whether a full report should be displayed
        /// </summary>
        public bool Full
        {
            get { return full; }
        }

        /// <summary>
        /// Gets the test count
        /// </summary>
        public int TestCount
        {
            get { return tests.Count; }
        }

        /// <summary>
        /// Construct a CommandLineOptions object using default option chars
        /// </summary>
        public CommandLineOptions()
        {
            this.optionChars = System.IO.Path.DirectorySeparatorChar == '/' ? "-" : "/-";
        }

        /// <summary>
        /// Construct a CommandLineOptions object using specified option chars
        /// </summary>
        /// <param name="optionChars"></param>
        public CommandLineOptions(string optionChars)
        {
            this.optionChars = optionChars;
        }

        /// <summary>
        /// Parse command arguments and initialize option settings accordingly
        /// </summary>
        /// <param name="args">The argument list</param>
        public void Parse(params string[] args)
        {
            foreach( string arg in args )
            {
                if (optionChars.IndexOf(arg[0]) >= 0 )
                    ProcessOption(arg);
                else
                    ProcessParameter(arg);
            }
        }

        /// <summary>
        ///  Gets the parameters provided on the commandline
        /// </summary>
        public string[] Parameters
        {
#if CLR_2_0
            get { return (string[])parameters.ToArray(); }
#else
            get { return (string[])parameters.ToArray(typeof(string)); }
#endif
        }

        private void ProcessOption(string opt)
        {
            int pos = opt.IndexOfAny( new char[] { ':', '=' } );
            string val = string.Empty;

            if (pos >= 0)
            {
                val = opt.Substring(pos + 1);
                opt = opt.Substring(0, pos);
            }

            switch (opt.Substring(1))
            {
                case "wait":
                    wait = true;
                    break;
                case "nologo":
                    nologo = true;
                    break;
                case "help":
                    help = true;
                    break;
                case "props":
                    listprops = true;
                    break;
                case "test":
                    tests.Add(val);
                    break;
                case "full":
                    full = true;
                    break;
                default:
                    error = true;
                    invalidOptions.Add(opt);
                    break;
            }
        }

        private void ProcessParameter(string param)
        {
            parameters.Add(param);
        }

        /// <summary>
        /// Gets a value indicating whether there was an error in parsing the options.
        /// </summary>
        /// <value><c>true</c> if error; otherwise, <c>false</c>.</value>
        public bool Error
        {
            get { return error; }
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value>The error message.</value>
        public string ErrorMessage
        {
            get 
            {
                StringBuilder sb = new StringBuilder();
                foreach (string opt in invalidOptions)
                    sb.Append( "Invalid option: " + opt + NL );
                return sb.ToString();
            }
        }

        /// <summary>
        /// Gets the help text.
        /// </summary>
        /// <value>The help text.</value>
        public string HelpText
        {
            get
            {
                StringBuilder sb = new StringBuilder();

#if PocketPC || WindowsCE || NETCF
                string name = "NUnitLite";
#else
                string name = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
#endif

                sb.Append(NL + name + " [assemblies] [options]" + NL + NL);
                sb.Append(" Runs a set of NUnitLite tests from the console." + NL + NL);
                sb.Append("You may specify one or more test assemblies by name, without a path or" + NL);
                sb.Append("extension. They must be in the same in the same directory as the exe" + NL);
                sb.Append("or on the probing path. If no assemblies are provided, tests in the" + NL);
                sb.Append("executing assembly itself are run." + NL + NL);
                sb.Append("Options:" + NL);
                sb.Append("  -test:testname  Provides the name of a test to run. This option may be" + NL);
                sb.Append("                  repeated. If no test names are given, all tests are run." + NL + NL);
                sb.Append("  -help           Displays this help" + NL + NL);
                sb.Append("  -nologo         Suppresses display of the initial message" + NL + NL);
                sb.Append("  -wait           Waits for a key press before exiting" + NL + NL);
                sb.Append("  -full           Prints full report of all test results." + NL + NL);
                if (System.IO.Path.DirectorySeparatorChar != '/')
                    sb.Append("On Windows, options may be prefixed by a '/' character if desired" + NL + NL);
                sb.Append("Options that take values may use an equal sign or a colon" + NL);
                sb.Append("to separate the option from its value." + NL + NL);

                return sb.ToString();
            }
        }
    }
}
