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
	public class TypeHandler
	{
		/// <summary>
		///		Returns the Object of type obj with the value of str.
		/// </summary>
		/// <param name="str">
		///		The command line value
		/// </param>
		/// <param name="clazz">
		///		The type of argument
		/// </param>
		/// <returns>
		///		The instance of obj initialized with the value of str
		/// </returns>
		public static Object CreateValue( string str, Type clazz )
		{
			if ( PatternOptionBuilder.STRING_VALUE == clazz )
			{
				return str;
			}
			else if ( PatternOptionBuilder.NUMBER_VALUE == clazz )
			{
				return CreateNumber( str );
			}
			else if ( PatternOptionBuilder.DATE_VALUE == clazz )
			{
				return CreateDate( str );
			}
			else if ( PatternOptionBuilder.CLASS_VALUE == clazz )
			{
				return CreateClass( str );
			}
			else if ( PatternOptionBuilder.FILE_VALUE == clazz )
			{
				return CreateFile( str );
			}
			else if ( PatternOptionBuilder.EXISTING_FILE_VALUE == clazz )
			{
				return CreateFile( str );
			}
			else if ( PatternOptionBuilder.FILES_VALUE == clazz )
			{
				return CreateFiles( str );
			}
			else if ( PatternOptionBuilder.URL_VALUE == clazz )
			{
				return CreateUrl( str );
			}
			else if ( PatternOptionBuilder.OBJECT_VALUE == clazz )
			{
				return CreateObject( str );
			}
			else
			{
				return null;
			}
		}

		/**
      * <p>Create an Object from the classname and empty constructor.</p>
      *
      * @param str the argument value
      * @return the initialised object, or null if it couldn't create 
      * the Object.
      */

		public static Object CreateObject( String str )
		{
			Type cl = null;

			try
			{
				cl = Type.GetType( str );
			}
			catch ( Exception cnfe )
			{
				Console.WriteLine( "Unable to find: " + str );

				return null;
			}

			Object instance = null;

			try
			{
				instance = Activator.CreateInstance( cl, null );
			}
			catch ( Exception cnfe )
			{
				Console.WriteLine( "InstantiationException; Unable to create: " + str );

				return null;
			}

			return instance;
		}

		/**
		 * <p>Create a number from a String. If a . is present, it creates a 
		 *    Double, otherwise a Long. </p>
		 *
		 * @param str the value
		 * @return the number represented by <code>str</code>, if <code>str</code>
		 * is not a number, null is returned.
		 */

		public static double CreateNumber( String str )
		{
			/*
			try
			{
				if ( str != null )
				{
					if ( str.indexOf( '.' ) != -1 )
					{
						return Double.valueOf( str );
					}
					else
					{
						return Long.valueOf( str );
					}
				}
			}
			catch ( NumberFormatException nfe )
			{
				System.err.println( nfe.getMessage() );
			}

			return null;
			*/
			try
			{
				return ( double.Parse( str ) );
			}
			catch ( Exception  e )
			{
				Console.WriteLine( e.Message );
				return double.MinValue;
			}
		}

		/**
		 * <p>Returns the class whose name is <code>str</code>.</p>
		 *
		 * @param str the class name
		 * @return The class if it is found, otherwise return null
		 */

		public static Type CreateClass( String str )
		{
			try
			{
				Type t = Type.GetType( str );
				return t;
			}
			catch ( Exception cnfe )
			{
				Console.WriteLine( "Unable to find: " + str );
				return null;
			}
		}

		/**
		 * <p>Returns the date represented by <code>str</code>.</p>
		 *
		 * @param str the date string
		 * @return The date if <code>str</code> is a valid date string,
		 * otherwise return null.
		 */

		public static DateTime CreateDate( String str )
		{
			try
			{
				return ( DateTime.Parse( str ) );
			}
			catch
			{
				Console.WriteLine( "Unable to parse: " + str );
				return DateTime.MinValue;
			}
		}

		/**
		 * <p>Returns the URL represented by <code>str</code>.</p>
		 *
		 * @param str the URL string
		 * @return The URL is <code>str</code> is well-formed, otherwise
		 * return null.
		 */

		public static Uri CreateUrl( String str )
		{
			try
			{
				return new Uri( str );
			}
			catch ( Exception mue )
			{
				Console.WriteLine( "Unable to parse: " + str );
				return null;
			}
		}

		/**
		 * <p>Returns the File represented by <code>str</code>.</p>
		 *
		 * @param str the File location
		 * @return The file represented by <code>str</code>.
		 */

		public static FileInfo CreateFile( String str )
		{
			return new FileInfo( str );
		}

		/**
		 * <p>Returns the File[] represented by <code>str</code>.</p>
		 *
		 * @param str the paths to the files
		 * @return The File[] represented by <code>str</code>.
		 */

		public static FileInfo[] CreateFiles( String str )
		{
			// to implement/port:
			//        return FileW.findFiles(str);
			return null;
		}
	}
}