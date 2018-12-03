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
using System.Text;
using System.Text.RegularExpressions;

namespace net.sf.dotnetcli
{
	/// <summary>
	///		<code>Parser</code> creates <see cref="CommandLine"/>s.
	/// </summary>
	public abstract class Parser : ICommandLineParser
	{
		/// <summary>
		///		commandLine instance
		/// </summary>
		private CommandLine m_cmd_line;

		/// <summary>
		///		Current options
		/// </summary>
		private Options m_options;

		/// <summary>
		///		List of required option strings
		/// </summary>
		private ArrayList m_required_options;

		#region ICommandLineParser Members

		/// <summary>
		///		Parses the specified arguments based on the specified Options.
		/// </summary>
		/// <param name="options">The Options</param>
		/// <param name="arguments">The arguments</param>
		/// <returns>The CommandLine</returns>
		/// <exception cref="ParseException">
		///		If an error occurs when parsing the arguments.
		/// </exception>
		public CommandLine Parse( Options options, string[] arguments )
		{
			return Parse( options, arguments, null, false );
		}

		/// <summary>
		///		Parses the specified arguments based on the specified Options.
		/// </summary>
		/// <param name="options">The Options</param>
		/// <param name="arguments">The arguments</param>
		/// <param name="stopAtNonOption">
		///		Specifies whether to stop interpreting the arguments when a non
		///		option has been encountered and to add them to the CommandLines
		///		args list.
		///	</param>
		/// <returns>The CommandLine</returns>
		/// <exception cref="ParseException">
		///		If an error occurs when parsing the arguments.
		/// </exception>
		public CommandLine Parse( Options options,
		                          string[] arguments,
		                          bool stopAtNonOption )
		{
			return Parse( options, arguments, null, stopAtNonOption );
		}

		#endregion

		/// <summary>
		///		Subclasses must implement this method to reduce the arguments
		///		that have been passed to the Parse method.
		/// </summary>
		/// <param name="opts">
		///		The Options to Parse the arguments by.
		/// </param>
		/// <param name="arguments">
		///		The arguments that have to be flattened.
		/// </param>
		/// <param name="stopAtNonOption">
		///		Specifies whether to stop flattening when a non option 
		///		has been encountered
		/// </param>
		/// <returns>
		///		A string array of the flattened arguments
		/// </returns>
		protected abstract string[] Flatten( Options opts,
		                                     string[] arguments,
		                                     bool stopAtNonOption );

		/// <summary>
		///		Parses the specified arguments based on the specified Options.
		/// </summary>
		/// <param name="options">The Options</param>
		/// <param name="arguments">The arguments</param>
		/// <param name="properties">
		///		Command line options name-value pairs
		///	</param>
		/// <returns>The CommandLine</returns>
		/// <exception cref="ParseException">
		///		If an error occurs when parsing the arguments.
		/// </exception>
		public CommandLine Parse( Options options,
		                          string[] arguments,
		                          Dictionary<string, string> properties )
		{
			return Parse( options, arguments, properties, false );
		}

		/// <summary>
		///		Parses the specified arguments based on the specified Options.
		/// </summary>
		/// <param name="options">The Options</param>
		/// <param name="arguments">The arguments</param>
		/// <param name="properties">
		///		Command line options name-value pairs
		///	</param>
		/// <param name="stopAtNonOption">
		///		Specifies whether to stop interpreting the arguments when a non
		///		option has been encountered and to add them to the CommandLines
		///		args list.
		///	</param>
		/// <returns>The CommandLine</returns>
		/// <exception cref="ParseException">
		///		If an error occurs when parsing the arguments.
		/// </exception>
		public CommandLine Parse( Options options,
		                          string[] arguments,
		                          Dictionary<string, string> properties,
		                          bool stopAtNonOption )
		{
			// initialise members
			m_options = options;

			// clear out the data in m_options in case 
			// it's been used before (CLI-71)
			foreach ( Option option in options.HelpOptions )
			{
				option.ClearValues();
			}

			m_required_options = options.RequiredOptions;
			m_cmd_line = new CommandLine();

			bool eatTheRest = false;

			if ( arguments == null )
			{
				arguments = new string[0];
			}

			List<string> tokenList =
				new List<string>( Flatten( options, arguments, stopAtNonOption ) );

			// Create an enumerator to enumerate the list.
			BidirectionalListEnumerator<string> iter =
				new BidirectionalListEnumerator<string>( tokenList );

			// process each flattened token
			while ( iter.MoveNext() )
			{
				string t = iter.Current;

				// the value is the double-dash
				if ( "--".Equals( t ) )
				{
					eatTheRest = true;
				}

					// the value is a single dash
				else if ( "-".Equals( t ) )
				{
					if ( stopAtNonOption )
					{
						eatTheRest = true;
					}
					else
					{
						m_cmd_line.AddArg( t );
					}
				}

					// the value is an option
				else if ( t.StartsWith( "-" ) )
				{
					if ( stopAtNonOption && !options.HasOption( t ) )
					{
						eatTheRest = true;
						m_cmd_line.AddArg( t );
					}
					else
					{
						ProcessOption( t, iter );
					}
				}

					// the value is an argument
				else
				{
					m_cmd_line.AddArg( t );

					if ( stopAtNonOption )
					{
						eatTheRest = true;
					}
				}

				// eat the remaining tokens
				if ( eatTheRest )
				{
					while ( iter.MoveNext() )
					{
						string str = iter.Current;

						// ensure only one double-dash is added
						if ( !"--".Equals( str ) )
						{
							m_cmd_line.AddArg( str );
						}
					}
				}
			}

			ProcessProperties( properties );
			CheckRequiredOptions();

			return m_cmd_line;
		}

		/// <summary>
		///		Sets the values of Options using the values in properties.
		/// </summary>
		/// <param name="properties">
		///		The value properties to be processed.
		/// </param>
		private void ProcessProperties( Dictionary<string, string> properties )
		{
			if ( properties == null )
			{
				return;
			}

			foreach ( string option in properties.Keys )
			{
				if ( !m_cmd_line.HasOption( option ) )
				{
					Option opt = m_options.GetOption( option );

					// get the value from the properties instance
					string value = properties[ option ];

					if ( opt.HasArg )
					{
						if ( ( opt.Values == null ) || ( opt.Values.Length == 0 ) )
						{
							try
							{
								opt.AddValueForProcessing( value );
							}
							catch ( Exception )
							{
								// if we cannot add the value don't worry about it
							}
						}
					}
					else if ( !Regex.IsMatch( value, "yes|true|1", RegexOptions.IgnoreCase ) )
					{
						// if the value is not yes, true or 1 then don't add the
						// option to the CommandLine
						break;
					}

					m_cmd_line.AddOption( opt );
				}
			}
		}

		/// <summary>
		///		Throws a MissingOptionException if all of the required options
		///		are not present.
		/// </summary>
		/// <exception cref="MissingOptionException">
		///		If any of the required Options are not present.
		/// </exception>
		private void CheckRequiredOptions()
		{
			// if there are required m_options that have not been
			// processsed
			if ( m_required_options.Count > 0 )
			{
				StringBuilder buff = new StringBuilder( "Missing required option" );
				buff.Append( m_required_options.Count == 1 ? "" : "s" );
				buff.Append( ": " );

				foreach ( Object obj in m_required_options )
				{
					buff.Append( obj.ToString() );
				}

				throw new MissingOptionException( buff.ToString() );
			}
		}

		/// <summary>
		///		Process the argument values for the specified Option opt using
		///		the values retrieved from the specified iterator iter.
		/// </summary>
		/// <param name="opt">
		///		The current Option
		/// </param>
		/// <param name="iter">
		///		The iterator over the flattened command line arguments.
		/// </param>
		/// <exception cref="ParseException">
		///		If an argument value is required and it has not been found.
		/// </exception>
		public void ProcessArgs( Option opt, BidirectionalListEnumerator<string> iter )
		{
			// loop until an option is found
			while ( iter.MoveNext() )
			{
				string str = iter.Current;

				// found an Option, not an argument
				if ( m_options.HasOption( str ) && str.StartsWith( "-" ) )
				{
					iter.MovePrevious();
					break;
				}

				// found a value
				try
				{
					opt.AddValueForProcessing( Util.StripLeadingAndTrailingQuotes( str ) );
				}
				catch ( Exception )
				{
					iter.MovePrevious();
					break;
				}
			}

			if ( ( opt.Values == null ) && !opt.HasOptionalArg )
			{
				throw new MissingArgumentException(
					"Missing argument for option:" + opt.Key );
			}
		}

		/// <summary>
		///		Process the Option specified by arg using the values 
		///		retrieved from the specified list and list index.
		/// </summary>
		/// <param name="arg">
		///		The string value representing an Option
		/// </param>
		/// <param name="iter">
		///		The iterator over the flattened command line arguments.
		/// </param>
		/// <exception cref="ParseException">
		///		If arg does not represent an Option
		/// </exception>
		private void ProcessOption( string arg,
		                            BidirectionalListEnumerator<string> iter )
		{
			bool hasOption = m_options.HasOption( arg );

			// if there is no option throw an UnrecognisedOptionException
			if ( !hasOption )
			{
				throw new UnrecognizedOptionException( "Unrecognized option: " + arg );
			}

			// get the option represented by arg
			Option opt = m_options.GetOption( arg );

			// if the option is a required option remove the option from
			// the m_required_options list
			if ( opt.IsRequired )
			{
				m_required_options.Remove( opt.Key );
			}

			// if the option is in an OptionGroup make that option the selected
			// option of the group
			if ( m_options.GetOptionGroup( opt ) != null )
			{
				OptionGroup group = m_options.GetOptionGroup( opt );

				if ( group.isRequired )
				{
					m_required_options.Remove( group );
				}

				group.setSelected( opt );
			}

			// if the option takes an argument value
			if ( opt.HasArg )
			{
				ProcessArgs( opt, iter );
			}

			// set the option on the command line
			m_cmd_line.AddOption( opt );
		}
	}
}