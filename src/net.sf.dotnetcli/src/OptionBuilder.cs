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
	///		OptionBuilder allows the user to create Options using descriptive
	///		methods.
	///		
	///		Details on the Builder pattern can be found at 
	///		http://c2.com/cgi-bin/wiki?BuilderPattern.
	/// </summary>
	public class OptionBuilder
	{
		/** long option */

		/** argument name */
		private static readonly OptionBuilder instance = new OptionBuilder();
		private static String argName;
		private static String description;
		private static String longopt;

		/** is required? */

		/** the number of arguments */
		private static int numberOfArgs = Option.UNINITIALIZED;

		/** option type */

		/** option can have an optional argument value */
		private static bool optionalArg;
		private static bool required;
		private static Type type;

		/** value separator for argument value */
		private static char valuesep;

		/** option builder instance */

		/**
		 * private constructor to prevent instances being created
		 */

		private OptionBuilder()
		{
			// hide the constructor
		}

		/// <summary>
		///		Returns a static instance of OptionBuilder.
		/// </summary>
		public static OptionBuilder Factory
		{
			get { return instance; }
		}

		/**
		 * Resets the member variables to their default values.
		 */

		private static void Reset()
		{
			description = null;
			argName = "arg";
			longopt = null;
			type = null;
			required = false;
			numberOfArgs = Option.UNINITIALIZED;


			// PMM 9/6/02 - these were missing
			optionalArg = false;
			valuesep = ( char ) 0;
		}

		/**
		 * The next Option created will have the following long option value.
		 *
		 * @param newLongopt the long option value
		 * @return the OptionBuilder instance
		 */

		public OptionBuilder WithLongOpt( String newLongopt )
		{
			longopt = newLongopt;

			return instance;
		}

		/**
		 * The next Option created will require an argument value.
		 *
		 * @return the OptionBuilder instance
		 */

		public OptionBuilder HasArg()
		{
			numberOfArgs = 1;

			return instance;
		}

		/**
		 * The next Option created will require an argument value if
		 * <code>hasArg</code> is true.
		 *
		 * @param hasArg if true then the Option has an argument value
		 * @return the OptionBuilder instance
		 */

		public OptionBuilder HasArg( bool hasArg )
		{
			numberOfArgs = hasArg ? 1 : Option.UNINITIALIZED;

			return instance;
		}

		/**
		 * The next Option created will have the specified argument value 
		 * name.
		 *
		 * @param name the name for the argument value
		 * @return the OptionBuilder instance
		 */

		public OptionBuilder WithArgName( String name )
		{
			argName = name;

			return instance;
		}

		/**
		 * The next Option created will be required.
		 *
		 * @return the OptionBuilder instance
		 */

		public OptionBuilder IsRequired()
		{
			required = true;

			return instance;
		}

		/**
		 * The next Option created uses <code>sep</code> as a means to
		 * separate argument values.
		 *
		 * <b>Example:</b>
		 * <pre>
		 * Option opt = OptionBuilder.withValueSeparator(':')
		 *                           .create('D');
		 *
		 * CommandLine line = parser.parse(args);
		 * String propertyName = opt.getValue(0);
		 * String propertyValue = opt.getValue(1);
		 * </pre>
		 *
		 * @param sep The value separator to be used for the argument values.
		 *
		 * @return the OptionBuilder instance
		 */

		public OptionBuilder WithValueSeparator( char sep )
		{
			valuesep = sep;

			return instance;
		}

		/**
		 * The next Option created uses '<code>=</code>' as a means to
		 * separate argument values.
		 *
		 * <b>Example:</b>
		 * <pre>
		 * Option opt = OptionBuilder.withValueSeparator()
		 *                           .create('D');
		 *
		 * CommandLine line = parser.parse(args);
		 * String propertyName = opt.getValue(0);
		 * String propertyValue = opt.getValue(1);
		 * </pre>
		 *
		 * @return the OptionBuilder instance
		 */

		public OptionBuilder WithValueSeparator()
		{
			valuesep = '=';

			return instance;
		}

		/**
		 * The next Option created will be required if <code>required</code>
		 * is true.
		 *
		 * @param newRequired if true then the Option is required
		 * @return the OptionBuilder instance
		 */

		public OptionBuilder IsRequired( bool newRequired )
		{
			required = newRequired;

			return instance;
		}

		/**
		 * The next Option created can have unlimited argument values.
		 *
		 * @return the OptionBuilder instance
		 */

		public OptionBuilder HasArgs()
		{
			numberOfArgs = Option.UNLIMITED_VALUES;

			return instance;
		}

		/**
		 * The next Option created can have <code>num</code> 
		 * argument values.
		 *
		 * @param num the number of args that the option can have
		 * @return the OptionBuilder instance
		 */

		public OptionBuilder HasArgs( int num )
		{
			numberOfArgs = num;

			return instance;
		}

		/**
		 * The next Option can have an optional argument.
		 *
		 * @return the OptionBuilder instance
		 */

		public OptionBuilder HasOptionalArg()
		{
			numberOfArgs = 1;
			optionalArg = true;

			return instance;
		}

		/**
		 * The next Option can have an unlimited number of
		 * optional arguments.
		 *
		 * @return the OptionBuilder instance
		 */

		public OptionBuilder HasOptionalArgs()
		{
			numberOfArgs = Option.UNLIMITED_VALUES;
			optionalArg = true;

			return instance;
		}

		/**
		 * The next Option can have the specified number of 
		 * optional arguments.
		 *
		 * @param numArgs - the maximum number of optional arguments
		 * the next Option created can have.
		 * @return the OptionBuilder instance
		 */

		public OptionBuilder HasOptionalArgs( int numArgs )
		{
			numberOfArgs = numArgs;
			optionalArg = true;

			return instance;
		}

		/**
		 * The next Option created will have a value that will be an instance 
		 * of <code>type</code>.
		 *
		 * @param newType the type of the Options argument value
		 * @return the OptionBuilder instance
		 */

		public OptionBuilder WithType( Type newType )
		{
			type = newType;

			return instance;
		}

		/**
		 * The next Option created will have the specified description
		 *
		 * @param newDescription a description of the Option's purpose
		 * @return the OptionBuilder instance
		 */

		public OptionBuilder WithDescription( String newDescription )
		{
			description = newDescription;

			return instance;
		}

		/**
		 * Create an Option using the current settings and with 
		 * the specified Option <code>char</code>.
		 *
		 * @param opt the character representation of the Option
		 * @return the Option instance
		 * @throws IllegalArgumentException if <code>opt</code> is not
		 * a valid character.  See Option.
		 */

		public Option Create( char opt )
		{
			return Create( Convert.ToString( opt ) );
		}

		/**
		 * Create an Option using the current settings
		 *
		 * @return the Option instance
		 * @throws IllegalArgumentException if <code>longOpt</code> has
		 * not been set.  
		 */

		public Option Create()
		{
			if ( longopt == null )
			{
				throw new ArgumentException( "must specify longopt" );
			}

			return Create( null );
		}

		/**
		 * Create an Option using the current settings and with 
		 * the specified Option <code>char</code>.
		 *
		 * @param opt the <code>java.lang.String</code> representation 
		 * of the Option
		 * @return the Option instance
		 * @throws IllegalArgumentException if <code>opt</code> is not
		 * a valid character.  See Option.
		 */

		public Option Create( String opt )
		{
			// create the option
			Option option = new Option( opt, description );


			// set the option properties
			option.LongOpt = longopt;
			option.IsRequired = required;
			option.OptionalArg = optionalArg;
			option.NumberOfArgs = numberOfArgs;
			option.Type = type;
			option.ValueSeparator = valuesep;
			option.ArgName = argName;


			// reset the OptionBuilder properties
			Reset();

			// return the Option instance
			return option;
		}
	}
}