using System;
using Bamboo.Prevalence.Indexing.FullText;

namespace Bamboo.Prevalence.Indexing.FullText.Filters
{
	/// <summary>
	/// Filter tokens by length.
	/// </summary>
	[Serializable]
	public class TokenLengthFilter : ConditionalFilter
	{
		int _minTokenLength;

		public TokenLengthFilter(int minTokenLength)
		{
			_minTokenLength = minTokenLength;
		}

		public TokenLengthFilter(ITokenizer previous, int minTokenLength) : base(previous)
		{
			_minTokenLength = minTokenLength;
		}

		public override bool IsValidToken(Token token)
		{
			return (token.Value.Length >= _minTokenLength);
		}
	}
}
