#region License
// Bamboo.Prevalence - a .NET object prevalence engine
// Copyright (C) 2002 Rodrigo B. de Oliveira
//
// Based on the original concept and implementation of Prevayler (TM)
// by Klaus Wuestefeld. Visit http://www.prevayler.org for details.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//
// As a special exception, if you link this library with other files to
// produce an executable, this library does not by itself cause the
// resulting executable to be covered by the GNU General Public License.
// This exception does not however invalidate any other reasons why the
// executable file might be covered by the GNU General Public License.
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
