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
			string text = "a foo Bar a�a\n45\n\n\n";			
			TokenAssertions.AssertTokens(new StringTokenizer(text),
				new Token("a", 0),
				new Token("foo", 2),
				new Token("Bar", 6),
				new Token("a�a", 10),
				new Token("45", 14),
				null
				);

			TokenAssertions.AssertTokens(new StringTokenizer(""),
				(Token)null);

			TokenAssertions.AssertTokens(new StringTokenizer("\n\t   "),
				(Token)null);

			TokenAssertions.AssertTokens(new StringTokenizer("\n\t  a"),
				new Token("a", 4),
				null);
		}

		[Test]
		public void TestPunctuation()
		{
			string text = "A foo,bar goest! flu? Oh, yes, flu!!! really? yep.\n.\tdidn't think [so..(yep)";
			TokenAssertions.AssertTokenValues(new StringTokenizer(text),
				"A", "foo", "bar", "goest", "flu",
				"Oh", "yes", "flu", "really", "yep",
				"didn", "t", "think", "so", "yep"
				);
		}		

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullValues()
		{
			StringTokenizer tokenizer = new StringTokenizer(null);
		}
	}
}
