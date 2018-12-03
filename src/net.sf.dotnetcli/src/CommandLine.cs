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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace net.sf.dotnetcli
{
	/// <summary>
	///		<para>Represents list of arguments parsed against an 
	///		<see cref="Options"/> descriptor.</para>
	///		
	///		<para>It allows querying of a bool <see cref="HasOption"/>, 
	///		in addition to retrieving the <
	///		see cref="GetOptionValue( string opt )"/> 
	///		for options requiring arguments.</para>
	///		
	///		<para>Additionally, any left-over or unrecognized arguments are 
	///		available for further processing.</para>
	/// </summary>
	public class CommandLine : IEnumerable
	{
		/// <summary>
		///		The unrecognised options/arguments
		/// </summary>
		private readonly List<string> args = new List<string>();

		/// <summary>
		///		The processed options
		/// </summary>
		private readonly List<Option> options = new List<Option>();

		/// <summary>
		///		Create a command line
		/// </summary>
		internal CommandLine()
		{
			// nothing to do
		}

		/// <summary>
		///		Retrieve any left-over non-recognized options and arguments
		/// </summary>
		public String[] Args
		{
			get { return ( args.ToArray() ); }
		}

		/// <summary>
		///		Retrieve any left-over non-recognized options and arguments
		///		as a <code>List</code>
		/// </summary>
		public List<string> ArgList
		{
			get { return args; }
		}

		/// <summary>
		///		Returns an array of the processed Options.
		/// </summary>
		public Option[] Options
		{
			get { return ( options.ToArray() ); }
		}

		#region IEnumerable Members

		/// <summary>
		///		Returns an enumator over the Option members of CommandLine.
		/// </summary>
		/// <returns>The enumator</returns>
		public IEnumerator GetEnumerator()
		{
			return options.GetEnumerator();
		}

		#endregion

		/// <summary>
		///		Query to see if an option has been set.
		/// </summary>
		/// <param name="opt">
		///		Short name of the option
		/// </param>
		/// <returns>
		///		True if set, false if not
		/// </returns>
		public bool HasOption( String opt )
		{
			return options.Contains( ResolveOption( opt ) );
		}

		/// <summary>
		///		Query to see if an option has been set.
		/// </summary>
		/// <param name="opt">
		///		Character name of the option
		/// </param>
		/// <returns>
		///		True if set, false if not
		/// </returns>
		[DebuggerHidden]
		public bool HasOption( char opt )
		{
			return HasOption( Convert.ToString( opt ) );
		}

		/// <summary>
		///		Return the <code>Object</code> type of this Option.
		/// </summary>
		/// <param name="opt">
		///		The name of the option
		/// </param>
		/// <returns>
		///		The type of this <code>Option</code>
		/// </returns>
		public Object GetOptionObject( String opt )
		{
			String res = GetOptionValue( opt );

			Option option = ResolveOption( opt );
			if ( option == null )
			{
				return null;
			}

			Type type = option.Type;

			return ( res == null ) ? null : TypeHandler.CreateValue( res, type );
		}

		/// <summary>
		///		The <code>Object</code> type of this <code>Option</code>.
		/// </summary>
		/// <param name="opt">
		///		The name of the option
		/// </param>
		/// <returns>
		///		The type of opt
		/// </returns>
		public Object GetOptionObject( char opt )
		{
			return GetOptionObject( Convert.ToString( opt ) );
		}

		/// <summary>
		///		Retrieve the argument, if any, of this option.
		/// </summary>
		/// <param name="opt">
		///		The name of the option
		/// </param>
		/// <returns>
		///		Value of the argument if option is set, and has an argument,
		///		otherwise null.
		/// </returns>
		public String GetOptionValue( String opt )
		{
			String[] values = GetOptionValues( opt );

			return ( values == null ) ? null : values[ 0 ];
		}

		/// <summary>
		///		Retrieve the argument, if any, of this option.
		/// </summary>
		/// <param name="opt">
		///		The character name of the option
		/// </param>
		/// <returns>
		///		Value of the argument if option is set, and has an argument,
		///		otherwise null.
		/// </returns>
		public String GetOptionValue( char opt )
		{
			return GetOptionValue( Convert.ToString( opt ) );
		}

		/// <summary>
		///		Retrieves the array of values, if any, of an option
		/// </summary>
		/// <param name="opt">
		///		String name of the option
		/// </param>
		/// <returns>
		///		Values of the argument if option is set, and has an argument,
		///		otherwise null.
		/// </returns>
		public String[] GetOptionValues( String opt )
		{
			Option key = ResolveOption( opt );

			if ( options.Contains( key ) )
			{
				return key.Values;
			}

			return null;
		}

		/// <summary>
		///		Retrieves the option objet given the long or short option as a
		///		String
		/// </summary>
		/// <param name="opt">
		///		Short or long name of the option
		/// </param>
		/// <returns>
		///		Canonicalized option
		/// </returns>
		[DebuggerHidden]
		private Option ResolveOption( String opt )
		{
			opt = Util.StripLeadingHyphens( opt );
			foreach ( Option option in options )
			{
				if ( opt.Equals( option.Opt ) )
				{
					return option;
				}
				if ( opt.Equals( option.LongOpt ) )
				{
					return option;
				}
			}

			return null;
		}

		/// <summary>
		///		Retrieves the array of values, if any, of an option.
		/// </summary>
		/// <param name="opt">
		///		Character name of the option
		/// </param>
		/// <returns>
		///		Values of an argument if option is set, and has an argument,
		///		otherwise null.
		/// </returns>
		public String[] GetOptionValues( char opt )
		{
			return GetOptionValues( Convert.ToString( opt ) );
		}

		/// <summary>
		///		Retrieve the argument, if any, of an option.
		/// </summary>
		/// <param name="opt">
		///		Name of the option
		/// </param>
		/// <param name="defaultValue">
		///		The default value to be returned if the option is not 
		///		specified
		/// </param>
		/// <returns>
		///		Value of the argument if option is set, and has an argument,
		///		otherwise <code>defaultValue</code>.
		/// </returns>
		public String GetOptionValue( String opt, String defaultValue )
		{
			String answer = GetOptionValue( opt );

			return ( answer != null ) ? answer : defaultValue;
		}

		/// <summary>
		///		Retrieve the argument, if any, of an option.
		/// </summary>
		/// <param name="opt">
		///		Character name of the option
		/// </param>
		/// <param name="defaultValue">
		///		The default value to be returned if the option is not specified
		/// </param>
		/// <returns>
		///		Value of the argument if option is set, and has an argument,
		///		otherwise <code>defaultValue</code>.
		/// </returns>
		public String GetOptionValue( char opt, String defaultValue )
		{
			return GetOptionValue( Convert.ToString( opt ), defaultValue );
		}

		/// <summary>
		///		Add left-over unrecognized option/argument.
		/// </summary>
		/// <param name="arg">
		///		The unrecognized option/argument.
		/// </param>
		internal void AddArg( String arg )
		{
			args.Add( arg );
		}

		/// <summary>
		///		Add an option to the command line. The values of the option are
		///		stored.
		/// </summary>
		/// <param name="opt">The processed option</param>
		internal void AddOption( Option opt )
		{
			options.Add( opt );
		}
	}
}