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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace net.sf.dotnetcli
{
	/// <summary>
	///		Main entry-point into the library.
	///		
	///		Options represents a collection of Option objects, which
	///		describe the possible options for a command-line.
	///		
	///		It may flexibly parse long and short options, with or without
	///		values.  Additionally, it may parse only a portion of a commandline,
	///		allowing for flexible multi-stage parsing.
	/// </summary>
	public class Options
	{
		/// <summary>
		///		A map of the m_options with the long key
		/// </summary>
		private readonly Dictionary<string, Option> m_long_opts =
			new Dictionary<string, Option>();

		/// <summary>
		///		A map of the option groups
		/// </summary>
		private readonly Dictionary<string, OptionGroup> m_option_groups =
			new Dictionary<string, OptionGroup>();

		/// <summary>
		///		A map of the required Options
		/// </summary>
		private readonly ArrayList m_required_opts = new ArrayList();

		/// <summary>
		///		A map of the m_options with the character key
		/// </summary>
		private readonly Dictionary<string, Option> m_short_opts =
			new Dictionary<string, Option>();

		/// <summary>
		///		Lists the OptionGroups that are members of 
		///		this Options instance.
		/// </summary>
		private List<OptionGroup> OptionGroups
		{
			get { return ( new List<OptionGroup>( m_option_groups.Values ) ); }
		}

		/// <summary>
		///		Retrieves a read-only list of Options in this set.
		/// </summary>
		public ReadOnlyCollection<Option> HelpOptionsReadOnly
		{
			get { return ( HelpOptions.AsReadOnly() ); }
		}

		/// <summary>
		///		Returns the Options for use by the HelpFormatter.
		/// </summary>
		internal List<Option> HelpOptions
		{
			get
			{
				List<Option> opts = new List<Option>( m_short_opts.Values );

				foreach ( Option option in m_long_opts.Values )
				{
					if ( !opts.Contains( option ) )
					{
						opts.Add( option );
					}
				}

				return ( opts );
			}
		}

		/// <summary>
		///		Returns the required options as an ArrayList
		/// </summary>
		public ArrayList RequiredOptions
		{
			get { return m_required_opts; }
		}

		/// <summary>
		///		Add the specified option group.
		/// </summary>
		/// <param name="group">
		///		The OptionGroup that is to be added.
		/// </param>
		/// <returns>
		///		The resulting Options instance
		/// </returns>
		public Options AddOptionGroup( OptionGroup group )
		{
			if ( group.isRequired )
			{
				m_required_opts.Add( group );
			}

			foreach ( Option option in group.Options )
			{
				// an Option cannot be required if it is in an
				// OptionGroup, either the group is required or
				// nothing is required
				option.IsRequired = false;
				AddOption( option );

				m_option_groups.Add( option.Key, group );
			}

			return this;
		}

		/// <summary>
		///		Add an option that only contains a short-name. It may be 
		///		specified as requiring an argument.
		/// </summary>
		/// <param name="opt">
		///		Short single-character name of the option.
		/// </param>
		/// <param name="hasArg">
		///		Flag signaling if an argument is required after this option
		/// </param>
		/// <param name="description">
		///		Self-documenting description
		/// </param>
		/// <returns>
		///		The resulting Options instance
		/// </returns>
		public Options AddOption( String opt, bool hasArg, String description )
		{
			AddOption( opt, null, hasArg, description );

			return this;
		}

		/// <summary>
		///		Add an option that only contains a short-name and a long-name.
		///		It may be specified as requiring an argument.
		/// </summary>
		/// <param name="opt">
		///		Short single-character name of the option.
		/// </param>
		/// <param name="longOpt">
		///		The long representation of this option.
		/// </param>
		/// <param name="hasArg">
		///		Flag signaling if an argument is required after this option
		/// </param>
		/// <param name="description">
		///		Self-documenting description
		/// </param>
		/// <returns>
		///		The resulting Options instance
		/// </returns>
		public Options AddOption( string opt,
		                          string longOpt,
		                          bool hasArg,
		                          string description )
		{
			AddOption( new Option( opt, longOpt, hasArg, description ) );

			return this;
		}

		/// <summary>
		///		Adds an option instance
		/// </summary>
		/// <param name="opt">The option that is to be added.</param>
		/// <returns>The resulting Options instance</returns>
		public Options AddOption( Option opt )
		{
			String key = opt.Key;

			// add it to the long option list
			if ( opt.HasLongOpt )
			{
				m_long_opts.Add( opt.LongOpt, opt );
			}

			// if the option is required add it to the required list
			if ( opt.IsRequired )
			{
				if ( m_required_opts.Contains( key ) )
				{
					m_required_opts.Remove( m_required_opts.IndexOf( key ) );
				}
				m_required_opts.Add( key );
			}

			m_short_opts.Add( key, opt );

			return this;
		}

		/// <summary>
		///		Retrieve the named Option
		/// </summary>
		/// <param name="opt">
		///		Short or long name of the Option
		/// </param>
		/// <returns>
		///		The option represented by opt
		/// </returns>
		[DebuggerHidden]
		public Option GetOption( String opt )
		{
			opt = Util.StripLeadingHyphens( opt );

			if ( m_short_opts.ContainsKey( opt ) )
			{
				return m_short_opts[ opt ];
			}

			if ( m_long_opts.ContainsKey( opt ) )
			{
				return m_long_opts[ opt ];
			}

			return null;
		}

		/// <summary>
		///		Returns whether the named Option is a member of this Options.
		/// </summary>
		/// <param name="opt">
		///		Short or long name of the Option
		/// </param>
		/// <returns>
		///		True if the named Option is a member of this Options
		/// </returns>
		[DebuggerHidden]
		public bool HasOption( String opt )
		{
			opt = Util.StripLeadingHyphens( opt );

			return ( m_short_opts.ContainsKey( opt ) || m_long_opts.ContainsKey( opt ) );
		}

		/// <summary>
		///		Returns the OptionGroup the opt belongs to.
		/// </summary>
		/// <param name="opt">
		///		The option whose OptionGroup is being queried.
		/// </param>
		/// <returns>
		///		The OptionGroup if opt is part of an OptionGroup, otherwise
		///		return null.
		/// </returns>
		public OptionGroup GetOptionGroup( Option opt )
		{
			if ( !m_option_groups.ContainsKey( opt.Key ) )
				return ( null );
			else
				return ( m_option_groups[ opt.Key ] );
		}

		/// <summary>
		///		Dump state, suitable for debugging.
		/// </summary>
		/// <returns>
		/// 
		///		Stringified form of this object.
		///	</returns>
		public override string ToString()
		{
			StringBuilder buf = new StringBuilder();

			buf.Append( "[ Options: [ short " );
			buf.Append( m_short_opts.ToString() );
			buf.Append( " ] [ long " );
			buf.Append( m_long_opts );
			buf.Append( " ]" );

			return buf.ToString();
		}
	}
}