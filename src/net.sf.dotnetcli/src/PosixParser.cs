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

namespace net.sf.dotnetcli
{
	/// <summary>
	///		The class PosixParser provides an implementation of the 
	///		Parser.Flatten method.
	/// </summary>
	public class PosixParser : Parser
	{
		/// <summary>
		///		Holder for flattened tokens
		/// </summary>
		private readonly List<string> m_tokens = new List<string>();

		/// <summary>
		///		Holder for the current option
		/// </summary>
		private Option m_current_option;

		/// <summary>
		///		Specifies if bursting should continue
		/// </summary>
		private bool m_eat_the_rest;

		/// <summary>
		///		The command line Options
		/// </summary>
		private Options m_options;

		/// <summary>
		///		<para>Resets the members to their original state i.e. remove
		///		all of <code>tokens</code> entries, set <code>eatTheRest</code>
		///		to false and set <code>currentOption</code> to null.</para>
		///	</summary>
		private void Init()
		{
			m_eat_the_rest = false;
			m_tokens.Clear();
			m_current_option = null;
		}

		/**
		 * <p>An implementation of {@link Parser}'s abstract
		 * {@link Parser#flatten(Options,String[],bool) flatten} method.</p>
		 *
		 * <p>The following are the rules used by this flatten method.
		 * <ol>
		 *  <li>if <code>stopAtNonOption</code> is <b>true</b> then do not
		 *  burst anymore of <code>arguments</code> entries, just add each
		 *  successive entry without further processing.  Otherwise, ignore
		 *  <code>stopAtNonOption</code>.</li>
		 *  <li>if the current <code>arguments</code> entry is "<b>--</b>"
		 *  just add the entry to the list of processed tokens</li>
		 *  <li>if the current <code>arguments</code> entry is "<b>-</b>"
		 *  just add the entry to the list of processed tokens</li>
		 *  <li>if the current <code>arguments</code> entry is two characters
		 *  in length and the first character is "<b>-</b>" then check if this
		 *  is a valid {@link Option} id.  If it is a valid id, then add the
		 *  entry to the list of processed tokens and set the current {@link Option}
		 *  member.  If it is not a valid id and <code>stopAtNonOption</code>
		 *  is true, then the remaining entries are copied to the list of 
		 *  processed tokens.  Otherwise, the current entry is ignored.</li>
		 *  <li>if the current <code>arguments</code> entry is more than two
		 *  characters in length and the first character is "<b>-</b>" then
		 *  we need to burst the entry to determine its constituents.  For more
		 *  information on the bursting algorithm see 
		 *  {@link PosixParser#burstToken(String, bool) burstToken}.</li>
		 *  <li>if the current <code>arguments</code> entry is not handled 
		 *  by any of the previous rules, then the entry is added to the list
		 *  of processed tokens.</li>
		 * </ol>
		 * </p>
		 *
		 * @param options The command line {@link Options}
		 * @param arguments The command line arguments to be parsed
		 * @param stopAtNonOption Specifies whether to stop flattening
		 * when an non option is found.
		 * @return The flattened <code>arguments</code> String array.
		 */

		protected override String[] Flatten( Options options,
		                                     String[] arguments,
		                                     bool stopAtNonOption )
		{
			Init();
			m_options = options;

			// an iterator for the command line tokens
			BidirectionalListEnumerator<string> iter =
				new BidirectionalListEnumerator<string>( new List<string>( arguments ) );

			// process each command line token
			while ( iter.MoveNext() )
			{
				// get the next command line token
				string token = iter.Current;

				// handle SPECIAL TOKEN
				if ( token.StartsWith( "--" ) )
				{
					if ( token.IndexOf( '=' ) != -1 )
					{
						m_tokens.Add( JavaPorts.Substring( token, 0, token.IndexOf( '=' ) ) );

						m_tokens.Add(
							JavaPorts.Substring( token, token.IndexOf( '=' ) + 1, token.Length ) );
					}
					else
					{
						m_tokens.Add( token );
					}
				}

					// single hyphen
				else if ( "-".Equals( token ) )
				{
					processSingleHyphen( token );
				}
				else if ( token.StartsWith( "-" ) )
				{
					int tokenLength = token.Length;

					if ( tokenLength == 2 )
					{
						processOptionToken( token, stopAtNonOption );
					}
					else if ( options.HasOption( token ) )
					{
						m_tokens.Add( token );
					}
						// requires bursting
					else
					{
						burstToken( token, stopAtNonOption );
					}
				}
				else
				{
					if ( stopAtNonOption )
					{
						process( token );
					}
					else
					{
						m_tokens.Add( token );
					}
				}

				gobble( iter );
			}

			return ( m_tokens.ToArray() );
		}

		/**
		 * <p>Adds the remaining tokens to the processed tokens list.</p>
		 *
		 * @param iter An iterator over the remaining tokens
		 */

		private void gobble( BidirectionalListEnumerator<string> iter )
		{
			if ( m_eat_the_rest )
			{
				while ( iter.MoveNext() )
				{
					m_tokens.Add( iter.Current );
				}
			}
		}

		/**
		 * <p>If there is a current option and it can have an argument
		 * value then add the token to the processed tokens list and 
		 * set the current option to null.</p>
		 * <p>If there is a current option and it can have argument
		 * values then add the token to the processed tokens list.</p>
		 * <p>If there is not a current option add the special token
		 * "<b>--</b>" and the current <code>value</code> to the processed
		 * tokens list.  The add all the remaining <code>argument</code>
		 * values to the processed tokens list.</p>
		 *
		 * @param value The current token
		 */

		private void process( String value )
		{
			if ( ( m_current_option != null ) && m_current_option.HasArg )
			{
				if ( m_current_option.HasArg )
				{
					m_tokens.Add( value );
					m_current_option = null;
				}
				else if ( m_current_option.HasArgs )
				{
					m_tokens.Add( value );
				}
			}
			else
			{
				m_eat_the_rest = true;
				m_tokens.Add( "--" );
				m_tokens.Add( value );
			}
		}

		/**
		 * <p>If it is a hyphen then add the hyphen directly to
		 * the processed tokens list.</p>
		 *
		 * @param hyphen The hyphen token
		 */

		private void processSingleHyphen( String hyphen )
		{
			m_tokens.Add( hyphen );
		}

		/**
		 * <p>If an {@link Option} exists for <code>token</code> then
		 * set the current option and add the token to the processed 
		 * list.</p>
		 * <p>If an {@link Option} does not exist and <code>stopAtNonOption</code>
		 * is set then ignore the current token and add the remaining tokens
		 * to the processed tokens list directly.</p>
		 *
		 * @param token The current option token
		 * @param stopAtNonOption Specifies whether flattening should halt
		 * at the first non option.
		 */

		private void processOptionToken( String token, bool stopAtNonOption )
		{
			if ( m_options.HasOption( token ) )
			{
				m_current_option = m_options.GetOption( token );
				m_tokens.Add( token );
			}
			else if ( stopAtNonOption )
			{
				m_eat_the_rest = true;
			}
		}

		/**
		 * <p>Breaks <code>token</code> into its constituent parts
		 * using the following algorithm.
		 * <ul>
		 *  <li>ignore the first character ("<b>-</b>")</li>
		 *  <li>foreach remaining character check if an {@link Option}
		 *  exists with that id.</li>
		 *  <li>if an {@link Option} does exist then add that character
		 *  prepended with "<b>-</b>" to the list of processed tokens.</li>
		 *  <li>if the {@link Option} can have an argument value and there 
		 *  are remaining characters in the token then add the remaining 
		 *  characters as a token to the list of processed tokens.</li>
		 *  <li>if an {@link Option} does <b>NOT</b> exist <b>AND</b> 
		 *  <code>stopAtNonOption</code> <b>IS</b> set then add the special token
		 *  "<b>--</b>" followed by the remaining characters and also 
		 *  the remaining tokens directly to the processed tokens list.</li>
		 *  <li>if an {@link Option} does <b>NOT</b> exist <b>AND</b>
		 *  <code>stopAtNonOption</code> <b>IS NOT</b> set then add that
		 *  character prepended with "<b>-</b>".</li>
		 * </ul>
		 * </p>
		 *
		 * @param token The current token to be <b>burst</b>
		 * @param stopAtNonOption Specifies whether to stop processing
		 * at the first non-Option encountered.
		 */

		protected void burstToken( String token, bool stopAtNonOption )
		{
			int tokenLength = token.Length;

			for ( int i = 1; i < tokenLength; i++ )
			{
				String ch = Convert.ToString( token[ i ] );
				bool hasOption = m_options.HasOption( ch );

				if ( hasOption )
				{
					m_tokens.Add( "-" + ch );
					m_current_option = m_options.GetOption( ch );

					if ( m_current_option.HasArg && ( token.Length != ( i + 1 ) ) )
					{
						m_tokens.Add( JavaPorts.Substring( token, i + 1 ) );

						break;
					}
				}
				else if ( stopAtNonOption )
				{
					process( JavaPorts.Substring( token, i ) );
				}
				else
				{
					m_tokens.Add( token );
					break;
				}
			}
		}
	}
}