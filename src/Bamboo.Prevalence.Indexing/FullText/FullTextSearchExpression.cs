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
using Bamboo.Prevalence.Indexing;

namespace Bamboo.Prevalence.Indexing.FullText
{
	/// <summary>
	/// Search expression class for searches over
	/// a <see cref="FullTextSearchIndex"/>.
	/// </summary>
	[Serializable]
	public class FullTextSearchExpression : ISearchExpression
	{
		string _expression;

		FullTextSearchMode _mode;

		/// <summary>
		/// Creates a new search expression that will
		/// return all the records that include any of
		/// the words (<see cref="FullTextSearchMode.IncludeAny"/>)
		/// in the expression passed as argument.
		/// </summary>
		/// <param name="expression">words to search for</param>
		public FullTextSearchExpression(string expression)
		{
			_expression = expression;
			_mode = FullTextSearchMode.IncludeAny;
		}

		/// <summary>
		/// Creates a new search expression for
		/// the words in the expression argument
		/// with the specific behavior indicated by the mode
		/// argument.
		/// </summary>
		/// <param name="expression">words to search for</param>
		/// <param name="mode">search mode</param>
		public FullTextSearchExpression(string expression, FullTextSearchMode mode)
		{
			_expression = expression;
			_mode = mode;
		}

		/// <summary>
		/// Search expression
		/// </summary>
		public string Expression
		{
			get
			{
				return _expression;
			}
		}

		/// <summary>
		/// Search mode
		/// </summary>
		public FullTextSearchMode SearchMode
		{
			get
			{
				return _mode;
			}
		}

		#region Implementation of ISearchExpression
		/// <summary>
		/// Delegates to <see cref="Bamboo.Prevalence.Indexing.FullText.FullTextSearchIndex.Search"/>.
		/// </summary>
		/// <param name="index">index</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">if the
		/// index argument is not of the correct type</exception>
		public Bamboo.Prevalence.Indexing.SearchResult Evaluate(Bamboo.Prevalence.Indexing.IIndex index)
		{
			FullTextSearchIndex ftindex = index as FullTextSearchIndex;
			if (null == ftindex)
			{
				throw new ArgumentException("FullTextSearchExpression objects can evaluated against FullTextSearchIndex objects only!");
			}
			return ftindex.Search(this);
		}
		#endregion
	}
}
