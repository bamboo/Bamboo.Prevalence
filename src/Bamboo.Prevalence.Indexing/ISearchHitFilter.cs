using System;

namespace Bamboo.Prevalence.Indexing
{
	/// <summary>
	/// SearchHit filtering condition.
	/// </summary>
	public interface ISearchHitFilter
	{
		/// <summary>
		/// Test if the hit matches the filtering condition.
		/// </summary>
		/// <param name="hit">the hit</param>
		/// <returns>true if the hit matches the condition and should be included in the
		/// resulting SearchResult</returns>
		bool Test(SearchHit hit);
	}
}
