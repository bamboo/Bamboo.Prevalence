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
