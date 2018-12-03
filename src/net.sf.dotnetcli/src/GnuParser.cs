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

using System.Collections.Generic;

namespace net.sf.dotnetcli
{
	/// <summary>
	///		The class GnuParser provides an implementation of the 
	///		<see cref="Parser.Flatten( Options, string[], bool )"/> method.
	/// </summary>
	public class GnuParser : Parser
	{
		/// <summary>
		///		Holder for flattened tokens
		/// </summary>
		private readonly List<string> tokens = new List<string>();

		/// <summary>
		///		Resets the members to their original state i.e. remove all of
		///		<code>tokens</code> entries.
		/// </summary>
		private void Init()
		{
			tokens.Clear();
		}

		/// <summary>
		///		<para>This flatten method does so using the following
		///		rules:</para>
		///		<list type="bullet">
		///			<item>If an <see cref="Option"/> entry AND an
		///			<see cref="Option"/> does not exist for the whole
		///			<code>argument</code> then add the first character as an
		///			option to the processed tokens list e.g. "-D" and add the
		///			rest of the entry to the also.</item>
		///			<item>Otherwise just add the token to the processed
		///			tokens list.</item>
		///		</list><para>
		/// </summary>
		/// <param name="options">
		///		The Options to parse the arguments by.
		/// </param>
		/// <param name="arguments">
		///		The arguments that have to be flattened.
		/// </param>
		/// <param name="stopAtNonOption">
		///		Specifies whether to stop flattening when a non option has been
		///		encountered
		/// </param>
		/// <returns>
		///		A string array of the flattened arguments
		/// </returns>
		protected override string[] Flatten( Options options,
		                                     string[] arguments,
		                                     bool stopAtNonOption )
		{
			Init();

			bool eatTheRest = false;
			Option currentOption = null;

			for ( int i = 0; i < arguments.Length; i++ )
			{
				if ( "--".Equals( arguments[ i ] ) )
				{
					eatTheRest = true;
					tokens.Add( "--" );
				}
				else if ( "-".Equals( arguments[ i ] ) )
				{
					tokens.Add( "-" );
				}
				else if ( arguments[ i ].StartsWith( "-" ) )
				{
					Option option = options.GetOption( arguments[ i ] );

					// this is not an Option
					if ( option == null )
					{
						// handle special properties Option
						Option specialOption =
							options.GetOption( JavaPorts.Substring( arguments[ i ], 0, 2 ) );

						if ( specialOption != null )
						{
							tokens.Add( JavaPorts.Substring( arguments[ i ], 0, 2 ) );
							tokens.Add( JavaPorts.Substring( arguments[ i ], 2 ) );
						}
						else if ( stopAtNonOption )
						{
							eatTheRest = true;
							tokens.Add( arguments[ i ] );
						}
						else
						{
							tokens.Add( arguments[ i ] );
						}
					}
					else
					{
						// WARNING: Findbugs reports major problems with the 
						//			following code. 
						//
						//			As option cannot be null, currentOption 
						//			cannot and much of the code below is never 
						//			going to be run.

						currentOption = option;

						// special option
						Option specialOption =
							options.GetOption( JavaPorts.Substring( arguments[ i ], 0, 2 ) );

						if ( ( specialOption != null ) && ( option == null ) )
						{
							tokens.Add( JavaPorts.Substring( arguments[ i ], 0, 2 ) );
							tokens.Add( JavaPorts.Substring( arguments[ i ], 2 ) );
						}
						else if ( ( currentOption != null ) && currentOption.HasArg )
						{
							if ( currentOption.HasArg )
							{
								tokens.Add( arguments[ i ] );
								currentOption = null;
							}
							else if ( currentOption.HasArgs )
							{
								tokens.Add( arguments[ i ] );
							}
							else if ( stopAtNonOption )
							{
								eatTheRest = true;
								tokens.Add( "--" );
								tokens.Add( arguments[ i ] );
							}
							else
							{
								tokens.Add( arguments[ i ] );
							}
						}
						else if ( currentOption != null )
						{
							tokens.Add( arguments[ i ] );
						}
						else if ( stopAtNonOption )
						{
							eatTheRest = true;
							tokens.Add( "--" );
							tokens.Add( arguments[ i ] );
						}
						else
						{
							tokens.Add( arguments[ i ] );
						}
					}
				}
				else
				{
					tokens.Add( arguments[ i ] );
				}

				if ( eatTheRest )
				{
					for ( i++; i < arguments.Length; i++ )
					{
						tokens.Add( arguments[ i ] );
					}
				}
			}

			return ( tokens.ToArray() );
		}
	}
}