﻿// ***********************************************************************
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
using System.Text;
using System.Collections;
#if CLR_2_0
using System.Collections.Generic;
#endif

namespace NUnit.AdhocTestRunner
{
    /// <summary>
    /// The CommandLineOptions class parses and holds the values of
    /// any options entered at the command line.
    /// </summary>
    public class CommandLineOptions
    {
        private bool wait = false;
        private bool nologo = false;
        private bool help = false;
        private bool labels = false;
        private bool useappdomain = false;

        private bool error = false;

#if CLR_2_0
        private List<string> loadList = new List<string>();
        private List<string> runList = new List<string>();
        private List<string> invalidOptions = new List<string>();
        private List<string> parameters = new List<string>();
#else
        private ArrayList loadList = new ArrayList();
        private ArrayList runList = new ArrayList();
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
        /// Gets a value indicating whether the 'help' option was used.
        /// </summary>
        public bool Help
        {
            get { return help; }
        }

        /// <summary>
        /// Gets the value of the labels option
        /// </summary>
        public bool Labels
        {
            get { return labels; }
        }

        /// <summary>
        /// Gets value indicating whether a separate AppDomain 
        /// should be used to run tests.
        /// </summary>
        public bool UseAppDomain
        {
            get { return useappdomain; }
        }

        /// <summary>
        /// Gets a list of all tests to be loaded
        /// </summary>
        public IList Load
        {
            get { return loadList; }
        }

        /// <summary>
        /// Gets a list of all tests to be run
        /// </summary>
        public IList Run
        {
            get { return runList; }
        }

        /// <summary>
        /// Parse command arguments and initialize option settings accordingly
        /// </summary>
        /// <param name="args">The argument list</param>
        public void Parse(params string[] args)
        {
            foreach (string arg in args)
            {
                if (arg[0] == '-')
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
            get { return parameters.ToArray(); }
#else
            get { return (string[])parameters.ToArray(typeof(string)); }
#endif
        }

        private void ProcessOption(string opt)
        {
            int pos = opt.IndexOfAny(new char[] { ':', '=' });
            string val = string.Empty;

            if (pos >= 0)
            {
                val = opt.Substring(pos + 1);
                opt = opt.Substring(0, pos);
            }

            switch (opt.Substring(1))
            {
                case "w":
                case "wait":
                    wait = true;
                    break;
                case "nologo":
                    nologo = true;
                    break;
                case "h":
                case "help":
                    help = true;
                    break;
                case "load":
                    loadList.Add(val);
                    break;
                case "r":
                case "run":
                    runList.Add(val);
                    break;
                case "l":
                case "labels":
                    labels = true;
                    break;
                case "a":
                case "appdomain":
                    useappdomain = true;
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
            if (parameters.Count > 1)
                error = true;
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
                if (invalidOptions.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string opt in invalidOptions)
                        sb.Append("Invalid option: " + opt + Environment.NewLine);
                    return sb.ToString();
                }
                else if (parameters.Count > 1)
                    return "Only one assembly may be loaded at a time";
                else
                    return "Unknown error";
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
                string NL = Environment.NewLine;

                string name = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;

                sb.Append(NL + name + " [assemblies] [options]" + NL + NL);
                sb.Append(" Runs a set of NUnitLite tests from the console." + NL + NL);
                sb.Append("You may specify one or more test assemblies by name, without a path or" + NL);
                sb.Append("extension. They must be in the same in the same directory as the exe" + NL);
                sb.Append("or on the probing path. If no assemblies are provided, tests in the" + NL);
                sb.Append("executing assembly itself are run." + NL + NL);
                sb.Append("Options:" + NL);
                sb.Append("  -f[ixture]:name   Provides the name of a test to load. This option may be" + NL);
                sb.Append("                    repeated. If no test names are given, all tests are loaded." + NL + NL);
                sb.Append("  -r[un]:name       Provides the name of a test to run. This option may be" + NL);
                sb.Append("                    repeated. If no test names are given, all loaded tests are run." + NL + NL);
                sb.Append("  -h[elp]           Displays this help" + NL + NL);
                sb.Append("  -nologo           Suppresses display of the initial message" + NL + NL);
                sb.Append("  -w[ait]           Waits for a key press before exiting" + NL + NL);
                sb.Append("  -l[abels]       Display name of each test as it is run" + NL + NL);
                sb.Append("  -a[ppdomain]      Run tests in a separate AppDomain" + NL + NL);
                sb.Append("Options that take values may use an equal sign or a colon" + NL);
                sb.Append("to separate the option from its value." + NL + NL);

                return sb.ToString();
            }
        }
    }
}
