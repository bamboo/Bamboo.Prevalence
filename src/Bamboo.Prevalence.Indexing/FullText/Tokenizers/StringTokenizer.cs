#region License
// Bamboo.Prevalence - a .NET object prevalence engine
// Copyright (C) 2002 Rodrigo B. de Oliveira
//
// Based on the original concept and implementation of Prevayler (TM)
// by Klaus Wuestefeld. Visit http://www.prevayler.org for details.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//
// As a special exception, if you link this library with other files to
// produce an executable, this library does not by itself cause the
// resulting executable to be covered by the GNU General Public License.
// This exception does not however invalidate any other reasons why the
// executable file might be covered by the GNU General Public License.
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
	/// whitespace and punctuation as separators.
	/// </summary>
	[Serializable]
	public class StringTokenizer : ITokenizer
	{	
		string _text;

		int _current;

		public StringTokenizer(string text)
		{
			_text = text;
			_current = 0;
		}

		protected StringTokenizer(string text, int current)
		{
			_text = text;
			_current = current;
		}

		#region Implementation of ITokenizer
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
