/*
Copyright (c) 2008, Schley Andrew Kutz <sakutz@gmail.com>
All rights reserved.

Redistribution and use in source and binary forms, with or without modification,
are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice,
    this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice,
    this list of conditions and the following disclaimer in the documentation
    and/or other materials provided with the distribution.
    * Neither the name of Schley Andrew Kutz nor the names of its 
    contributors may be used to endorse or promote products derived from this 
    software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Diagnostics;

namespace net.sf.dotnetcli
{
	/// <summary>
	///		Contains useful helper methods for classes within this package.
	/// </summary>
	public class Util
	{
		/// <summary>
		///		Removes the hyphens from the beginning of str and returns the
		///		new String.
		/// </summary>
		/// <param name="str">
		///		The string from which the hyphens should be remoed.
		/// </param>
		/// <returns>The new String.</returns>
		[DebuggerHidden]
		public static string StripLeadingHyphens( String str )
		{
			if ( str == null )
			{
				return null;
			}
			if ( str.StartsWith( "--" ) )
			{
				return JavaPorts.Substring( str, 2, str.Length );
			}
			else if ( str.StartsWith( "-" ) )
			{
				return JavaPorts.Substring( str, 1, str.Length );
			}

			return str;
		}

		/// <summary>
		///		Remove the leading and trailing quotes from str.
		///		E.g. If str is '"one two"', then 'one two' is retruned.
		/// </summary>
		/// <param name="str">
		///		The string from which the leading and trailing quotes
		///		should be removed.
		/// </param>
		/// <returns>
		///		The string without the leading and trailing quotes.
		/// </returns>
		internal static string StripLeadingAndTrailingQuotes( String str )
		{
			if ( str.StartsWith( "\"" ) )
			{
				str = JavaPorts.Substring( str, 1, str.Length );
			}
			if ( str.EndsWith( "\"" ) )
			{
				str = JavaPorts.Substring( str, 0, str.Length - 1 );
			}
			return str;
		}
	}
}