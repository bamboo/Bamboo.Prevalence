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
		/// <summary>
		/// A filter that considers only tokens with more than 2 characters
		/// and replaces special characters (like 'ç', 'à') by their
		/// simpler counterparts ('c', 'a').
		/// </summary>
		public static ITokenFilter DefaultFilter = new SpecialCharactersFilter(new TokenLengthFilter(3));

		IndexedFieldCollection _fields;

		Hashtable _postings;

		ITokenFilter _filter = FullTextSearchIndex.DefaultFilter;

		public FullTextSearchIndex()
		{
			_fields = new IndexedFieldCollection();
			_postings = new Hashtable();
		}

		public FullTextSearchIndex(ITokenFilter filter) : this()
		{
			Filter = filter;
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
					throw new ArgumentNullException("value", "Filter cannot be null!");
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
			ITokenizer tokenizer = CreateTokenizer(expression.Expression);
			Token token = tokenizer.NextToken();
			if (null == token)
			{
				throw new ArgumentException("Invalid search expression. The expression must contain at least one valid token!", "expression");
			}

			long begin = System.Environment.TickCount;

			SearchResult result = null;
			if (expression.SearchMode == FullTextSearchMode.IncludeAny)
			{
				result = IncludeAny(tokenizer, token);
			}
			else
			{
				result = IncludeAll(tokenizer, token);
			}
			
			result.ElapsedTime = System.Environment.TickCount - begin;

			return result;
		}

		SearchResult IncludeAny(ITokenizer tokenizer, Token token)
		{
			SearchResult result = new SearchResult();
			while (null != token)
			{
				SearchToken(result, token);
				token = tokenizer.NextToken();
			}
			return result;
		}

		SearchResult IncludeAll(ITokenizer tokenizer, Token token)
		{
			ArrayList results = new ArrayList();
			while (null != token)
			{
				SearchResult tokenResult = new SearchResult();
				SearchToken(tokenResult, token);
				results.Add(tokenResult);

				token = tokenizer.NextToken();
			}

			SearchResult result = (SearchResult)results[0];
			for (int i=1; i<results.Count && result.Count > 0; ++i)
			{
				result = result.Intersect((SearchResult)results[i]);
			}
			return result;
		}

		void IndexByField(IRecord record, IndexedField field)
		{
			string value = (string)record[field.Name];
			ITokenizer tokenizer = CreateTokenizer(value);
			Token token = tokenizer.NextToken();
			while (null != token)
			{
				IndexByToken(token, record, field);
				token = tokenizer.NextToken();
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

		void SearchToken(SearchResult result, Token token)
		{
			Postings postings = (Postings)_postings[token.Value];
			if (null != postings)
			{
				AddToResult(result, postings);
			}

		}

		void AddToResult(SearchResult result, Postings found)
		{
			foreach (Posting posting in found)
			{
				result.Add(new SearchHit(posting.Record));
			}
		}		

		ITokenizer CreateTokenizer(string value)
		{
			return _filter.Clone(new StringTokenizer(value));
		}
	}
}
