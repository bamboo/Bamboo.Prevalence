using System;
using NUnit.Framework;
using Bamboo.Prevalence.Indexing.FullText;
using Bamboo.Prevalence.Indexing.FullText.Filters;

namespace Bamboo.Prevalence.Indexing.Tests
{
	/// <summary>
	/// Summary description for TokenLengthFilterTest.
	/// </summary>
	[TestFixture]
	public class TokenLengthFilterTest
	{
		[Test]
		public void TestFilter()
		{
			string text = "a bc dado de o";
			TokenAssertions.AssertTokenValues(text,
				new TokenLengthFilter(1),
				"a", "bc", "dado", "de", "o"
				);

			TokenAssertions.AssertTokenValues(text,
				new TokenLengthFilter(2),
				"bc", "dado", "de"
				);

			TokenAssertions.AssertTokenValues(text,
				new TokenLengthFilter(3),
				"dado"
				);

			TokenAssertions.AssertTokenValues(text,
				new TokenLengthFilter(4),
				"dado"
				);
		}
	}
}
