using System;
using System.Collections;
using Bamboo.Prevalence.Indexing;
using Bamboo.Prevalence.Indexing.FullText.Filters;
using Bamboo.Prevalence.Indexing.FullText.Tokenizers;

namespace Bamboo.Prevalence.Indexing.FullText
{
	/// <summary>
	/// An index for full text searches over an record.
	/// </summary>
	[Serializable]
	public class FullTextSearchIndex : IIndex
	{
		public static ITokenFilter DefaultFilter = new SpecialCharactersFilter();

		IndexedFieldCollection _fields;

		Hashtable _postings;

		ITokenFilter _filter = FullTextSearchIndex.DefaultFilter;

		public FullTextSearchIndex()
		{
			_fields = new IndexedFieldCollection();
			_postings = new Hashtable();
		}

		public ITokenFilter Filter
		{
			get
			{
				return _filter;
			}

			set
			{
				if (null == value)
				{
					throw new ArgumentNullException("value");
				}
				_filter = value;
			}
		}

		public Postings[] Postings
		{
			get
			{
				Postings[] postings = new Postings[_postings.Count];
				_postings.Values.CopyTo(postings, 0);
				return postings;
			}
		}

		public IRecord[] Records
		{
			get
			{
				Hashtable records = new Hashtable();
				foreach (Postings postings in _postings.Values)
				{
					foreach (IRecord record in postings.Records)
					{
						records[record] = null;
					}
				}

				IRecord[] returnValue = new IRecord[records.Count];
				records.Keys.CopyTo(returnValue, 0);
				return returnValue;
			}
		}

		/// <summary>
		/// Fields that compose the index.
		/// </summary>
		public IndexedFieldCollection Fields
		{
			get
			{
				return _fields;
			}
		}

		#region Implementation of IIndex
		public void Add(Bamboo.Prevalence.Indexing.IRecord record)
		{					
			foreach (IndexedField field in _fields)
			{
				IndexByField(record, field);
			}
		}

		public void Remove(Bamboo.Prevalence.Indexing.IRecord record)
		{
			foreach (Postings postings in _postings.Values)
			{
				postings.Remove(record);
			}
		}

		public void Update(Bamboo.Prevalence.Indexing.IRecord record)
		{
			Remove(record);
			Add(record);
		}

		public Bamboo.Prevalence.Indexing.SearchResult Search(Bamboo.Prevalence.Indexing.ISearchExpression expression)
		{
			FullTextSearchExpression ftexpression = expression as FullTextSearchExpression;
			if (null != ftexpression)
			{
				return Search(ftexpression);
			}
			return expression.Evaluate(this);
		}
		#endregion

		public Bamboo.Prevalence.Indexing.SearchResult Search(FullTextSearchExpression expression)
		{
			ITokenizer tokenizer = new StringTokenizer(expression.Expression);
			Token token = _filter.Filter(tokenizer);
			if (null == token)
			{
				throw new ArgumentException("Invalid search expression. The expression must contain at least one valid token!", "expression");
			}

			long begin = System.Environment.TickCount;

			SearchResult result = new SearchResult();
			while (null != token)
			{
				Postings postings = _postings[token.Value] as Postings;
				if (null != postings)
				{
					AddToResult(result, postings);
				}

				token = _filter.Filter(tokenizer);
			}

			result.ElapsedTime = System.Environment.TickCount - begin;

			return result;
		}

		void IndexByField(IRecord record, IndexedField field)
		{
			string value = (string)record[field.Name];			
			ITokenizer tokenizer = new StringTokenizer(value);
			Token token = _filter.Filter(tokenizer);
			while (null != token)
			{
				IndexByToken(token, record, field);
				token = _filter.Filter(tokenizer);
			}
		}

		void IndexByToken(Token token, IRecord record, IndexedField field)
		{
			Postings postings = (Postings)_postings[token.Value];
			if (null == postings)
			{
				postings = new Postings(token.Value);
				_postings[token.Value] = postings;
			}
			postings.Add(record, field, token.Position);
		}

		void AddToResult(SearchResult result, Postings found)
		{
			foreach (Posting posting in found)
			{
				result.Add(new SearchHit(posting.Record));
			}
		}
	}
}
