using System;
using Bamboo.Prevalence.Indexing.FullText;

namespace Bamboo.Prevalence.Indexing.FullText.Filters
{
	/// <summary>
	/// Base class for filter implementations that 
	/// exclude tokens from the stream based on 
	/// a condition.
	/// </summary>
	[Serializable]
	public abstract class ConditionalFilter : AbstractFilter
	{
		/// <summary>
		/// Creates a standalone filter (with no previous
		/// tokenizer).
		/// </summary>
		protected ConditionalFilter()
		{		
		}

		/// <summary>
		/// Creates a filter in a filter chain.
		/// </summary>
		/// <param name="previous">the previous token in the chain</param>
		protected ConditionalFilter(ITokenizer previous) : base(previous)
		{
		}

		/// <summary>
		/// Gets a token from the previous tokenizer in the chain
		/// and checks the condition implemented by IsValidToken, 
		/// when IsValidToken returns false the token is discarded
		/// and a new one is tried. This process is repeated until
		/// IsValidToken returns true or the previous tokenizer
		/// returns null.
		/// </summary>
		/// <returns>the next token for which IsValidToken returns true or
		/// null when the previous tokenizer runs out of tokens</returns>
		public override Bamboo.Prevalence.Indexing.FullText.Token NextToken()
		{
			Token token = _previous.NextToken();
			while (null != token)
			{
				if (IsValidToken(token))
				{
					break;
				}
				token = _previous.NextToken();
			}
			return token;
		}

		/// <summary>
		/// Test if the token is a valid token and 
		/// as such should be returned to the
		/// caller of <see cref="NextToken" />.
		/// </summary>
		/// <param name="token">token to be tested</param>
		/// <returns>true if the token is valid, false otherwise</returns>
		protected abstract bool IsValidToken(Token token);
	}
}
