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
using System.IO;

namespace net.sf.dotnetcli
{
	/// <summary>
	///		<para>Allows Options to be created from a single String.
	///		The pattern contains various single character flags and via
	///		an optional punctuation character, their expected type.</para>
	///		<list type="table">
	///			<item>a    -a flag</item>
	///			<item>b@   -b [classname]</item>
	///			<item>c&gt;-c [filename]</item>
	///			<item>d+   -d [classname] (creates object via empty 
	///			constructor</item>
	///			<item>e%   -e [number]</item>
	///			<item>f/   -f [url]</item>
	///			<item>g:   -g [string]</item>
	///			<item>t#...-t [date]</item>
	///		</list>
	/// </summary>
	public class PatternOptionBuilder
	{
		/** String class */

		/** Class class */
		public static Type CLASS_VALUE = typeof ( Type );
		public static Type DATE_VALUE = typeof ( DateTime );

		/// can we do this one?? 
		// is meant to check that the file exists, else it errors.
		// ie) it's for reading not writing.
		/** FileInputStream class */
		public static Type EXISTING_FILE_VALUE = typeof ( FileStream );

		/** File class */
		public static Type FILE_VALUE = typeof ( FileInfo );

		/** File array class */
		public static Type FILES_VALUE = typeof ( FileInfo[] );
		public static Type NUMBER_VALUE = typeof ( int );
		public static Type OBJECT_VALUE = typeof ( object );
		public static Type STRING_VALUE = typeof ( string );

		/** URL class */
		public static Type URL_VALUE = typeof ( Uri );

		/**
		 * <p>Retrieve the class that <code>ch</code> represents.</p>
		 *
		 * @param ch the specified character
		 * @return The class that <code>ch</code> represents
		 */

		public static Type GetValueClass( char ch )
		{
			if ( ch == '@' )
			{
				return OBJECT_VALUE;
			}
			else if ( ch == ':' )
			{
				return STRING_VALUE;
			}
			else if ( ch == '%' )
			{
				return NUMBER_VALUE;
			}
			else if ( ch == '+' )
			{
				return CLASS_VALUE;
			}
			else if ( ch == '#' )
			{
				return DATE_VALUE;
			}
			else if ( ch == '<' )
			{
				return EXISTING_FILE_VALUE;
			}
			else if ( ch == '>' )
			{
				return FILE_VALUE;
			}
			else if ( ch == '*' )
			{
				return FILES_VALUE;
			}
			else if ( ch == '/' )
			{
				return URL_VALUE;
			}

			return null;
		}

		/**
		 * <p>Returns whether <code>ch</code> is a value code, i.e.
		 * whether it represents a class in a pattern.</p>
		 * 
		 * @param ch the specified character
		 * @return true if <code>ch</code> is a value code, otherwise false.
		 */

		public static bool IsValueCode( char ch )
		{
			if ( ( ch != '@' ) && ( ch != ':' ) && ( ch != '%' ) && ( ch != '+' ) &&
			     ( ch != '#' ) && ( ch != '<' ) && ( ch != '>' ) && ( ch != '*' ) &&
			     ( ch != '/' ) && ( ch != '!' ) )
			{
				return false;
			}

			return true;
		}

		/**
		 * <p>Returns the {@link Options} instance represented by 
		 * <code>pattern</code>.</p>
		 *
		 * @param pattern the pattern string
		 * @return The {@link Options} instance
		 */

		public static Options ParsePattern( String pattern )
		{
			int sz = pattern.Length;

			char opt = ' ';
			char ch = ' ';
			bool required = false;
			Type type = null;

			Options options = new Options();

			for ( int i = 0; i < sz; i++ )
			{
				ch = pattern[ i ];

				// a value code comes after an option and specifies 
				// details about it
				if ( !IsValueCode( ch ) )
				{
					if ( opt != ' ' )
					{
						OptionBuilder.Factory.HasArg( type != null );
						OptionBuilder.Factory.IsRequired( required );
						OptionBuilder.Factory.WithType( type );

						// we have a previous one to deal with
						options.AddOption( OptionBuilder.Factory.Create( opt ) );
						required = false;
						type = null;
						opt = ' ';
					}

					opt = ch;
				}
				else if ( ch == '!' )
				{
					required = true;
				}
				else
				{
					type = GetValueClass( ch );
				}
			}

			if ( opt != ' ' )
			{
				OptionBuilder.Factory.HasArg( type != null );
				OptionBuilder.Factory.IsRequired( required );
				OptionBuilder.Factory.WithType( type );

				// we have a final one to deal with
				options.AddOption( OptionBuilder.Factory.Create( opt ) );
			}

			return options;
		}
	}
}