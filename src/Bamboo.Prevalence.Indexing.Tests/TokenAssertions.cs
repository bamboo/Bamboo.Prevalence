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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;
using Bamboo.Prevalence.Indexing.FullText;
using Bamboo.Prevalence.Indexing.FullText.Tokenizers;

namespace Bamboo.Prevalence.Indexing.Tests
{
	/// <summary>
	/// Summary description for TokenAssertions.
	/// </summary>
	public class TokenAssertions
	{
		public static void AssertTokens(string text, ITokenFilter filter, params Token[] tokens)
		{
			AssertTokens(new StringTokenizer(text), filter, tokens);
		}

		public static void AssertTokens(ITokenizer tokenizer, ITokenFilter filter, params Token[] tokens)
		{
			ITokenizer actual = filter.Clone(tokenizer);
			foreach (Token expected in tokens)
			{
				Assertion.AssertEquals(expected, actual.NextToken());
			}
		}

		public static void AssertTokens(ITokenizer tokenizer, params Token[] tokens)
		{
			foreach (Token expected in tokens)
			{
				Assertion.AssertEquals(expected, tokenizer.NextToken());
			}
		}

		public static void AssertTokenValues(ITokenizer tokenizer, params string[] expectedValues)
		{
			foreach (string value in expectedValues)
			{
				Assertion.AssertEquals(value, tokenizer.NextToken().Value);
			}
			Assertion.AssertNull(tokenizer.NextToken());
		}

		public static void AssertTokenValues(string text, ITokenFilter filter, params string[] expectedValues)
		{
			ITokenizer tokenizer = filter.Clone(new StringTokenizer(text));
			AssertTokenValues(tokenizer, expectedValues);
		}

		public static object SerializeDeserialize(object graph)
		{
			BinaryFormatter formatter = new BinaryFormatter();
			MemoryStream stream = new MemoryStream();
			formatter.Serialize(stream, graph);
			
			stream.Position = 0;
			return formatter.Deserialize(stream);
		}
	}
}
