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
using System.Collections;
using Bamboo.Prevalence.Indexing.FullText;

namespace Bamboo.Prevalence.Indexing.FullText.Filters
{
	/// <summary>
	/// Filters off tokens by word.
	/// </summary>
	[Serializable]
	public class WordFilter : ConditionalFilter
	{
		Hashtable _words;

		/// <summary>
		/// See <see cref="WordFilter(string[])"/>.
		/// </summary>
		/// <param name="previous">the previous tokenizer in the chain</param>
		/// <param name="words">list of words that should be filtered
		/// off the chain</param>
		public WordFilter(ITokenizer previous, params string[] words) : base(previous)
		{
			if (null == words)
			{
				throw new ArgumentNullException("words");
			}

			_words = new Hashtable(words.Length);
			foreach (string word in words)
			{
				_words[word] = null;
			}
		}

		/// <summary>
		/// Creates a new filter that will not allow
		/// any words in the list represented by the words
		/// argument to pass through the chain.
		/// </summary>
		/// <param name="words">list of words that should be filtered
		/// off the chain</param>
		public WordFilter(params string[] words) : this(null, words)
		{
		}
		
		/// <summary>
		/// See <see cref="ConditionalFilter.IsValidToken"/> for details.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		protected override bool IsValidToken(Token token)
		{
			return !_words.ContainsKey(token.Value);
		}
	}
}
