using System;
using System.Collections;
using System.Text;

namespace Bamboo.Prevalence.Indexing.FullText
{
	/// <summary>
	/// A collection of Posting objects.
	/// </summary>
	[Serializable]
	public class Postings : System.Collections.IEnumerable
	{
		Hashtable _postings;

		string _term;

		public Postings(string term)
		{
			_term = term;
			_postings = new Hashtable();
		}

		public string Term
		{
			get
			{
				return _term;
			}
		}

		public IRecord[] Records
		{
			get
			{
				IRecord[] records = new IRecord[_postings.Count];
				_postings.Keys.CopyTo(records, 0);
				return records;
			}
		}

		public void Add(IRecord record, IndexedField field, int termIndex)
		{
			Posting posting = _postings[record] as Posting;
			if (null == posting)
			{
				posting = new Posting(record);
				_postings[record] = posting;
			}
			posting.Occurrences.Add(field, termIndex);
		}

		public void Remove(IRecord record)
		{
			_postings.Remove(record);
		}

		public System.Collections.IEnumerator GetEnumerator()
		{
			return _postings.Values.GetEnumerator();
		}

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
