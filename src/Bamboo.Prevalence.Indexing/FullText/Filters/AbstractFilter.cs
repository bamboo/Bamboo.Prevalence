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
	/// Basic implementation for ITokenFilter with
	/// support for tokenizer chaining. 
	/// </summary>
	[Serializable]
	public abstract class AbstractFilter : ITokenFilter
	{
		/// <summary>
		/// the previous tokenizer in the chain
		/// </summary>
		protected ITokenizer _previous;

		/// <summary>
		/// Creates a new filter with no previous
		/// tokenizer.
		/// </summary>
		protected AbstractFilter()
		{
			_previous = null;
		}

		/// <summary>
		/// Creates a new filter with a previous
		/// tokenizer in a tokenizer chain.
		/// </summary>
		/// <param name="previous">the previous tokenizer
		/// in the chain</param>
		protected AbstractFilter(ITokenizer previous)
		{
			_previous = previous;
		}

		/// <summary>
		/// Gets/sets the previous tokenizer
		/// in the chain.
		/// </summary>
		public ITokenizer Previous
		{
			get
			{
				return _previous;
			}

			set
			{
				_previous = value;
			}
		}

		/// <summary>
		/// Returns a MemberwiseClone of this object
		/// with the guarantee that the tail argument
		/// will be the last tokenizer in the new
		/// tokenizer chain.
		/// </summary>
		/// <param name="tail">the last tokenizer for the
		/// new chain</param>
		/// <returns>cloned chain with tail as the
		/// last tokenizer in the chain</returns>
		public ITokenizer Clone(ITokenizer tail)
		{
			AbstractFilter clone = MemberwiseClone() as AbstractFilter;
			if (null == _previous)
			{
				clone._previous = tail;
			}
			else
			{
				clone._previous = _previous.Clone(tail);
			}
			return clone;
		}

		/// <summary>
		/// Must be supplied by derived classes.
		/// </summary>
		/// <returns></returns>
		public abstract Token NextToken();
	}
}
