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

namespace net.sf.dotnetcli
{
	/// <summary>
	///		A class that implements the CommandLineParser interface 
	///		can Parse a String array according to the Options specified
	///		and return a CommandLine.
	/// </summary>
	public interface ICommandLineParser
	{
		/// <summary>
		///		Parse the arugments according to the specified options.
		/// </summary>
		/// <param name="options">
		///		The specified Options
		/// </param>
		/// <param name="arguments">
		///		The command line arguments
		/// </param>
		/// <returns>
		///		The list of atomic option and value tokens
		/// </returns>
		/// <exception cref="ParseException">
		///		If there are any problems encountered while parsing the
		///		command line tokens.
		/// </exception>
		CommandLine Parse( Options options, string[] arguments );

		/// <summary>
		///		Parse the arugments according to the specified options.
		/// </summary>
		/// <param name="options">
		///		The specified Options
		/// </param>
		/// <param name="arguments">
		///		The command line arguments
		/// </param>
		/// <param name="stopAtNonOption">
		///		Specifies whether to contine parsing the arguments if a non
		///		option is encountered.
		/// </param>
		/// <returns>
		///		The list of atomic option and value tokens
		/// </returns>
		/// <exception cref="ParseException">
		///		If there are any problems encountered while parsing the
		///		command line tokens.
		/// </exception>
		CommandLine Parse( Options options, string[] arguments, bool stopAtNonOption );
	}
}