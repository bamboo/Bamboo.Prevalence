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

namespace Bamboo.Prevalence.Indexing.FullText
{
	/// <summary>
	/// A tokenizer is a source of or a filter for <see cref="Token"/> objects.
	/// </summary>
	public interface ITokenizer
	{
		/// <summary>
		/// For chaining tokenizers together.
		/// </summary>
		ITokenizer Previous
		{
			get;

			set;
		}

		/// <summary>
		/// Clone the tokenizer. If the tokenizer
		/// supports chaining it should also clone
		/// the Previous tokenizer in the chain. If
		/// Previous is null the value of the tail
		/// parameter should be used instead (but without cloning
		/// the tail).
		/// </summary>
		/// <param name="tail">the last tokenizer in the chain</param>
		/// <example>
		/// <code>
		///	public ITokenizer Clone(ITokenizer tail)
		///	{
		///		ITokenizer clone = this.MemberwiseClone() as ITokenizer;
		///		if (null == this.Previous)
		///		{
		///			clone.Previous = tail;
		///		}
		///		else
		///		{
		///			clone.Previous = this.Previous.Clone(tail);
		///		}
		///		return clone;
		///	}
		/// </code>
		/// </example>
		ITokenizer Clone(ITokenizer tail);

		/// <summary>
		/// Retrieves the next token.
		/// </summary>
		/// <returns>
		/// next token or null if no more tokens
		/// are available.
		/// </returns>
		Token NextToken();		
	}
}
