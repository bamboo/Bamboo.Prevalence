using System;

namespace Bamboo.Prevalence.Indexing.FullText
{
	/// <summary>
	/// 
	/// </summary>
	public interface ITokenFilter
	{
		/// <summary>
		/// Get the next token and apply any filtering
		/// rules.
		/// </summary>
		Token Filter(ITokenizer tokenizer);
	}
}
