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

namespace Bamboo.Prevalence.Indexing
{
	public class SearchHit
	{
		IRecord _record;

		public SearchHit(IRecord record)
		{
			_record = record;
		}

		public void Combine(SearchHit other)
		{
		}

		public IRecord Record
		{
			get
			{
				return _record;
			}
		}

		public SearchHit Clone()
		{
			return new SearchHit(_record);
		}
	}

	/// <summary>
	/// Accumulates the results of a search.
	/// </summary>
	public class SearchResult
	{
		#region RecordFieldComparer (used by SortByField)
		/// <summary>
		/// IComparer implementation for IRecord fields.
		/// </summary>
		public class RecordFieldComparer : System.Collections.IComparer
		{
			string _field;

			public RecordFieldComparer(string field)
			{
				_field = field;
			}

			#region Implementation of IComparer
			public int Compare(object x, object y)
			{
				IRecord lhs = ((SearchHit)x).Record;
				IRecord rhs = ((SearchHit)y).Record;
				object lhsField = lhs[_field];
				object rhsField = rhs[_field];
				return ((IComparable)lhsField).CompareTo(rhsField);
			}
			#endregion
		}
		#endregion

		long _elapsedTime;

		ArrayList _hits;
		
		public SearchResult()
		{
			_hits = new ArrayList();
		}

		public int Count
		{
			get
			{
				return _hits.Count;
			}
		}

		public bool Contains(IRecord record)
		{
			return FindSearchHit(record) != null;
		}

		public void Add(SearchHit hit)
		{
			SearchHit existing = FindSearchHit(hit.Record);
			if (null != existing)
			{
				existing.Combine(hit);
			}
			else
			{
				_hits.Add(hit);
			}
		}

		public long ElapsedTime
		{
			get
			{
				return _elapsedTime;
			}

			set
			{
				_elapsedTime = value;
			}
		}

		public SearchHit this[int index]
		{
			get
			{
				return (SearchHit)_hits[index];
			}
		}

		public SearchResult Intersect(SearchResult other)
		{
			SearchResult result = new SearchResult();
			foreach (SearchHit hit in _hits)
			{
				SearchHit otherHit = other.FindSearchHit(hit.Record);
				if (null != otherHit)
				{
					SearchHit resultingHit = hit.Clone();
					resultingHit.Combine(otherHit);
					result.Add(resultingHit);
				}
			}
			return result;
		}

		public void SortByField(string field)
		{
			_hits.Sort(new RecordFieldComparer(field));
		}

		protected SearchHit FindSearchHit(IRecord record)
		{
			foreach (SearchHit hit in _hits)
			{
				if (hit.Record == record)
				{
					return hit;
				}
			}
			return null;
		}
	}
}
