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
using System.Text.RegularExpressions;
using Bamboo.Prevalence.Indexing.FullText;

namespace Bamboo.Prevalence.Indexing.FullText.Filters
{
	/// <summary>
	/// Filters off tokens based on a regular expression.
	/// </summary>
	/// <remarks>This filter will filter off
	/// any tokens that <b>match</b> the regular
	/// expression (and not the ones that don't)</remarks>
	[Serializable]
	public class RegexTokenFilter : ConditionalFilter
	{
		Regex _regex;

		/// <summary>
		/// Creates a new filter that will filter off any 
		/// tokens that match the regular expression passed
		/// as argument.
		/// </summary>
		/// <param name="regex">the regular expression</param>
		public RegexTokenFilter(string regex) : this(null, regex)
		{		
		}		

		/// <summary>
		/// Creates a new filter that will filter off any
		/// tokens that match the regular expression passed
		/// as argument.
		/// </summary>
		/// <param name="previous">the previous tokenizer in the chain</param>
		/// <param name="regex">the regular expression</param>
		public RegexTokenFilter(ITokenizer previous, string regex) : base(previous)
		{
			if (null == regex)
			{
				throw new ArgumentNullException("regex", "regex can't be null!");
			}
			_regex = new Regex(regex);
		}

		/// <summary>
		/// Creates a new filter that will filter off any 
		/// tokens that match the regular expression passed
		/// as argument.
		/// </summary>
		/// <param name="regex">the regular expression</param>
		public RegexTokenFilter(Regex regex) : this(null, regex)
		{				
		}

		/// <summary>
		/// Creates a new filter that will filter off any
		/// tokens that match the regular expression passed
		/// as argument.
		/// </summary>
		/// <param name="previous">the previous tokenizer in the chain</param>
		/// <param name="regex">the regular expression</param>
		public RegexTokenFilter(ITokenizer previous, Regex regex) : base(previous)
		{
			if (null == regex)
			{
				throw new ArgumentNullException("regex", "regex can't be null!");
			}
			_regex = regex;
		}

		/// <summary>
		/// See <see cref="ConditionalFilter.IsValidToken"/> for details.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		protected override bool IsValidToken(Token token)
		{
			return !_regex.IsMatch(token.Value);
		}
	}
}
