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

		[Test]
		public void TestPunctuation()
		{
			string text = "A foo,bar goest! flu? Oh, yes, flu!!! really? yep.\n.\tdidn't think so..yep";
			AssertTokenValues(new StringTokenizer(text),
				"A", "foo", "bar", "goest", "flu",
				"Oh", "yes", "flu", "really", "yep",
				"didn", "t", "think", "so", "yep"
				);
		}

		void AssertTokens(StringTokenizer tokenizer, params Token[] expected)
		{
			foreach (Token t in expected)
			{
				AssertEquals(t, tokenizer.NextToken());
			}
		}

		void AssertTokenValues(StringTokenizer tokenizer, params string[] expectedValues)
		{
			foreach (string value in expectedValues)
			{
				AssertEquals(value, tokenizer.NextToken().Value);
			}
		}
	}
}
