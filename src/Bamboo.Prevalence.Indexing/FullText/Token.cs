using System;

namespace Bamboo.Prevalence.Indexing.FullText
{
	/// <summary>
	/// Summary description for Token.
	/// </summary>
	[Serializable]
	public class Token
	{
		int _position;

		string _value;

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

		public int Position
		{
			get
			{
				return _position;
			}
		}

		public override bool Equals(object other)
		{
			Token token = other as Token;
			if (null == token)
			{
				return false;
			}
			return _position == token._position && _value == token._value;
		}

		public override int GetHashCode()
		{
			return _position.GetHashCode() ^ _value.GetHashCode();
		}

		public override string ToString()
		{
			return "<\"" + _value + "\" at " + _position + ">";
		}
	}
}
