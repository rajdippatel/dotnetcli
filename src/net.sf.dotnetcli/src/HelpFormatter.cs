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
using System.IO;
using System.Text;

namespace net.sf.dotnetcli
{
	/// <summary>
	///		A formatter of help messages for the current command line options.
	/// </summary>
	public class HelpFormatter
	{
		/// <summary>
		///		Default name for an argument
		/// </summary>
		public const string DEFAULT_ARG_NAME = "arg";

		/// <summary>
		///		The number of characters of padding to be prefixed to each
		///		description line
		/// </summary>
		public const int DEFAULT_DESC_PAD = 3;

		/// <summary>
		///		Default padding to the left of each line 
		/// </summary>
		public const int DEFAULT_LEFT_PAD = 1;

		/// <summary>
		///		Default prefix for long Option
		/// </summary>
		public const string DEFAULT_LONG_OPT_PREFIX = "--";

		/// <summary>
		///		Default prefix for shortOpts
		/// </summary>
		public const string DEFAULT_OPT_PREFIX = "-";

		/// <summary>
		///		The string to display at the begining of the usage statement
		/// </summary>
		public const string DEFAULT_SYNTAX_PREFIX = "usage: ";

		/// <summary>
		///		Default number of characters per line
		/// </summary>
		public const int DEFAULT_WIDTH = 74;

		/// <summary>
		///		The name of the argument
		/// </summary>
		private string m_default_arg_name = DEFAULT_ARG_NAME;

		/// <summary>
		///		The number of characters of padding to be prefixed
		///		to each description line
		/// </summary>
		private int m_default_desc_pad = DEFAULT_DESC_PAD;

		/// <summary>
		///		Amount of padding to the left of each line
		/// </summary>
		private int m_default_left_pad = DEFAULT_LEFT_PAD;

		/// <summary>
		///		The long Opt prefix
		/// </summary>
		private string m_default_long_opt_prefix = DEFAULT_LONG_OPT_PREFIX;

		/// <summary>
		///		The new line string
		/// </summary>
		private string m_default_new_line = Environment.NewLine;

		/// <summary>
		///		The shortOpt prefix
		/// </summary>
		private string m_default_opt_prefix = DEFAULT_OPT_PREFIX;

		/// <summary>
		///		The string to display at the begining of the usage statement
		/// </summary>
		private string m_default_syntax_prefix = DEFAULT_SYNTAX_PREFIX;

		/// <summary>
		///		Number of characters per line
		/// </summary>
		private int m_default_width = DEFAULT_WIDTH;


		/// <summary>
		///		Gets and sets the 'width'.
		/// </summary>
		public int Width
		{
			get { return m_default_width; }
			set { m_default_width = value; }
		}

		/// <summary>
		///		Gets and sets the 'leftPadding'.
		/// </summary>
		public int LeftPadding
		{
			get { return m_default_left_pad; }
			set { m_default_left_pad = value; }
		}

		/// <summary>
		///		Gets and sets the 'descPadding'.
		/// </summary>
		public int DescPadding
		{
			get { return m_default_desc_pad; }
			set { m_default_desc_pad = value; }
		}

		/// <summary>
		///		Gets and sets the 'syntaxPrefix'.
		/// </summary>
		public string SyntaxPrefix
		{
			get { return m_default_syntax_prefix; }
			set { m_default_syntax_prefix = value; }
		}

		/// <summary>
		///		Gets and sets the 'newLine'.
		/// </summary>
		public string NewLine
		{
			get { return m_default_new_line; }
			set { m_default_new_line = value; }
		}

		/// <summary>
		///		Gets and sets the 'optPrefix'.
		/// </summary>
		public string OptPrefix
		{
			get { return m_default_opt_prefix; }
			set { m_default_opt_prefix = value; }
		}

		/// <summary>
		///		Gets and sets the 'longOptPrefix'.
		/// </summary>
		public string LongOptPrefix
		{
			get { return m_default_long_opt_prefix; }
			set { m_default_long_opt_prefix = value; }
		}

		/// <summary>
		///		Gets and sets the 'argName'.
		/// </summary>
		public string ArgName
		{
			get { return m_default_arg_name; }
			set { m_default_arg_name = value; }
		}

		/// <summary>
		///		<para>Print the help for <code>options</code> with the specified
		///		command line syntax. This method prints help information
		///		to Console.Out.</para>
		/// </summary>
		/// <param name="cmdLineSyntax">
		///		The syntax for this application
		/// </param>
		/// <param name="options">
		///		The Options instance.
		/// </param>
		public void PrintHelp( string cmdLineSyntax, Options options )
		{
			PrintHelp( m_default_width, cmdLineSyntax, null, options, null, false );
		}

		/// <summary>
		///		<para>Print the help for <code>options</code> with the specified
		///		command line syntax. This method prints help information
		///		to Console.Out.</para>
		/// </summary>
		/// <param name="cmdLineSyntax">
		///		The syntax for this application
		/// </param>
		/// <param name="options">
		///		The Options instance.
		/// </param>
		/// <param name="autoUsage">
		///		Whether to print an automatically generated usage statement.
		/// </param>
		public void PrintHelp( string cmdLineSyntax, Options options, bool autoUsage )
		{
			PrintHelp( m_default_width, cmdLineSyntax, null, options, null, autoUsage );
		}

		/// <summary>
		///		<para>Print the help for <code>options</code> with the specified
		///		command line syntax. This method prints help information
		///		to Console.Out.</para>
		/// </summary>
		/// <param name="cmdLineSyntax">
		///		The syntax for this application
		/// </param>
		/// <param name="header">
		///		The banner to display at the beginning of the help
		/// </param>
		/// <param name="options">
		///		The Options instance.
		/// </param>
		/// <param name="footer">
		///		The banner to display at the end of the help
		/// </param>
		public void PrintHelp( string cmdLineSyntax,
		                       string header,
		                       Options options,
		                       string footer )
		{
			PrintHelp( cmdLineSyntax, header, options, footer, false );
		}

		/// <summary>
		///		<para>Print the help for <code>options</code> with the specified
		///		command line syntax. This method prints help information
		///		to Console.Out.</para>
		/// </summary>
		/// <param name="cmdLineSyntax">
		///		The syntax for this application
		/// </param>
		/// <param name="header">
		///		The banner to display at the beginning of the help
		/// </param>
		/// <param name="options">
		///		The Options instance.
		/// </param>
		/// <param name="footer">
		///		The banner to display at the end of the help
		/// </param>
		/// <param name="autoUsage">
		///		Whether to print an automatically generated usage statement.
		/// </param>
		public void PrintHelp( string cmdLineSyntax,
		                       string header,
		                       Options options,
		                       string footer,
		                       bool autoUsage )
		{
			PrintHelp(
				m_default_width, cmdLineSyntax, header, options, footer, autoUsage );
		}

		/// <summary>
		///		<para>Print the help for <code>options</code> with the specified
		///		command line syntax. This method prints help information
		///		to Console.Out.</para>
		/// </summary>
		/// <param name="width">
		///		The number of characters to be displayed on each line
		/// </param>
		/// <param name="cmdLineSyntax">
		///		The syntax for this application
		/// </param>
		/// <param name="header">
		///		The banner to display at the beginning of the help
		/// </param>
		/// <param name="options">
		///		The Options instance.
		/// </param>
		/// <param name="footer">
		///		The banner to display at the end of the help
		/// </param>
		public void PrintHelp( int width,
		                       string cmdLineSyntax,
		                       string header,
		                       Options options,
		                       string footer )
		{
			PrintHelp( width, cmdLineSyntax, header, options, footer, false );
		}

		/// <summary>
		///		<para>Print the help for <code>options</code> with the specified
		///		command line syntax. This method prints help information
		///		to Console.Out.</para>
		/// </summary>
		/// <param name="width">
		///		The number of characters to be displayed on each line
		/// </param>
		/// <param name="cmdLineSyntax">
		///		The syntax for this application
		/// </param>
		/// <param name="header">
		///		The banner to display at the beginning of the help
		/// </param>
		/// <param name="options">
		///		The Options instance.
		/// </param>
		/// <param name="footer">
		///		The banner to display at the end of the help
		/// </param>
		/// <param name="autoUsage">
		///		Whether to print an automatically generated usage statement.
		/// </param>
		public void PrintHelp( int width,
		                       string cmdLineSyntax,
		                       string header,
		                       Options options,
		                       string footer,
		                       bool autoUsage )
		{
			TextWriter pw = Console.Out;

			PrintHelp(
				pw,
				width,
				cmdLineSyntax,
				header,
				options,
				m_default_left_pad,
				m_default_desc_pad,
				footer,
				autoUsage );

			pw.Flush();
		}

		/// <summary>
		///		<para>Print the help for <code>options</code> with the specified
		///		command line syntax. This method prints help information
		///		to Console.Out.</para>
		/// </summary>
		/// <param name="pw">
		///		The writer to which the help will be written
		/// </param>
		/// <param name="width">
		///		The number of characters to be displayed on each line
		/// </param>
		/// <param name="cmdLineSyntax">
		///		The syntax for this application
		/// </param>
		/// <param name="header">
		///		The banner to display at the beginning of the help
		/// </param>
		/// <param name="options">
		///		The Options instance.
		/// </param>
		/// <param name="leftPad">
		///		The number of characters of padding to be prefixed to each line
		/// </param>
		/// <param name="descPad">
		///		The number of characters of padding to be prefixed to each
		///		description line
		/// </param>
		/// <param name="footer">
		///		The banner to display at the end of the help
		/// </param>
		public void PrintHelp( TextWriter pw,
		                       int width,
		                       string cmdLineSyntax,
		                       string header,
		                       Options options,
		                       int leftPad,
		                       int descPad,
		                       string footer )
		{
			PrintHelp(
				pw, width, cmdLineSyntax, header, options, leftPad, descPad, footer, false );
		}

		/// <summary>
		///		<para>Print the help for <code>options</code> with the specified
		///		command line syntax. This method prints help information
		///		to Console.Out.</para>
		/// </summary>
		/// <param name="pw">
		///		The writer to which the help will be written
		/// </param>
		/// <param name="width">
		///		The number of characters to be displayed on each line
		/// </param>
		/// <param name="cmdLineSyntax">
		///		The syntax for this application
		/// </param>
		/// <param name="header">
		///		The banner to display at the beginning of the help
		/// </param>
		/// <param name="options">
		///		The Options instance.
		/// </param>
		/// <param name="leftPad">
		///		The number of characters of padding to be prefixed to each line
		/// </param>
		/// <param name="descPad">
		///		The number of characters of padding to be prefixed to each
		///		description line
		/// </param>
		/// <param name="footer">
		///		The banner to display at the end of the help
		/// </param>
		/// <param name="autoUsage">
		///		Whether to print an automatically generated usage statement.
		/// </param>
		public void PrintHelp( TextWriter pw,
		                       int width,
		                       string cmdLineSyntax,
		                       string header,
		                       Options options,
		                       int leftPad,
		                       int descPad,
		                       string footer,
		                       bool autoUsage )
		{
			if ( ( cmdLineSyntax == null ) || ( cmdLineSyntax.Length == 0 ) )
			{
				throw new ArgumentException( "cmdLineSyntax not provided" );
			}

			if ( autoUsage )
			{
				PrintUsage( pw, width, cmdLineSyntax, options );
			}
			else
			{
				PrintUsage( pw, width, cmdLineSyntax );
			}

			if ( ( header != null ) && ( header.Trim().Length > 0 ) )
			{
				PrintWrapped( pw, width, header );
			}

			PrintOptions( pw, width, options, leftPad, descPad );

			if ( ( footer != null ) && ( footer.Trim().Length > 0 ) )
			{
				PrintWrapped( pw, width, footer );
			}
		}

		/// <summary>
		///		<para>Prints the usage statement for the specified 
		///		application.</para>
		/// </summary>
		/// <param name="pw">
		///		The TextWriter to print the usage statement
		///	</param>
		/// <param name="width">
		///		The number of characters to display per line
		/// </param>
		/// <param name="app">
		///		The application name
		/// </param>
		/// <param name="options">
		///		The command line options
		/// </param>
		public void PrintUsage( TextWriter pw, int width, string app, Options options )
		{
			// initialise the string buffer
			StringBuilder buff =
				new StringBuilder( m_default_syntax_prefix ).Append( app ).Append( " " );

			// create a list for processed option groups
			List<OptionGroup> processedGroups = new List<OptionGroup>();

			List<Option> optList = new List<Option>( options.HelpOptionsReadOnly );
			optList.Sort( new OptionComparator() );

			// for each option in the OptionGroup
			for (
				BidirectionalListEnumerator<Option> i =
					new BidirectionalListEnumerator<Option>( optList );
				i.MoveNext(); )
			{
				Option option = i.Current;

				// check if the option is part of an OptionGroup
				OptionGroup group = options.GetOptionGroup( option );

				// if the option is part of a group 
				if ( group != null )
				{
					// and if the group has not already been processed
					if ( !processedGroups.Contains( group ) )
					{
						// add the group to the processed list
						processedGroups.Add( group );


						// add the usage clause
						AppendOptionGroup( buff, group );
					}

					// otherwise the option was displayed in the group
					// previously so ignore it.
				}

					// if the Option is not part of an OptionGroup
				else
				{
					AppendOption( buff, option, option.IsRequired );
				}

				if ( i.HasNext )
				{
					buff.Append( " " );
				}
			}

			// call PrintWrapped
			PrintWrapped(
				pw, width, buff.ToString().IndexOf( ' ' ) + 1, buff.ToString() );
		}

		/// <summary>
		///		Appends the usage clause for an OptionGroup to a StringBuilder.
		///		The clause is wrapped in square brackets if the group is 
		///		required. The display of the options is handled by AppendOption
		/// </summary>
		/// <param name="buff">
		///		The StringBuilder to Append to
		/// </param>
		/// <param name="group">
		///		The group to Append
		/// </param>
		/// <see cref="AppendOption( StringBuilder, Option, bool )"/>
		private static void AppendOptionGroup( StringBuilder buff, OptionGroup group )
		{
			if ( !group.isRequired )
			{
				buff.Append( "[" );
			}

			List<Option> optList = new List<Option>( group.Options );
			optList.Sort( new OptionComparator() );

			// for each option in the OptionGroup
			for (
				BidirectionalListEnumerator<Option> i =
					new BidirectionalListEnumerator<Option>( optList );
				i.MoveNext(); )
			{
				// whether the option is required or not is handled at group level
				AppendOption( buff, i.Current, true );

				if ( i.HasNext )
				{
					buff.Append( " | " );
				}
			}

			if ( !group.isRequired )
			{
				buff.Append( "]" );
			}
		}

		/// <summary>
		///		Appends the usage clause for an Option to a StringBuilder
		/// </summary>
		/// <param name="buff">
		///		The StringBuilder to Append to
		/// </param>
		/// <param name="option">
		///		The Option to Append
		/// </param>
		/// <param name="required">
		///		Whether the Option is required or not
		/// </param>
		private static void AppendOption( StringBuilder buff,
		                                  Option option,
		                                  bool required )
		{
			if ( !required )
			{
				buff.Append( "[" );
			}

			if ( option.Opt != null )
			{
				buff.Append( "-" ).Append( option.Opt );
			}
			else
			{
				buff.Append( "--" ).Append( option.LongOpt );
			}

			// if the Option has a value
			if ( option.HasArg && ( option.ArgName != null ) )
			{
				buff.Append( " <" ).Append( option.ArgName ).Append( ">" );
			}

			// if the Option is not a required option
			if ( !required )
			{
				buff.Append( "]" );
			}
		}

		/// <summary>
		///		<para>Print the cmdLineSyntax to the specified writer,
		///		user the specified width.</para>
		/// </summary>
		/// <param name="pw">
		///		The TextWriter to write the help to
		/// </param>
		/// <param name="width">
		///		The number of characters per line for the usage statement.
		/// </param>
		/// <param name="cmdLineSyntax">
		///		The usage statement.
		/// </param>
		public void PrintUsage( TextWriter pw, int width, string cmdLineSyntax )
		{
			int argPos = cmdLineSyntax.IndexOf( ' ' ) + 1;

			PrintWrapped(
				pw,
				width,
				m_default_syntax_prefix.Length + argPos,
				m_default_syntax_prefix + cmdLineSyntax );
		}

		/// <summary>
		///		<para>Print the help for the specified Options to the 
		///		specified writer, using the specified width, left padding and
		///		description padding.</para>
		/// </summary>
		/// <param name="pw">
		///		The TextWriter to write the help to
		/// </param>
		/// <param name="width">
		///		The number of characters to display per line
		/// </param>
		/// <param name="options">
		///		The command line Options
		/// </param>
		/// <param name="leftPad">
		///		The number of characters of padding to be prefixed to each line
		/// </param>
		/// <param name="descPad">
		///		The number of characters of padding to be prefixed to each
		///		description line
		/// </param>
		public void PrintOptions( TextWriter pw,
		                          int width,
		                          Options options,
		                          int leftPad,
		                          int descPad )
		{
			StringBuilder sb = new StringBuilder();

			RenderOptions( sb, width, options, leftPad, descPad );
			pw.WriteLine( sb.ToString() );
		}

		/// <summary>
		///		<para>Print the specified text to the specified
		///		TextWriter</para>
		/// </summary>
		/// <param name="pw">
		///		The TextWriter to write the help to
		/// </param>
		/// <param name="width">
		///		The number of characters to display per line
		/// </param>
		/// <param name="text">
		///		The text to be written to the TextWriter
		/// </param>
		public void PrintWrapped( TextWriter pw, int width, string text )
		{
			PrintWrapped( pw, width, 0, text );
		}

		/// <summary>
		///		<para>Print the specified text to the specified
		///		TextWriter</para>
		/// </summary>
		/// <param name="pw">
		///		The TextWriter to write the help to
		/// </param>
		/// <param name="width">
		///		The number of characters to display per line
		/// </param>
		/// <param name="nextLineTabStop">
		///		The position on the next line for the first tab.
		/// </param>
		/// <param name="text">
		///		The text to be written to the TextWriter
		/// </param>
		public void PrintWrapped( TextWriter pw,
		                          int width,
		                          int nextLineTabStop,
		                          string text )
		{
			StringBuilder sb = new StringBuilder( text.Length );

			RenderWrappedText( sb, width, nextLineTabStop, text );
			pw.WriteLine( sb.ToString() );
		}

		/// <summary>
		///		<para>Render the specified Options and return the rendered
		///		Options in a StringBuilder</para>
		/// </summary>
		/// <param name="sb">
		///		The StringBuilder to place the rendered Options into.
		/// </param>
		/// <param name="width">
		///		The number of characters to display per line
		/// </param>
		/// <param name="options">
		///		The command line Options
		/// </param>
		/// <param name="leftPad">
		///		The number of character of padding to be prefixed to each line
		/// </param>
		/// <param name="descPad">
		///		The number of characters of padding to be prefixed to each
		///		description line
		/// </param>
		/// <returns>
		///		The StringBuilder with the rendered Options contents.
		/// </returns>
		public StringBuilder RenderOptions( StringBuilder sb,
		                                    int width,
		                                    Options options,
		                                    int leftPad,
		                                    int descPad )
		{
			string lpad = CreatePadding( leftPad );
			string dpad = CreatePadding( descPad );

			// first create list containing only <lpad>-a,--aaa where 
			// -a is opt and --aaa is long opt; in parallel look for 
			// the longest opt string this list will be then used to 
			// sort options ascending
			int max = 0;
			StringBuilder optBuf;
			List<StringBuilder> prefixList = new List<StringBuilder>();
			List<Option> optList = options.HelpOptions;

			optList.Sort( new OptionComparator() );

			foreach ( Option option in optList )
			{
				optBuf = new StringBuilder( 8 );

				if ( option.Opt == null )
				{
					optBuf.Append( lpad ).Append( "   " + m_default_long_opt_prefix ).Append(
						option.LongOpt );
				}
				else
				{
					optBuf.Append( lpad ).Append( m_default_opt_prefix ).Append( option.Opt );

					if ( option.HasLongOpt )
					{
						optBuf.Append( ',' ).Append( m_default_long_opt_prefix ).Append(
							option.LongOpt );
					}
				}

				if ( option.HasArg )
				{
					if ( option.HasArgName )
					{
						optBuf.Append( " <" ).Append( option.ArgName ).Append( ">" );
					}
					else
					{
						optBuf.Append( ' ' );
					}
				}

				prefixList.Add( optBuf );
				max = ( optBuf.Length > max ) ? optBuf.Length : max;
			}

			int x = 0;

			for (
				BidirectionalListEnumerator<Option> i =
					new BidirectionalListEnumerator<Option>( optList );
				i.MoveNext(); )
			{
				Option option = i.Current;
				optBuf = new StringBuilder( prefixList[ x++ ].ToString() );

				if ( optBuf.Length < max )
				{
					optBuf.Append( CreatePadding( max - optBuf.Length ) );
				}

				optBuf.Append( dpad );

				int nextLineTabStop = max + descPad;

				if ( option.Description != null )
				{
					optBuf.Append( option.Description );
				}

				RenderWrappedText( sb, width, nextLineTabStop, optBuf.ToString() );

				if ( i.HasNext )
				{
					sb.Append( m_default_new_line );
				}
			}

			return sb;
		}

		/// <summary>
		///		<para>Render the specified text and return the rendered
		///		Options in a StringBuilder.</para>
		/// </summary>
		/// <param name="sb">
		///		The StringBuilder to place the rendered text into.
		/// </param>
		/// <param name="width">
		///		The number of characters to display per line
		/// </param>
		/// <param name="nextLineTabStop">
		///		The position on the next line for the first tab.
		/// </param>
		/// <param name="text">
		///		The text to be rendered.
		/// </param>
		/// <returns>
		///		The StringBuilder with the rendered Option contents.
		/// </returns>
		public StringBuilder RenderWrappedText( StringBuilder sb,
		                                        int width,
		                                        int nextLineTabStop,
		                                        string text )
		{
			int pos = FindWrapPos( text, width, 0 );

			if ( pos == -1 )
			{
				sb.Append( RTrim( text ) );

				return sb;
			}
			sb.Append( RTrim( JavaPorts.Substring( text, 0, pos ) ) ).Append(
				m_default_new_line );

			// all following lines must be padded with nextLineTabStop space 
			// characters
			string padding = CreatePadding( nextLineTabStop );

			while ( true )
			{
				text = padding + JavaPorts.Substring( text, pos ).Trim();
				pos = FindWrapPos( text, width, nextLineTabStop );

				if ( pos == -1 )
				{
					sb.Append( text );

					return sb;
				}

				sb.Append( RTrim( JavaPorts.Substring( text, 0, pos ) ) ).Append(
					m_default_new_line );
			}
		}

		/// <summary>
		///		Finds the next text wrap position after <code>startPos</code>
		///		for the text in <code>text</code> with the column width
		///		<code>width</code>. The wrap point is the last position before 
		///		startPos+width having a whitespace character (space, \n, \r).
		/// </summary>
		/// <param name="text">
		///		The text being searched for the wrap position
		/// </param>
		/// <param name="width">
		///		Width of the wrapped text
		/// </param>
		/// <param name="startPos">
		///		Position from which to start the lookup whitepsace character
		/// </param>
		/// <returns>
		///		Position on which the text must be rapped or -1 if the wrap
		///		position is at the end of the text
		/// </returns>
		public int FindWrapPos( string text, int width, int startPos )
		{
			int pos = -1;

			// the line ends before the max wrap pos or a new line char found
			if ( ( ( pos = text.IndexOf( '\n', startPos ) ) != -1 && pos <= width ) ||
			     ( ( pos = text.IndexOf( '\t', startPos ) ) != -1 && pos <= width ) )
			{
				return pos + 1;
			}
			else if ( ( startPos + width ) >= text.Length )
			{
				return -1;
			}


			// look for the last whitespace character before startPos+width
			pos = startPos + width;

			char c;

			while ( ( pos >= startPos ) && ( ( c = text[ pos ] ) != ' ' ) &&
			        ( c != '\n' ) && ( c != '\r' ) )
			{
				--pos;
			}

			// if we found it - just return
			if ( pos > startPos )
			{
				return pos;
			}

			// must look for the first whitespace chearacter after startPos 
			// + width
			pos = startPos + width;

			while ( ( pos <= text.Length ) && ( ( c = text[ pos ] ) != ' ' ) &&
			        ( c != '\n' ) && ( c != '\r' ) )
			{
				++pos;
			}

			return ( pos == text.Length ) ? ( -1 ) : pos;
		}

		/// <summary>
		///		<para>Return a string of padding of length 
		///		<code>len</code></para>
		/// </summary>
		/// <param name="len">
		///		The length of the string of padding to create.
		/// </param>
		/// <returns>
		///		The string of padding
		/// </returns>
		public string CreatePadding( int len )
		{
			StringBuilder sb = new StringBuilder( len );

			for ( int i = 0; i < len; ++i )
			{
				sb.Append( ' ' );
			}

			return sb.ToString();
		}

		/// <summary>
		///		<para>Remove the trailing whitespace from the specified
		///		string.</para>
		/// </summary>
		/// <param name="s">
		///		The string to remove the trailing padding from.
		/// </param>
		/// <returns>
		///		The string of without the trailing padding
		/// </returns>
		protected string RTrim( string s )
		{
			if ( ( s == null ) || ( s.Length == 0 ) )
			{
				return s;
			}

			int pos = s.Length;

			while ( ( pos > 0 ) && char.IsWhiteSpace( s[ pos - 1 ] ) )
			{
				--pos;
			}

			return JavaPorts.Substring( s, 0, pos );
		}

		#region Nested type: OptionComparator

		/// <summary>
		///		<para>The class implements the <code>Comparator</code>
		///		interface for comparing Options</para>
		/// </summary>
		public class OptionComparator : IComparer<Option>
		{
			#region IComparer<Option> Members

			/// <summary>
			///		<para>Compares its two arguments for order. Returns a
			///		negative integer, zero, or a positive integer as the
			///		first argument is less than, equal to, or greater than
			///		the second.</para>
			/// </summary>
			/// <param name="x">
			///		The first Option to be compared.
			/// </param>
			/// <param name="y">
			///		The second Option to be compared.
			/// </param>
			/// <returns>
			///		A negative integer, zero, or a positive integer as the
			///		first rgument is less than, equal to, or greater than the
			///		second.
			/// </returns>
			public int Compare( Option x, Option y )
			{
				return ( string.Compare( x.Key, y.Key, true ) );
			}

			#endregion
		}

		#endregion
	}
}