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
	}

	/// <summary>
	/// Accumulates the results of a search.
	/// </summary>
	public class SearchResult
	{
		long _elapsedTime;

		Hashtable _hits;
		
		public SearchResult()
		{
			_hits = new Hashtable();
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
			return _hits.ContainsKey(record);
		}

		public void Add(SearchHit hit)
		{
			SearchHit existing = _hits[hit.Record] as SearchHit;
			if (null != existing)
			{
				existing.Combine(hit);
			}
			else
			{
				_hits[hit.Record] = hit;
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
	}
}
