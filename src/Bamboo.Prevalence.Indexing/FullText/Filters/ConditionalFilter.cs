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
		public ConditionalFilter()
		{		
		}

		public ConditionalFilter(ITokenizer previous) : base(previous)
		{
		}

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
		public abstract bool IsValidToken(Token token);
	}
}
