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

namespace net.sf.dotnetcli
{
	/// <summary>
	///		Validates an Option string
	/// </summary>
	public class OptionValidator
	{
		/// <summary>
		///		Validates whether m_str_opt is a permissable Option shortOpt.
		///		The rules that specify if the m_str_opt is valid are:
		///		
		///			- m_str_opt is not NULL
		///			- A single character m_str_opt that is either (special case), 
		///			  '?', '@', or a letter
		///			- A multi-character m_str_opt that only contains letters.
		/// </summary>
		/// <param name="m_str_opt">
		///		The option string to validate
		/// </param>
		/// <exception cref="ArgumentException">
		///		If the Option is not valid
		/// </exception>
		internal static void ValidateOption( String opt )
		{
			// check that m_str_opt is not NULL
			if ( opt == null )
			{
				return;
			}

				// handle the single character m_str_opt
			else if ( opt.Length == 1 )
			{
				char ch = opt[ 0 ];

				if ( !IsValidOpt( ch ) )
				{
					throw new ArgumentException( "illegal option value '" + ch + "'" );
				}
			}

				// handle the multi character m_str_opt
			else
			{
				char[] chars = opt.ToCharArray();

				for ( int i = 0; i < chars.Length; i++ )
				{
					if ( !IsValidChar( chars[ i ] ) )
					{
						throw new ArgumentException(
							"m_str_opt contains illegal character value '" + chars[ i ] + "'" );
					}
				}
			}
		}

		/// <summary>
		///		Returns whether the specified character is a valid Option.
		/// </summary>
		/// <param name="c">
		///		The option to validate
		/// </param>
		/// <returns>
		///		True if c is a letter, ' ', '?', or '@', otherwise false.
		/// </returns>
		private static bool IsValidOpt( char c )
		{
			return ( IsValidChar( c ) || ( c == ' ' ) || ( c == '?' ) || c == '@' );
		}

		/// <summary>
		///		Returns whether the specified character is a valid character.
		/// </summary>
		/// <param name="c">
		///		The character to validate
		/// </param>
		/// <returns>
		///		True if c is a letter
		/// </returns>
		private static bool IsValidChar( char c )
		{
			return ( char.IsLetterOrDigit( c ) || c == '$' || c == '_' );
		}
	}
}