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
	///		A group of mutually exclusive options.
	/// </summary>
	public class OptionGroup
	{
		/// <summary>
		///		Hold the options
		/// </summary>
		private readonly Dictionary<string, Option> optionMap =
			new Dictionary<string, Option>();

		/// <summary>
		///		Specified whether this group is required
		/// </summary>
		private bool required;

		/// <summary>
		///		The name of the selected option
		/// </summary>
		private String selected;

		/// <summary>
		///		The names of the options in this group as a generic Dictionary.
		/// </summary>
		public Dictionary<string, Option>.KeyCollection Names
		{
			// the key set is the collection of names
			get { return optionMap.Keys; }
		}

		/**
		 * @return the m_options in this group as a <code>Collection</code>
		 */

		public Dictionary<string, Option>.ValueCollection Options
		{
			// the values are the collection of m_options
			get { return optionMap.Values; }
		}

		/// <summary>
		///		The selected option name.
		/// </summary>
		public String Selected
		{
			get { return selected; }
		}

		/// <summary>
		///		Returns whether this option group is required.
		/// </summary>
		public bool isRequired
		{
			get { return required; }
		}

		/// <summary>
		///		Add opt to this group
		/// </summary>
		/// <param name="opt">
		///		The option to add to this group
		/// </param>
		/// <returns>
		///		This option group with the opt added
		/// </returns>
		public OptionGroup AddOption( Option opt )
		{
			// key   - option name
			// value - the option
			optionMap.Add( opt.Key, opt );
			return this;
		}

		/// <summary>
		///		Set the selected option of this gorup to name.
		/// </summary>
		/// <param name="opt">
		///		The option that is selected.
		/// </param>
		/// <exception cref="AlreadySelectedException">
		///		If an option from this group has already been selected.
		/// </exception>
		public void setSelected( Option opt )
		{
			// if no option has already been selected or the 
			// same option is being reselected then set the
			// selected member variable
			if ( ( selected == null ) || selected.Equals( opt.Opt ) )
			{
				selected = opt.Opt;
			}
			else
			{
				throw new AlreadySelectedException(
					"an option from this group has " + "already been selected: '" + selected +
					"'" );
			}
		}

		/// <summary>
		///		Specifies if this group is required
		/// </summary>
		/// <param name="required">
		///		The value to set set
		/// </param>
		public void setRequired( bool required )
		{
			this.required = required;
		}

		/// <summary>
		///		Returns the stringified version of this OptionGroup.
		/// </summary>
		/// <returns>
		///		The stringified representation of this group.
		/// </returns>
		public override string ToString()
		{
			StringBuilder buff = new StringBuilder();
			buff.Append( "[" );
			foreach ( Option option in optionMap.Values )
			{
				if ( buff.Length > 1 )
				{
					buff.Append( ", " );
				}

				if ( option.Opt != null )
				{
					buff.Append( "-" );
					buff.Append( option.Opt );
				}
				else
				{
					buff.Append( "--" );
					buff.Append( option.LongOpt );
				}

				buff.Append( " " );
				buff.Append( option.Description );
			}

			buff.Append( "]" );

			return buff.ToString();
		}
	}
}