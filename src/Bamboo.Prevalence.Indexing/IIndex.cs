using System;

namespace Bamboo.Prevalence.Indexing
{
	/// <summary>
	/// A index.
	/// </summary>
	public interface IIndex
	{
		void Add(IRecord record);

		void Remove(IRecord record);

		void Update(IRecord record);

		/// <summary>
		/// Executes the search represented by the
		/// expression passed as argument.<br />
		/// 
		/// If the index does not know how to 
		/// execute the search it should call
		/// <see cref="ISearchExpression.Evaluate"/>. Because
		/// of that, <see cref="ISearchExpression.Evaluate"/>
		/// must never call this method.
		/// </summary>
		/// <param name="expression">search expression</param>
		SearchResult Search(ISearchExpression expression);
	}
}
