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

namespace Bamboo.Prevalence.Indexing.FullText
{
	/// <summary>
	/// 
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
		/// <param name="tail">last tokenizer in the chain</param>
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

		Token NextToken();		
	}
}
