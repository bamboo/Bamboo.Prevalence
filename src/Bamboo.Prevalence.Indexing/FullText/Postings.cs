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
using System.Collections;
using System.Text;

namespace Bamboo.Prevalence.Indexing.FullText
{
	/// <summary>
	/// A collection of Posting objects for a 
	/// specific term.<br />
	/// Posting objects are indexed by record for
	/// fast Add and Remove	operations.
	/// </summary>
	[Serializable]
	public class Postings : System.Collections.IEnumerable
	{
		Hashtable _postings;

		string _term;

		/// <summary>
		/// Creates a new Postings object for 
		/// a term.
		/// </summary>
		/// <param name="term">the term</param>
		public Postings(string term)
		{
			_term = term;
			_postings = new Hashtable();
		}

		/// <summary>
		/// the term
		/// </summary>
		public string Term
		{
			get
			{
				return _term;
			}
		}

		/// <summary>
		/// Returns a snapshot of all the 
		/// records currently indexed by the term
		/// </summary>
		public IRecord[] Records
		{
			get
			{
				IRecord[] records = new IRecord[_postings.Count];
				_postings.Keys.CopyTo(records, 0);
				return records;
			}
		}

		/// <summary>
		/// Adds a new occurrence of the term. The occurrence
		/// information (field and position) will be added
		/// to an existing Posting object whenever possible.
		/// </summary>
		/// <param name="record">the record where the term was found</param>
		/// <param name="field">the field where the term was found</param>
		/// <param name="position">the position in the field where the term was found</param>
		public void Add(IRecord record, IndexedField field, int position)
		{
			Posting posting = _postings[record] as Posting;
			if (null == posting)
			{
				posting = new Posting(record);
				_postings[record] = posting;
			}
			posting.Occurrences.Add(field, position);
		}

		/// <summary>
		/// Removes all information related to a
		/// specific record from this object.
		/// </summary>
		/// <param name="record">the record to be removed</param>
		public void Remove(IRecord record)
		{
			_postings.Remove(record);
		}

		/// <summary>
		/// Enumerates through all the Posting objects.
		/// </summary>
		/// <returns></returns>
		public System.Collections.IEnumerator GetEnumerator()
		{
			return _postings.Values.GetEnumerator();
		}

		/// <summary>
		/// Builds a readable representation of this object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			builder.Append(_term);
			builder.Append(" => [");			
			foreach (Posting posting in _postings.Values)
			{
				builder.Append(posting.ToString());
				builder.Append(", ");
			}
			if (builder.Length > 1)
			{
				builder.Remove(builder.Length-2, 2);
			}
			builder.Append("]");
			return builder.ToString();
		}
	}
}
