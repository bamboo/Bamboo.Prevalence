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

namespace Bamboo.Prevalence.Indexing
{
	#region SearchHit class
	/// <summary>
	/// A single item returned from a search.
	/// </summary>
	public class SearchHit
	{
		IRecord _record;

		/// <summary>
		/// creates a new object for the
		/// record passed as argument.
		/// </summary>
		/// <param name="record">a record</param>
		public SearchHit(IRecord record)
		{
			_record = record;
		}

		/// <summary>
		/// combines two search hits that refer
		/// to the same record. all the extended
		/// properties such as ranking and index specific
		/// information should be combined.
		/// </summary>
		/// <param name="other">the SearchHit that 
		/// should be combined to this one</param>
		public void Combine(SearchHit other)
		{
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
		/// Creates a clone from this object.
		/// </summary>
		/// <returns>a clone</returns>
		public SearchHit Clone()
		{
			return new SearchHit(_record);
		}
	}
	#endregion

	#region SearchResult class
	/// <summary>
	/// Accumulates the results of a search.
	/// </summary>
	public class SearchResult : IEnumerable
	{
		#region RecordFieldComparer (used by SortByField)
		/// <summary>
		/// IComparer implementation for IRecord fields.
		/// </summary>
		public class RecordFieldComparer : System.Collections.IComparer
		{
			string _field;

			/// <summary>
			/// Creates a new RecordFieldComparer for
			/// a specific field.
			/// </summary>
			/// <param name="field">field that should be used
			/// in comparisons</param>
			public RecordFieldComparer(string field)
			{
				_field = field;
			}

			#region Implementation of IComparer
			/// <summary>
			/// See <see cref="IComparer.Compare"/> for details.
			/// </summary>
			/// <param name="x"></param>
			/// <param name="y"></param>
			/// <returns></returns>
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

		#region RecordComparer
		internal class RecordComparer : System.Collections.IComparer
		{
			System.Collections.IComparer _comparer;

			public RecordComparer(System.Collections.IComparer comparer)
			{
				_comparer = comparer;
			}
			#region IComparer Members

			public int Compare(object x, object y)
			{
				return _comparer.Compare(((SearchHit)x).Record, ((SearchHit)y).Record);
			}

			#endregion

		}

		#endregion

		#region SearchHitRecordEnumerator (used by GetRecordEnumerator())
		/// <summary>
		/// Enumerates the records in a SearchResult.
		/// </summary>
		public class SearchHitRecordEnumerator : IEnumerator, IEnumerable
		{
			IEnumerator _hits;

			internal SearchHitRecordEnumerator(IEnumerator hits)
			{
				_hits = hits;
			}

			#region Implementation of IEnumerator

			/// <summary>
			/// See <see cref="IEnumerator.Reset"/> for details.
			/// </summary>
			public void Reset()
			{
				_hits.Reset();
			}

			/// <summary>
			/// See <see cref="IEnumerator.MoveNext"/> for details.
			/// </summary>
			/// <returns></returns>
			public bool MoveNext()
			{
				return _hits.MoveNext();
			}

			/// <summary>
			/// The current record.
			/// </summary>
			public IRecord Current
			{
				get
				{
					return ((SearchHit)_hits.Current).Record;
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return ((SearchHit)_hits.Current).Record;
				}
			}
			#endregion

			#region Implementation of IEnumerable
			System.Collections.IEnumerator IEnumerable.GetEnumerator()
			{
				return this;
			}
			#endregion
		}
		#endregion

		long _elapsedTime;

		ArrayList _hits;
		
		/// <summary>
		/// Creates an empty SearchResult object.
		/// </summary>
		public SearchResult()
		{
			_hits = new ArrayList();
		}

		
		/// <summary>
		/// Number of items returned by the search.
		/// </summary>
		public int Count
		{
			get
			{
				return _hits.Count;
			}
		}


		/// <summary>
		/// Checks if the specified record was returned
		/// by the search.
		/// </summary>
		/// <param name="record">record to be checked</param>
		/// <returns>true if the record was indeed returned
		/// by the search</returns>
		/// <remarks>reference comparison is always used</remarks>
		public bool Contains(IRecord record)
		{
			return FindSearchHit(record) != null;
		}


		/// <summary>
		/// Adds a new item to the collection of items
		/// returned by the search. If the hit
		/// represents an existing record it
		/// will be combined to the existing hit instead.
		/// </summary>
		/// <param name="hit">the hit to be added or
		/// combined to a existing hit</param>
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


		/// <summary>
		/// How long the search took
		/// </summary>
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


		/// <summary>
		/// Returns an item by its position
		/// </summary>
		public SearchHit this[int index]
		{
			get
			{
				return (SearchHit)_hits[index];
			}
		}


		/// <summary>
		/// Set intersection operation. Creates
		/// a new SearchResult with all the records
		/// that exist in both SearchResult objects.
		/// </summary>
		/// <param name="other"></param>
		/// <returns>a SearchResult representing the
		/// intersection between the this and other objects
		/// </returns>
		/// <remarks>all the SearchHit objects in
		/// the resulting SearchResult are clones from
		/// the original ones combined to the ones in
		/// other</remarks>
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

		/// <summary>
		/// Build a new SearchResult object including
		/// only those elements for which the 
		/// filter returns true.
		/// </summary>
		/// <param name="filter">filter</param>
		/// <returns>a new SearchResult containing all the elements for which 
		/// <see cref="ISearchHitFilter.Test"/> returned true</returns>
		public SearchResult Filter(ISearchHitFilter filter)
		{
			SearchResult result = new SearchResult();
			foreach (SearchHit hit in _hits)
			{
				if (filter.Test(hit))
				{
					result.Add(hit);
				}
			}			
			return result;
		}


		/// <summary>
		/// Sorts the result by a specific record field.
		/// </summary>
		/// <param name="field">the field to be used in the sort</param>
		public void SortByField(string field)
		{
			if (null == field)
			{
				throw new ArgumentNullException("field");
			}
			_hits.Sort(new RecordFieldComparer(field));
		}

		/// <summary>
		/// Sorts the result using the specified comparer.
		/// </summary>
		/// <param name="comparer"></param>
		public void Sort(IComparer comparer)
		{
			if (null == comparer)
			{
				throw new ArgumentNullException("comparer");
			}
			_hits.Sort(new RecordComparer(comparer));
		}

		/// <summary>
		/// Copies all the records to an array. The
		/// order is mantained so that
		/// this[N].Record == resultingArray[N] is
		/// valid for every 0 &lt;= N &lt; this.Count.
		/// </summary>
		/// <param name="recordType">array element type</param>
		/// <returns>the resulting array.</returns>
		public Array ToRecordArray(Type recordType)
		{			
			object[] records = (object[])Array.CreateInstance(recordType, _hits.Count);
			for (int i=0; i<records.Length; ++i)
			{				
				records[i] = ((SearchHit)_hits[i]).Record;
			}
			return records;
		}

		public Array ToRecordArray()
		{
			return ToRecordArray(typeof(object));
		}


		/// <summary>
		/// Returns an enumerator for all the records
		/// in this object.
		/// </summary>
		/// <returns></returns>
		public SearchHitRecordEnumerator GetRecordEnumerator()
		{
			return new SearchHitRecordEnumerator(_hits.GetEnumerator());
		}

		/// <summary>
		/// Finds a SearchHit that represents a specific
		/// record.
		/// </summary>
		/// <param name="record">the record to search for</param>
		/// <returns>the found SearchHit or null</returns>
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

		
		#region Implementation of IEnumerable
		/// <summary>
		/// See <see cref="IEnumerable.GetEnumerator"/> for details
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return _hits.GetEnumerator();
		}
		#endregion
	}
	#endregion
}
