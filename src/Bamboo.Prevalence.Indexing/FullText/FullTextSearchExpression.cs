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
