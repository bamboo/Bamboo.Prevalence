using System;
using NUnit.Framework;
using Bamboo.Prevalence.Indexing.FullText;
using Bamboo.Prevalence.Indexing.FullText.Tokenizers;

namespace Bamboo.Prevalence.Indexing.Tests
{
	/// <summary>
	/// Tests for the StringTokenizer class.
	/// </summary>
	[TestFixture]
	public class StringTokenizerTest : Assertion
	{
		[Test]
		public void TestSimpleStrings()
		{
			string text = "a foo Bar aça\n45\n\n\n";			
			AssertTokens(new StringTokenizer(text),
				new Token("a", 0),
				new Token("foo", 2),
				new Token("Bar", 6),
				new Token("aça", 10),
				new Token("45", 14),
				null
				);

			AssertTokens(new StringTokenizer(""),
				(Token)null);

			AssertTokens(new StringTokenizer("\n\t   "),
				(Token)null);

			AssertTokens(new StringTokenizer("\n\t  a"),
				new Token("a", 4),
				null);
		}

		void AssertTokens(StringTokenizer tokenizer, params Token[] expected)
		{
			foreach (Token t in expected)
			{
				AssertEquals(t, tokenizer.NextToken());
			}
		}
	}
}
