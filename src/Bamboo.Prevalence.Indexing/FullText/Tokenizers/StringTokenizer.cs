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

namespace Bamboo.Prevalence.Indexing.FullText.Tokenizers
{
	/// <summary>
	/// Splits a string into tokens by considering any 
	/// whitespace and punctuation characters as separators.
	/// </summary>
	/// <remarks>
	/// This tokenizer must always be the last in a 
	/// tokenizer chain.
	/// </remarks>
	[Serializable]
	public class StringTokenizer : ITokenizer
	{	
		string _text;

		int _current;

		/// <summary>
		/// Creates a new tokenizer for the string
		/// in the text argument.
		/// </summary>
		/// <param name="text">token source</param>
		public StringTokenizer(string text) : this(text, 0)
		{
		}

		/// <summary>
		/// Creates a new tokenizer for the string
		/// in the text argument starting from
		/// the position indicated by the current
		/// argument.
		/// </summary>
		/// <param name="text">token source</param>
		/// <param name="current">starting position</param>
		protected StringTokenizer(string text, int current)
		{
			if (null == text)
			{
				throw new ArgumentNullException("text", "text can't be null");
			}

			_text = text;
			_current = current;
		}

		#region Implementation of ITokenizer
		/// <summary>
		/// Always returns null since this tokenizer
		/// must be the last in the chain.
		/// </summary>
		public ITokenizer Previous
		{
			get
			{
				return null;
			}

			set
			{
				throw new NotSupportedException("ITokenizer chaining not supported by StringTokenizer!");
			}
		}

		/// <summary>
		/// See <see cref="Bamboo.Prevalence.Indexing.FullText.ITokenizer.NextToken"/> for
		/// details.
		/// </summary>
		/// <returns></returns>
		public Bamboo.Prevalence.Indexing.FullText.Token NextToken()
		{			
			SkipSeparators();
			int begin = _current;
			for (; _current<_text.Length; ++_current)
			{
				if (IsSeparator(_text, _current))
				{
					break;
				}
			}
			if (_current > begin)
			{
				return new Token(_text.Substring(begin, _current-begin), begin);
			}
			return null;
		}

		/// <summary>
		/// Returns a clone.
		/// </summary>
		/// <param name="tail">must always be null</param>
		/// <returns>a clone</returns>
		public ITokenizer Clone(ITokenizer tail)
		{
			if (null != tail)
			{
				throw new NotSupportedException("ITokenizer chaining not supported by StringTokenizer!");
			}
			return new StringTokenizer(_text, _current);
		}
		#endregion		

		void SkipSeparators()
		{
			for (; _current<_text.Length; ++_current)
			{
				if (!IsSeparator(_text, _current))
				{
					break;
				}
			}			
		}

		bool IsSeparator(string text, int index)
		{
			char current = text[index];
			return Char.IsWhiteSpace(current) || Char.IsPunctuation(current);
		}
	}
}
