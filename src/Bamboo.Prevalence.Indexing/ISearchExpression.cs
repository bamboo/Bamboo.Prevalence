using System;

namespace Bamboo.Prevalence.Indexing
{
	/// <summary>
	/// Models a search over an index or a set
	/// of indexes.
	/// </summary>
	public interface ISearchExpression
	{
		/// <summary>
		/// Evaluates the expression against
		/// the specified index.<br />
		/// If the expression can not be evaluated
		/// using the specified index
		/// it should raise ArgumentException.
		/// </summary>
		/// <param name="index">the index</param>
		/// <remarks>the expression must never call
		/// <see cref="IIndex.Search"/> unless it is completely sure
		/// the IIndex implementation knows how to handle the expression without
		/// calling this method again (what whould result in a infinite recursion loop)</remarks>
		/// <exception cref="ArgumentException">when the expression can not be evaluated with the specified index</exception>
		SearchResult Evaluate(IIndex index);
	}
}
