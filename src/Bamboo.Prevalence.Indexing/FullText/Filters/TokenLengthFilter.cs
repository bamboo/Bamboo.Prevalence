using System;
using Bamboo.Prevalence.Indexing.FullText;

namespace Bamboo.Prevalence.Indexing.FullText.Filters
{
	/// <summary>
	/// Filters off tokens by length.
	/// </summary>
	[Serializable]
	public class TokenLengthFilter : ConditionalFilter
	{
		int _minTokenLength;

		/// <summary>
		/// Creates a new filter that will only allow
		/// tokens with at least minTokenLength 
		/// characters to pass.
		/// </summary>
		/// <param name="minTokenLength">minimum token length</param>
		public TokenLengthFilter(int minTokenLength)
		{
			_minTokenLength = minTokenLength;
		}

		/// <summary>
		/// See <see cref="TokenLengthFilter(int)"/>.
		/// </summary>
		/// <param name="previous">previous tokenizer in the chain</param>
		/// <param name="minTokenLength">minimum token length</param>
		public TokenLengthFilter(ITokenizer previous, int minTokenLength) : base(previous)
		{
			_minTokenLength = minTokenLength;
		}

		/// <summary>
		/// See <see cref="ConditionalFilter.IsValidToken"/> for details.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		protected override bool IsValidToken(Token token)
		{
			return (token.Value.Length >= _minTokenLength);
		}
	}
}
