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

		public FullTextSearchExpression(string expression)
		{
			_expression = expression;
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
