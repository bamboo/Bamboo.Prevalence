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
	/// An search expression that searches over
	/// a <see cref="FullTextSearchIndex"/>.
	/// </summary>
	[Serializable]
	public class FullTextSearchExpression : ISearchExpression
	{
		string _expression;

		FullTextSearchMode _mode;

		public FullTextSearchExpression(string expression)
		{
			_expression = expression;
			_mode = FullTextSearchMode.IncludeAny;
		}

		public FullTextSearchExpression(string expression, FullTextSearchMode mode)
		{
			_expression = expression;
			_mode = mode;
		}

		/// <summary>
		/// search expression
		/// </summary>
		public string Expression
		{
			get
			{
				return _expression;
			}
		}

		public FullTextSearchMode SearchMode
		{
			get
			{
				return _mode;
			}
		}

		#region Implementation of ISearchExpression
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
