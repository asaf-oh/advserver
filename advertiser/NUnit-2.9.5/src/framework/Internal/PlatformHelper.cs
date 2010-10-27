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

namespace NUnit.Framework.Internal
{
    /// <summary>
    /// PlatformHelper class is used by the PlatformAttribute class to 
    /// determine whether a platform is supported.
    /// </summary>
	public class PlatformHelper
	{
		private OSPlatform os;
		private RuntimeFramework rt;

		// Set whenever we fail to support a list of platforms
		private string reason = string.Empty;

        /// <summary>
		/// Comma-delimited list of all supported OS platform constants
		/// </summary>
		public static readonly string OSPlatforms =
			"Win,Win32,Win32S,Win32NT,Win32Windows,WinCE,Win95,Win98,WinMe,NT3,NT4,NT5,NT6,Win2K,WinXP,Win2003Server,Vista,Win2008Server,Unix,Linux";
		
		/// <summary>
		/// Comma-delimited list of all supported Runtime platform constants
		/// </summary>
		public static readonly string RuntimePlatforms =
			"Net,NetCF,SSCLI,Rotor,Mono";

		/// <summary>
		/// Default constructor uses the operating system and
		/// common language runtime of the system.
		/// </summary>
		public PlatformHelper()
		{
			this.os = OSPlatform.CurrentPlatform;
			this.rt = RuntimeFramework.CurrentFramework;
		}

		/// <summary>
		/// Contruct a PlatformHelper for a particular operating
		/// system and common language runtime. Used in testing.
		/// </summary>
		/// <param name="os">OperatingSystem to be used</param>
        /// <param name="rt">RuntimeFramework to be used</param>
		public PlatformHelper( OSPlatform os, RuntimeFramework rt )
		{
			this.os = os;
			this.rt = rt;
		}

		/// <summary>
		/// Test to determine if one of a collection of platforms
		/// is being used currently.
		/// </summary>
		/// <param name="platforms"></param>
		/// <returns></returns>
		public bool IsPlatformSupported( string[] platforms )
		{
			foreach( string platform in platforms )
				if ( IsPlatformSupported( platform ) )
					return true;

			return false;
		}

		/// <summary>
		/// Tests to determine if the current platform is supported
		/// based on a platform attribute.
		/// </summary>
		/// <param name="platformAttribute">The attribute to examine</param>
		/// <returns></returns>
		public bool IsPlatformSupported( PlatformAttribute platformAttribute )
		{
            string include = platformAttribute.Include;
            string exclude = platformAttribute.Exclude;

			try
			{
				if (include != null && !IsPlatformSupported(include))
				{
					reason = string.Format("Only supported on {0}", include);
					return false;
				}

				if (exclude != null && IsPlatformSupported(exclude))
				{
					reason = string.Format("Not supported on {0}", exclude);
					return false;
				}
			}
			catch( ArgumentException ex )
			{
				reason = string.Format( "Invalid platform name: {0}", ex.ParamName );
				return false; 
			}

			return true;
		}

		/// <summary>
		/// Test to determine if the a particular platform or comma-
		/// delimited set of platforms is in use.
		/// </summary>
		/// <param name="platform">Name of the platform or comma-separated list of platform names</param>
		/// <returns>True if the platform is in use on the system</returns>
		public bool IsPlatformSupported( string platform )
		{
			if ( platform.IndexOf( ',' ) >= 0 )
				return IsPlatformSupported( platform.Split( new char[] { ',' } ) );

			string platformName = platform.Trim();
			bool nameOK = false;

			string versionSpecification = null;

			string[] parts = platformName.Split( new char[] { '-' } );
			if ( parts.Length == 2 )
			{
				platformName = parts[0];
				versionSpecification = parts[1];
			}

			switch( platformName.ToUpper() )
			{
				case "WIN":
				case "WIN32":
					nameOK = os.IsWindows;
					break;
				case "WIN32S":
                    nameOK = os.IsWin32S;
					break;
				case "WIN32WINDOWS":
					nameOK = os.IsWin32Windows;
					break;
				case "WIN32NT":
					nameOK = os.IsWin32NT;
					break;
				case "WINCE":
                    nameOK = os.IsWinCE;
					break;
				case "WIN95":
                    nameOK = os.IsWin95;
					break;
				case "WIN98":
                    nameOK = os.IsWin98;
					break;
				case "WINME":
					nameOK = os.IsWinME;
					break;
				case "NT3":
                    nameOK = os.IsNT3;
					break;
				case "NT4":
                    nameOK = os.IsNT4;
					break;
                case "NT5":
                    nameOK = os.IsNT5;
                    break;
                case "WIN2K":
                    nameOK = os.IsWin2K;
					break;
				case "WINXP":
                    nameOK = os.IsWinXP;
					break;
				case "WIN2003SERVER":
                    nameOK = os.IsWin2003Server;
					break;
                case "NT6":
                    nameOK = os.IsNT6;
                    break;
                case "VISTA":
                    nameOK = os.IsVista;
                    break;
                case "WIN2008SERVER":
                    nameOK = os.IsWin2008Server;
                    break;
                case "UNIX":
				case "LINUX":
                    nameOK = os.IsUnix;
					break;
				case "NET":
					nameOK = rt.IsNet;
					break;
				case "NETCF":
					nameOK = rt.IsNetCF;
					break;
				case "SSCLI":
				case "ROTOR":
					nameOK = rt.IsSSCLI;
					break;
				case "MONO":
                    nameOK = rt.IsMono;
					// Special handling because Mono 1.0 profile has version 1.1
					if ( versionSpecification == "1.0" )
						versionSpecification = "1.1";
					break;
				default:
					throw new ArgumentException( "Invalid platform name", platform.ToString() );
			}

			if ( nameOK ) 
			{
				if ( versionSpecification == null )
					return true;

				Version version = new Version( versionSpecification );

				if ( rt.Version.Major == version.Major &&
					 rt.Version.Minor == version.Minor &&
				   ( version.Build == -1 || rt.Version.Build == version.Build ) &&
				   ( version.Revision == -1 || rt.Version.Revision == version.Revision ) )
						return true;
			}

			this.reason = "Only supported on " + platform;
			return false;
		}

		/// <summary>
		/// Return the last failure reason. Results are not
		/// defined if called before IsSupported( Attribute )
		/// is called.
		/// </summary>
		public string Reason
		{
			get { return reason; }
		}
	}
}
