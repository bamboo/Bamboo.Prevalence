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
using System.Globalization;
using System.Text;
using Bamboo.Prevalence.Indexing.FullText;

namespace Bamboo.Prevalence.Indexing.FullText.Filters
{
	/// <summary>
	/// A filter that replaces special characters by
	/// their ASCII counterparts.
	/// </summary>
	[Serializable]
	public class SpecialCharactersFilter : AbstractFilter 
	{
		public SpecialCharactersFilter()
		{
		}

		public SpecialCharactersFilter(ITokenizer previous) : base(previous)
		{
		}

		public override Token NextToken()
		{
			Token token = _previous.NextToken();
			if (null != token)
			{
				token.Value = Filter(token.Value);
			}
			return token;
		}

		string Filter(string value)
		{			
			char[] mapped = new char[value.Length];
			for (int i=0; i<value.Length; ++i)
			{
				char c = value[i];

				if (Char.IsUpper(c))
				{
					c = Char.ToLower(c);
				}

				switch (c)
				{
					case 'á':
						c = 'a';
						break;

					case 'é':
						c = 'e';
						break;

					case 'í':
						c = 'i';
						break;

					case 'ó':
						c = 'o';
						break;

					case 'ú':
						c = 'u';
						break;

					case 'ã':
						c = 'a';
						break;

					case 'õ':
						c = 'o';
						break;

					case 'â':
						c = 'a';
						break;

					case 'ê':
						c = 'e';
						break;

					case 'ô':
						c = 'o';
						break;

					case 'à':
						c = 'a';
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
