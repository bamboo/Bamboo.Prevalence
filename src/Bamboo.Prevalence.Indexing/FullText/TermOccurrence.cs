#region license
// Bamboo.Prevalence - a .NET object prevalence engine
// Copyright (C) 2004 Rodrigo B. de Oliveira
//
// Based on the original concept and implementation of Prevayler (TM)
// by Klaus Wuestefeld. Visit http://www.prevayler.org for details.
//
// Permission is hereby granted, free of charge, to any person 
// obtaining a copy of this software and associated documentation 
// files (the "Software"), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge, 
// publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included 
// in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY 
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// Contact Information
//
// http://bbooprevalence.sourceforge.net
// mailto:rodrigobamboo@users.sourceforge.net
#endregion

using System;
using System.Text;

namespace Bamboo.Prevalence.Indexing.FullText
{
	/// <summary>
	/// Answers the questions:
	/// <list type="bullet">
	/// <item>in which field does the term occur?</item>
	/// <item>how many times does the term occur in that specific field?</item>
	/// <item>in which positions?</item>
	/// </list>
	/// This information can be used for ranking the search and 
	/// for specific search types such as proximity search, 
	/// searching for terms only in specific fields, etc.
	/// </summary>
	[Serializable]
	public class TermOccurrence
	{
		IndexedField _field;
		
		int[] _positions;
		
		/// <summary>
		/// Creates a new TermOccurrence for the
		/// field and position passed as arguments.
		/// </summary>
		/// <param name="field">the field where the term was found</param>
		/// <param name="position">the position where the term was found</param>
		public TermOccurrence(IndexedField field, int position)
		{
			_field = field;
			_positions = new int[] { position };
		}

		/// <summary>
		/// Field where the term was found
		/// </summary>
		public IndexedField Field
		{
			get
			{
				return _field;
			}
		}
	
		/// <summary>
		/// Positions in the field where
		/// the term was found.
		/// </summary>
		public int[] Positions
		{
			get
			{
				return _positions;
			}
		}

		internal void Add(int position)
		{
			int[] newPositions = new int[_positions.Length + 1];
			Array.Copy(_positions, newPositions, _positions.Length);
			newPositions[_positions.Length] = position;
		}

		/// <summary>
		/// More readable representation of the object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			builder.Append("<");
			builder.Append(_field.ToString());
			builder.Append(" => ");
			builder.Append("[");
			for (int i=0; i<_positions.Length; ++i)
			{
				builder.Append(_positions[i]);
				builder.Append(", ");
			}
			builder.Append("]");
			return builder.ToString();
		}
	}
}
