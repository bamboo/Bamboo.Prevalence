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
