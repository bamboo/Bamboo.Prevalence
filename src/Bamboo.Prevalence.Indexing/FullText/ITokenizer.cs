using System;

namespace Bamboo.Prevalence.Indexing.FullText
{
	/// <summary>
	/// 
	/// </summary>
	public interface ITokenizer
	{
		Token NextToken();
	}
}
