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
using System.Globalization;
using System.Text;
using Bamboo.Prevalence.Indexing.FullText;

namespace Bamboo.Prevalence.Indexing.FullText.Filters
{
	/// <summary>
	/// A filter that replaces special characters by
	/// their simpler ASCII counterparts.
	/// </summary>
	[Serializable]
	public class SpecialCharactersFilter : AbstractFilter 
	{
		/// <summary>
		/// Creates a new filter.
		/// </summary>
		public SpecialCharactersFilter()
		{
		}

		/// <summary>
		/// Creates a new filter in a tokenizer chain.
		/// </summary>
		/// <param name="previous">previous tokenizer in the chain</param>
		public SpecialCharactersFilter(ITokenizer previous) : base(previous)
		{
		}

		/// <summary>
		/// Gets the token from the previous tokenizer in the
		/// chain and replaces every "complex" character
		/// in the token by its simpler counterpart.
		/// </summary>
		/// <returns>the new token or null</returns>
		public override Token NextToken()
		{
			Token token = _previous.NextToken();
			if (null != token)
			{
				token.Value = Filter(token.Value);
			}
			return token;
		}

		public static string Filter(string value)
		{			
			char[] mapped = new char[value.Length];
			for (int i=0; i<value.Length; ++i)
			{
				char c = char.ToLower(value[i]);
				switch (c)
				{
					case 'á':
						c = 'a';
						break;

					case 'ã':
						c = 'a';
						break;

					case 'â':
						c = 'a';
						break;

					case 'à':
						c = 'a';
						break;

					case 'é':
						c = 'e';
						break;

					case 'ê':
						c = 'e';
						break;

					case 'í':
						c = 'i';
						break;

					case 'ó':
						c = 'o';
						break;

					case 'õ':
						c = 'o';
						break;							

					case 'ô':
						c = 'o';
						break;					

					case 'ú':
						c = 'u';
						break;

					case 'ü':
						c = 'u';
						break;									

					case 'ç':
						c = 'c';
						break;
				}
				mapped[i] = c;
			}
			return new string(mapped);
		}
	}
}
