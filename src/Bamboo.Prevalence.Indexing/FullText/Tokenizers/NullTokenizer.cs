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
	/// Summary description for NullTokenizer.
	/// </summary>
	public class NullTokenizer : ITokenizer
	{
		/// <summary>
		/// The one and only NullTokenizer instance.
		/// </summary>
		public static ITokenizer Instance = new NullTokenizer();

		private NullTokenizer()
		{
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
				throw new NotSupportedException("ITokenizer chaining not supported by NullTokenizer!");
			}
		}

		/// <summary>
		/// See <see cref="Bamboo.Prevalence.Indexing.FullText.ITokenizer.NextToken"/> for
		/// details.
		/// </summary>
		/// <returns></returns>
		public Bamboo.Prevalence.Indexing.FullText.Token NextToken()
		{
			return null;
		}

		/// <summary>
		/// Returns this.
		/// </summary>
		/// <param name="tail">must always be null</param>
		/// <returns>a clone</returns>
		public ITokenizer Clone(ITokenizer tail)
		{
			return this;
		}
		#endregion
	}
}
