#region license
// Bamboo.Prevalence - a .NET object prevalence engine
// Copyright (C) 2004 Rodrigo B. de Oliveira
//
// Based on the original concept and implementation of Prevayler (TM)
// by Klaus Wuestefeld. Visit http://www.prevayler.org for details.
//
// Permission is hereby granted, free of charge, to any person 
// obtaining a copy of this software and associated documentation 
// files (the "Software"), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge, 
// publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included 
// in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY 
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// Contact Information
//
// http://bbooprevalence.sourceforge.net
// mailto:rodrigobamboo@users.sourceforge.net
#endregion

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
