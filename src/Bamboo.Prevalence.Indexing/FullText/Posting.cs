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

namespace Bamboo.Prevalence.Indexing.FullText
{	
	/// <summary>
	/// A Posting object represents the occurrence
	/// of a term in a record and stores all
	/// the information associated to this occurrence
	/// (in which fields does the term occur? how many
	/// times?).<br />
	/// The term itself is not stored in the posting but
	/// should be used to index the posting in a 
	/// dictionary.
	/// </summary>
	[Serializable]
	public class Posting
	{
		IRecord _record;
		
		TermOccurrenceCollection _occurrences;

		/// <summary>
		/// Creates a new posting for a record.
		/// </summary>
		/// <param name="record">the record</param>
		public Posting(IRecord record)
		{				
			_record = record;
			_occurrences = new TermOccurrenceCollection();
		}

		/// <summary>
		/// Occurrences of the term in the record.
		/// </summary>
		internal TermOccurrenceCollection Occurrences
		{
			get
			{
				return _occurrences;
			}
		}
		
		/// <summary>
		/// The record.
		/// </summary>
		public IRecord Record
		{
			get
			{
				return _record;
			}
		}		

		/// <summary>
		/// Builds a more friendly representation of this object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return "<" + _record + " => " + _occurrences + ">";
		}
	}
}
