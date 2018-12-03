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
using System.Collections.Generic;
using System.Text;

namespace net.sf.dotnetcli
{
	/// <summary>
	///		Describes a single command-line option.  It maintains
	///		information regarding the short-name of the option, the long-name,
	///		if any exists, a flag indicating if an argument is m_bool_required for
	///		this option, and a self-documenting m_str_description of the option.
	/// 
	///		An Option is not created independantly, but is create through
	///		an instance of Options.
	/// </summary>
	public class Option
	{
		/// <summary>
		///		Constant that specifies the number of argument m_alist_values has not
		///		been specified.
		/// </summary>
		public const int UNINITIALIZED = -1;

		/// <summary>
		///		Constant that specified the number of argument m_alist_values 
		///		is infinite.
		/// </summary>
		public const int UNLIMITED_VALUES = -2;

		/// <summary>
		///		Specified whether this option has an associated argument.
		/// </summary>
		private readonly bool m_bool_has_arg;

		/// <summary>
		///		The name of the option.
		/// </summary>
		private readonly string m_str_opt;

		/// <summary>
		///		The list of argument values.
		/// </summary>
		private List<string> m_alist_values = new List<string>();

		/// <summary>
		///		Specifies whether the argument value of this Option is optional.
		/// </summary>
		private bool m_bool_optional_arg;

		/// <summary>
		///		The character that is the value separator.
		/// </summary>
		private char m_ch_value_sep;

		/// <summary>
		///		 Specifies the number of argument values this option can have.
		/// </summary>
		private int m_int_number_of_args = UNINITIALIZED;

		/// <summary>
		///		Type of this option.
		/// </summary>
		private Type m_obj_type;

		/// <summary>
		///		Specifies the name of the argument for this option.
		/// </summary>
		private string m_str_arg_name = "arg";

		/// <summary>
		///		Description of the option.
		/// </summary>
		private string m_str_description;

		/// <summary>
		///		The representation of the option.
		/// </summary>
		private string m_str_long_opt;

		/// <summary>
		///		Creates an Option using the specified parameters.
		/// </summary>
		/// <param name="opt">
		///		Short representation of the option
		///	</param>
		/// <param name="description">
		///		Describes the function of the option
		///	</param>
		///	<exception cref="ArgumentException">
		///		If there are any non valid Option characters in opt.
		///	</exception>
		public Option( string opt, string description )
			: this( opt, null, false, description )
		{
		}

		/// <summary>
		///		Creates an Option using the specified parameters.
		/// </summary>
		/// <param name="opt">
		///		Short representation of the option
		///	</param>
		/// <param name="hasArg">
		///		Specifies whether the Option takes an argument or not
		/// </param>
		/// <param name="description">
		///		Describes the function of the option
		///	</param>
		///	<exception cref="ArgumentException">
		///		If there are any non valid Option characters in opt.
		///	</exception>
		public Option( string opt, bool hasArg, string description )
			: this( opt, null, hasArg, description )
		{
		}

		/// <summary>
		///		Creates an Option using the specified parameters.
		/// </summary>
		/// <param name="opt">
		///		Short representation of the option
		///	</param>
		///	<param name="longOpt">
		///		The long representation of the option
		///	</param>
		/// <param name="hasArg">
		///		Specifies whether the Option takes an argument or not
		/// </param>
		/// <param name="description">
		///		Describes the function of the option
		///	</param>
		///	<exception cref="ArgumentException">
		///		If there are any non valid Option characters in opt.
		///	</exception>
		public Option( string opt, string longOpt, bool hasArg, string description )
		{
			// ensure that the option is valid
			OptionValidator.ValidateOption( opt );

			m_str_opt = opt;
			m_str_long_opt = longOpt;

			// if m_bool_has_arg is set then the number of arguments is 1
			if ( hasArg )
			{
				m_int_number_of_args = 1;
			}

			m_bool_has_arg = hasArg;
			m_str_description = description;
		}

		/// <summary>
		///		Returns the id of this Option.  This is only set when the
		///		Option shortOpt is a single character.  This is used for switch
		///		statements.
		/// </summary>
		public int ID
		{
			get { return Key[ 0 ]; }
		}

		/// <summary>
		///		Returns the 'unique' Option identifier.
		/// </summary>
		internal string Key
		{
			get
			{
				// if 'm_str_opt' is null, then it is a 'long' option
				if ( m_str_opt == null )
				{
					return m_str_long_opt;
				}

				return m_str_opt;
			}
		}

		/// <summary>
		///		Retrieve the name of this Option.
		///		
		///		It is this string which can be used with CommandLine.HasOption
		///		and CommandLine.GetOptionValue to check for existence and
		///		argument.
		/// </summary>
		public string Opt
		{
			get { return m_str_opt; }
		}

		/// <summary>
		///		Retrieve and set the type of this Option.
		/// </summary>
		public Type Type
		{
			get { return m_obj_type; }
			set { m_obj_type = value; }
		}

		/// <summary>
		///		Retrieve and set the long name of this Option.
		/// </summary>
		public string LongOpt
		{
			get { return m_str_long_opt; }
			set { m_str_long_opt = value; }
		}

		/// <summary>
		///		Sets whether this Option can have an optional argument.
		/// </summary>
		public bool OptionalArg
		{
			set { m_bool_optional_arg = value; }
		}

		/// <summary>
		///		Whether this Option can have an optional argument
		/// </summary>
		public bool HasOptionalArg
		{
			get { return m_bool_optional_arg; }
		}

		/// <summary>
		///		Query to see if this Option has a long name
		/// </summary>
		public bool HasLongOpt
		{
			get { return ( m_str_long_opt != null ); }
		}

		/// <summary>
		///		Query to see if this Option requires an argument
		/// </summary>
		public bool HasArg
		{
			get
			{
				return ( m_int_number_of_args > 0 ) ||
				       ( m_int_number_of_args == UNLIMITED_VALUES );
			}
		}

		/// <summary>
		///		Get and set the self-documenting description of this Option
		/// </summary>
		public string Description
		{
			get { return m_str_description; }
			set { m_str_description = value; }
		}

		/// <summary>
		///		Get and set if this Option requires an argument.
		/// </summary>
		public bool IsRequired { get; set; }

		/// <summary>
		///		Get and set the display name for the argument value.
		/// </summary>
		public string ArgName
		{
			get { return m_str_arg_name; }
			set { m_str_arg_name = value; }
		}

		/// <summary>
		///		Returns whether the display name for the argument value
		///		has been set.
		/// </summary>
		public bool HasArgName
		{
			get { return ( m_str_arg_name != null && m_str_arg_name.Length > 0 ); }
		}

		/// <summary>
		///		Query to see if this Option can take many m_alist_values.
		/// </summary>
		public bool HasArgs
		{
			get
			{
				return ( m_int_number_of_args > 1 ) ||
				       ( m_int_number_of_args == UNLIMITED_VALUES );
			}
		}

		/// <summary>
		///		Gets and sets the value separator character.
		/// </summary>
		public char ValueSeparator
		{
			get { return m_ch_value_sep; }
			set { m_ch_value_sep = value; }
		}

		/// <summary>
		///		Return whether this Option has specified a value separator.
		/// </summary>
		public bool HasValueSeparator
		{
			get { return ( m_ch_value_sep > 0 ); }
		}

		/// <summary>
		///		Gets and sets the number of arguments this Option can take.
		/// </summary>
		public int NumberOfArgs
		{
			get { return m_int_number_of_args; }
			set { m_int_number_of_args = value; }
		}

		/// <summary>
		///		Return the list values of this Option as a string array
		///		or null if there are no list values.
		/// </summary>
		public string[] Values
		{
			get { return HasNoValues ? null : m_alist_values.ToArray(); }
		}

		/// <summary>
		///		The values of this Option as a List or null
		///		if there are no values.
		/// </summary>
		public List<string> ValuesList
		{
			get { return m_alist_values; }
		}

		/// <summary>
		///		Returns whether this Option has any values.
		/// </summary>
		private bool HasNoValues
		{
			get { return m_alist_values.Count == 0; }
		}

		/// <summary>
		///		After calling this method, it is very likely you will want to
		///		call ClearValues().
		/// </summary>
		/// <returns>
		///		The cloned Option.
		/// </returns>
		public object Clone()
		{
			object sixthday = MemberwiseClone();
			( sixthday as Option ).m_alist_values = new List<string>( m_alist_values );
			return ( sixthday );
		}

		/// <summary>
		///		Adds the specified value to this Option.
		/// </summary>
		/// <param name="value">
		///		A/The value of this Option
		/// </param>
		public void AddValueForProcessing( string value )
		{
			switch ( m_int_number_of_args )
			{
				case UNINITIALIZED:
					throw new Exception( "NO_ARGS_ALLOWED" );
				default:
					ProcessValue( value );
					break;
			}
		}

		/// <summary>
		///		Processes the value.  If this Option has a value separator
		///		the value will have to be parsed into individual tokens.  When
		///		n-1 tokens have been processed and there are more value 
		///		separators in the value, parsing is ceased and the remaining 
		///		characters are added as a single token.
		/// </summary>
		/// <param name="value">The string to be processed</param>
		private void ProcessValue( string value )
		{
			// this Option has a separator character
			if ( HasValueSeparator )
			{
				// get the separator character
				char sep = ValueSeparator;

				// store the index for the value separator
				int index = value.IndexOf( sep );

				// while there are more value separators
				while ( index != -1 )
				{
					// next value to be added 
					if ( m_alist_values.Count == ( m_int_number_of_args - 1 ) )
					{
						break;
					}


					// store
					Add( JavaPorts.Substring( value, 0, index ) );


					// Parse
					value = JavaPorts.Substring( value, index + 1 );


					// get new index
					index = value.IndexOf( sep );
				}
			}

			// store the actual value or the last value that has been parsed
			Add( value );
		}

		/// <summary>
		///		Add the value to this Option. If the number of arguments is
		///		greater than zero and there is enough space in the list then
		///		Add the value. Otherwise, throw a runtime exception.
		/// </summary>
		/// <param name="value">
		///		The value to be added to this Option
		/// </param>
		private void Add( string value )
		{
			if ( ( m_int_number_of_args > 0 ) &&
			     ( m_alist_values.Count > ( m_int_number_of_args - 1 ) ) )
			{
				throw new Exception( "Cannot Add value, list full." );
			}

			// store value
			m_alist_values.Add( value );
		}

		/**
		 * Returns the specified value of this Option or 
		 * <code>null</code> if there is no value.
		 *
		 * @return the value/first value of this Option or 
		 * <code>null</code> if there is no value.
		 */
		//
		/// <summary>
		///		Returns the specified value of this Option or null if there is
		///		no value.
		/// </summary>
		/// <returns>
		///		Return the value/first value of this Option or null if there
		///		is no value.
		/// </returns>
		public string GetValue()
		{
			return HasNoValues ? null : m_alist_values[ 0 ];
		}

		/// <summary>
		///		Returns the specified value of this Option or null if there is
		///		no value.
		/// </summary>
		/// <param name="index">
		///		The index of the value to be returned.
		/// </param>
		/// <returns>
		///		The specified value of this Option or null if there is no value.
		/// </returns>
		/// <exception cref="IndexOutOfRangeException">
		///		If index is less than 1 or greater than the number of the list
		///		for this Option.
		/// </exception>
		public string GetValue( int index )
		{
			return HasNoValues ? null : m_alist_values[ index ];
		}

		/// <summary>
		///		Returns the value/first value of this Option or the 
		///		defaultValue if there is no value.
		/// </summary>
		/// <param name="defaultValue">
		///		The value to be returned if there is no value.
		/// </param>
		/// <returns>
		///		defaultValue if there are no list values.
		/// </returns>
		public string GetValue( string defaultValue )
		{
			string value = GetValue();

			return ( value != null ) ? value : defaultValue;
		}

		/// <summary>
		///		Dump state, suitable for debugging.
		/// </summary>
		/// <returns>A stringified version of this Option</returns>
		public override string ToString()
		{
			StringBuilder buf = new StringBuilder().Append( "[ option: " );

			buf.Append( m_str_opt );

			if ( m_str_long_opt != null )
			{
				buf.Append( " " ).Append( m_str_long_opt );
			}

			buf.Append( " " );

			if ( m_bool_has_arg )
			{
				buf.Append( "+ARG" );
			}

			buf.Append( " :: " ).Append( m_str_description );

			if ( m_obj_type != null )
			{
				buf.Append( " :: " ).Append( m_obj_type );
			}

			buf.Append( " ]" );

			return buf.ToString();
		}

		/// <summary>
		///		Override the equals method.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals( object obj )
		{
			if ( this == obj )
			{
				return true;
			}
			if ( obj == null || base.GetType() != obj.GetType() )
			{
				return false;
			}

			Option option = ( Option ) obj;


			if ( m_str_opt != null
			     	? !m_str_opt.Equals( option.m_str_opt )
			     	: option.m_str_opt != null )
			{
				return false;
			}
			if ( m_str_long_opt != null
			     	? !m_str_long_opt.Equals( option.m_str_long_opt )
			     	: option.m_str_long_opt != null )
			{
				return false;
			}

			return true;
		}

		/// <summary>
		///		Get this Option's hash code representation.
		/// </summary>
		/// <returns>
		///		This object's hash code representation.
		/// </returns>
		public override int GetHashCode()
		{
			int result;
			result = ( m_str_opt != null ? m_str_opt.GetHashCode() : 0 );
			result = 31*result +
			         ( m_str_long_opt != null ? m_str_long_opt.GetHashCode() : 0 );
			return result;
		}

		/// <summary>
		///		Clear the Option m_alist_values. After a 
		///		Parse is complete, these are left with data in them 
		///		and they need clearing if another Parse is done.
		/// </summary>
		public void ClearValues()
		{
			m_alist_values.Clear();
		}
	}
}