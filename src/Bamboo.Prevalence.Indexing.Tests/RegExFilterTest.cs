using System;
using NUnit.Framework;
using Bamboo.Prevalence.Indexing.FullText;
using Bamboo.Prevalence.Indexing.FullText.Filters;
using Bamboo.Prevalence.Indexing.FullText.Tokenizers;

namespace Bamboo.Prevalence.Indexing.Tests
{
	/// <summary>
	/// Summary description for RegExFilterTest.
	/// </summary>
	[TestFixture]
	public class RegExFilterTest
	{
		RegexTokenFilter _filter;

		[SetUp]
		public void SetUp()
		{
			_filter = new RegexTokenFilter(@"[0-9]+\w*");
		}

		[Test]
		public void TestSerializable()
		{
			_filter = (RegexTokenFilter)TokenAssertions.SerializeDeserialize(_filter);
			TestSimpleRegex();
		}

		[Test]
		public void TestSimpleRegex()
		{
			TokenAssertions.AssertTokens("a token, 100 23 1g 144kg other token",
				_filter,
				new Token("a", 0),
				new Token("token", 2),
				new Token("other", 25),
				new Token("token", 31),
				null
				);
		}
	}
}
