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

namespace net.sf.dotnetcli
{
	/// <summary>
	///		This class is a bidirectional enumerator for the generic class
	///		IList. This enumerator mimics the Java style iterator which can
	///		move forwards and backwards.
	/// </summary>
	/// <typeparam name="T">
	///		The class type of the list being enumerated.
	/// </typeparam>
	public class BidirectionalListEnumerator<T> : IEnumerator<T>
	{
		/// <summary>
		///		The list this enumerator iterates over.
		/// </summary>
		private readonly IList<T> m_list;

		/// <summary>
		///		The list index.
		/// </summary>
		private int m_index = -1;

		/// <summary>
		///		Construct a bidirectional enumerator.
		/// </summary>
		/// <param name="list">
		///		The list being enumerated.
		/// </param>
		public BidirectionalListEnumerator( IList<T> list )
		{
			m_list = list;
		}

		/// <summary>
		///		Returns true if the enumerator is not at the last element
		///		in the list.
		/// </summary>
		public bool HasNext
		{
			get { return ( m_index < m_list.Count - 1 ); }
		}

		#region IEnumerator<T> Members

		/// <summary>
		///		Get the current list element.
		/// </summary>
		public T Current
		{
			get { return m_list[ m_index ]; }
		}

		public void Dispose()
		{
			// Do nothing
		}

		/// <summary>
		///		Move to the next element in the list if one exists. This method
		///		must be invoked a first time to position this enumerator on
		///		the list's first element.
		/// </summary>
		/// <returns>
		///		True if the operation was successful; otherwise false.
		/// </returns>
		public bool MoveNext()
		{
			if ( m_index < m_list.Count - 1 )
			{
				++m_index;
				return ( true );
			}
			else
			{
				return ( false );
			}
		}

		/// <summary>
		///		Resets the iterator to right before the first element in the
		///		list.
		/// </summary>
		public void Reset()
		{
			m_index = -1;
		}

		/// <summary>
		///		Deprecated
		/// </summary>
		object IEnumerator.Current
		{
			get { throw new NotImplementedException(); }
		}

		#endregion

		/// <summary>
		///		Move to the previous element in the list.
		/// </summary>
		/// <returns>
		///		True if the operation was successful; otherwise false.
		/// </returns>
		public bool MovePrevious()
		{
			if ( m_index > 0 && m_list.Count != 0 )
			{
				--m_index;
				return ( true );
			}
			else
			{
				return ( false );
			}
		}
	}
}