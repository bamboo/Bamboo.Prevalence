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
