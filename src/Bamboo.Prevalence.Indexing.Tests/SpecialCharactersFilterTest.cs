using System;
using NUnit.Framework;
using Bamboo.Prevalence.Indexing.FullText;
using Bamboo.Prevalence.Indexing.FullText.Filters;
using Bamboo.Prevalence.Indexing.FullText.Tokenizers;

namespace Bamboo.Prevalence.Indexing.Tests
{
	/// <summary>
	/// Summary description for SpecialCharactersFilterTest.
	/// </summary>
	[TestFixture]
	public class SpecialCharactersFilterTest : Assertion
	{
		[Test]
		public void TestReplacements()
		{
			string text = "·ÈÌÛ˙ calÁ„o aviıes ¡ÌÛ˙·”";;
			StringTokenizer tokenizer = new StringTokenizer(text);
			AssertTokens(new SpecialCharactersFilter(),	tokenizer, 
				new Token("aeiou", 0),
				new Token("calcao", 6),
				new Token("avioes", 13),
				new Token("aiouao", 20),
				null
				);
		}

		void AssertTokens(ITokenFilter filter, ITokenizer tokenizer, params Token[] tokens)
		{
			foreach (Token expected in tokens)
			{
				AssertEquals(expected, filter.Filter(tokenizer));
			}
		}
	}
}
