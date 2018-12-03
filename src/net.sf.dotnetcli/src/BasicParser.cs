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
	///		<para>The class BasicParser provides a very simple implementation of
	///		the <see cref="Parser.Flatten( Options, string[], bool )"/> method.
	///		</para>
	/// </summary>
	public class BasicParser : Parser
	{
		/// <summary>
		///		<para>A simple implementation of Parser's abstract 
		///		<see cref="Parser.Flatten( Options, string[], bool )"/> method.
		///		</para>
		///		<para>Note: <code>options</code> and 
		///		<code>stopAtNonOption</code> are not used in this Flatten
		///		method.</para>
		/// </summary>
		/// <param name="options">
		///		The command line <see cref="Options"/>.
		/// </param>
		/// <param name="arguments">
		///		The command line <code>arguments</code> to be parsed.
		/// </param>
		/// <param name="stopAtNonOption">
		///		Specifies whether to stop flattening when a non option is found.
		/// </param>
		/// <returns>
		///		The arguments string array.
		/// </returns>
		protected override String[] Flatten( Options options,
		                                     string[] arguments,
		                                     bool stopAtNonOption )
		{
			// just echo the arguments
			return arguments;
		}
	}
}