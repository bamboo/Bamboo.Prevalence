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
	/// A token.
	/// </summary>
	[Serializable]
	public class Token
	{
		int _position;

		string _value;

		/// <summary>
		/// Creates a new token.
		/// </summary>
		/// <param name="value">token image</param>
		/// <param name="position">absolute position of the
		/// image in the original text</param>
		public Token(string value, int position)
		{
			if (null == value)
			{
				throw new ArgumentNullException("value");
			}

			if (position < 0)
			{
				throw new ArgumentOutOfRangeException("occurrences", "position must be a positive number");
			}

			_value = value;
			_position = position;
		}

		/// <summary>
		/// Token image
		/// </summary>
		public string Value
		{
			get
			{
				return _value;
			}

			set
			{
				if (null == value)
				{
					throw new ArgumentNullException("value");
				}
				_value = value;
			}
		}

		/// <summary>
		/// Absolute position in the original text from
		/// which this token was extracted.
		/// </summary>
		public int Position
		{
			get
			{
				return _position;
			}
		}

		/// <summary>
		/// Tokens are equal if both properties, 
		/// Value and Position, are considered
		/// equal.
		/// </summary>
		/// <param name="other">object to test equality for</param>
		/// <returns>true if the objects are considered equal</returns>
		public override bool Equals(object other)
		{
			Token token = other as Token;
			if (null == token)
			{
				return false;
			}
			return _position == token._position && _value == token._value;
		}

		/// <summary>
		/// Calculates a hashcode based on the properties
		/// Value and Position.
		/// </summary>
		/// <returns>the combined hashcode of both properties</returns>
		public override int GetHashCode()
		{
			return _position.GetHashCode() ^ _value.GetHashCode();
		}

		/// <summary>
		/// Builds a more human friendly representation of the token.
		/// </summary>
		/// <returns>a readable representation of the token</returns>
		public override string ToString()
		{
			return "<\"" + _value + "\" at " + _position + ">";
		}
	}
}
