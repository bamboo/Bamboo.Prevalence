using System;
using Bamboo.Prevalence.Indexing.FullText;

namespace Bamboo.Prevalence.Indexing.FullText.Tokenizers
{
	/// <summary>
	/// Splits a string into tokens considering any 
	/// whitespace as separators.
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

		#region Implementation of ITokenizer
		public Bamboo.Prevalence.Indexing.FullText.Token NextToken()
		{			
			SkipWhitespace();
			int begin = _current;
			for (; _current<_text.Length; ++_current)
			{
				if (Char.IsWhiteSpace(_text, _current))
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
		#endregion		

		void SkipWhitespace()
		{
			for (; _current<_text.Length; ++_current)
			{
				if (!Char.IsWhiteSpace(_text, _current))
				{
					break;
				}
			}			
		}
	}
}
