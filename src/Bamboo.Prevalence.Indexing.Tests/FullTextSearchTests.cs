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
using Bamboo.Prevalence.Indexing;
using Bamboo.Prevalence.Indexing.FullText;
using Bamboo.Prevalence.Indexing.Records;

namespace Bamboo.Prevalence.Indexing.Tests
{
	/// <summary>
	/// Test cases for the fulltext indexing/search support.
	/// </summary>
	[TestFixture]
	public class FullTextSearchTests : Assertion
	{
		IIndex _index;
		
		IIndex _multipleFieldIndex;

		HashtableRecord _record1;

		HashtableRecord _record2;

		HashtableRecord _record3;

		[SetUp]
		public void SetUp()
		{
			FullTextSearchIndex index = new FullTextSearchIndex();
			index.Fields.Add("Title");

			FullTextSearchIndex multipleFieldIndex = new FullTextSearchIndex();
			multipleFieldIndex.Fields.Add("Title");
			multipleFieldIndex.Fields.Add("Ingredients");

			_index = index;
			_multipleFieldIndex = multipleFieldIndex;
			
			_record1 = new HashtableRecord();
			_record1["Title"] = "Bolo de Chocolate";
			_record1["Calories"] = 300;
			_record1["Ingredients"] = "3 colheres de açucar\n1 lata de nescau\nfermento";
			_index.Add(_record1);
			_multipleFieldIndex.Add(_record1);
			DumpPostings(index.Postings);

			_record2 = new HashtableRecord();
			_record2["Title"] = "Bolo de Açafrão";
			_record2["Calories"] = 100;
			_record2["Ingredients"] = "10 folhas de açafrão\n1 colher de fermento em pó";
			_index.Add(_record2);
			_multipleFieldIndex.Add(_record2);
			DumpPostings(index.Postings);

			_record3 = new HashtableRecord();
			_record3["Title"] = "Torta de Chocolate";
			_record3["Calories"] = 400;
			_record3["Ingredients"] = "1 lata de nescau\nchocolate granulado\naçucar";
			_index.Add(_record3);
			_multipleFieldIndex.Add(_record3);
			DumpPostings(index.Postings);
		}

		[Test]
		public void TestSimpleSearch()
		{			
			ISearchExpression expression = new FullTextSearchExpression("bolo");
			AssertSearchContains(_index.Search(expression), _record1, _record2);

			expression = new FullTextSearchExpression("chocolate");
			AssertSearchContains(_index.Search(expression), _record1, _record3);

			expression = new FullTextSearchExpression("acafrão");
			AssertSearchContains(_index.Search(expression), _record2);

			expression = new FullTextSearchExpression("bolo AcaFrao");
			AssertSearchContains(_index.Search(expression), _record1, _record2);			
		}

		[Test]
		public void TestMultiIndexSimpleSearch()
		{
			ISearchExpression expression = new FullTextSearchExpression("nescau");
			AssertSearchContains(_multipleFieldIndex.Search(expression), _record1, _record3);

			expression = new FullTextSearchExpression("chocolate");
			AssertSearchContains(_multipleFieldIndex.Search(expression), _record1, _record3);

			expression = new FullTextSearchExpression("fermento");
			AssertSearchContains(_multipleFieldIndex.Search(expression), _record1, _record2);
		}

		[Test]
		public void TestIncludeAllSearch()
		{
			ISearchExpression expression = new FullTextSearchExpression("Bolo Chocolate", FullTextSearchMode.IncludeAll);
			AssertSearchContains(_index.Search(expression), _record1);
			AssertSearchContains(_multipleFieldIndex.Search(expression), _record1);

			expression = new FullTextSearchExpression("Bolo Açafrão", FullTextSearchMode.IncludeAll);
			AssertSearchContains(_index.Search(expression), _record2);
			AssertSearchContains(_multipleFieldIndex.Search(expression), _record2);

			expression = new FullTextSearchExpression("Torta Chocolate", FullTextSearchMode.IncludeAll);
			AssertSearchContains(_index.Search(expression), _record3);
			AssertSearchContains(_multipleFieldIndex.Search(expression), _record3);
		}

		[Test]
		public void TestMultiIndexIncludeAllSearch()
		{
			AssertSearchContains(
				_multipleFieldIndex.Search(new FullTextSearchExpression("bolo nescau", FullTextSearchMode.IncludeAll)),
				_record1
				);

			AssertSearchContains(
				_multipleFieldIndex.Search(new FullTextSearchExpression("torta nescau", FullTextSearchMode.IncludeAll)),
				_record3
				);

			AssertSearchContains(
				_multipleFieldIndex.Search(new FullTextSearchExpression("bolo fermento", FullTextSearchMode.IncludeAll)),
				_record1, _record2
				);
		}

		public void TestRemove()
		{
			_index.Remove(_record1);
			_multipleFieldIndex.Remove(_record1);
			
			AssertSearchContains(
				_index.Search(new FullTextSearchExpression("bolo chocolate")),
				_record2, _record3
				);
			
			AssertSearchContains(
				_multipleFieldIndex.Search(new FullTextSearchExpression("açucar")), 
				_record3
				);
		}

		[Test]
		public void TestUpdate()
		{
			_record1["Title"] = "Torta de Salsinha";
			_index.Update(_record1);
			_multipleFieldIndex.Update(_record1);

			ISearchExpression expression = new FullTextSearchExpression("torta salsinha");
			AssertSearchContains(_index.Search(expression), _record1, _record3);

			AssertSearchContains(_multipleFieldIndex.Search(expression), _record1, _record3);
		}

		[Test]
		public void TestIndexSerialization()
		{			
			_index = SerializeDeserialize(_index) as IIndex;

			FullTextSearchIndex index = _index as FullTextSearchIndex;
			IRecord[] records = index.Records;
			AssertEquals(3, records.Length);
			_record1 = FindByTitle(records, (string)_record1["Title"]);
			_record2 = FindByTitle(records, (string)_record2["Title"]);
			_record3 = FindByTitle(records, (string)_record3["Title"]);
			
			TestSimpleSearch();
		}

		HashtableRecord FindByTitle(IRecord[] records, string title)
		{
			foreach (HashtableRecord record in records)
			{
				if (((string)record["Title"]) == title)
				{
					return record;
				}
			}
			throw new ArgumentException("Record not found!");
		}

		object SerializeDeserialize(object graph)
		{
			BinaryFormatter formatter = new BinaryFormatter();
			MemoryStream stream = new MemoryStream();
			formatter.Serialize(stream, graph);
			
			stream.Position = 0;
			return formatter.Deserialize(stream);
		}

		void AssertSearchContains(SearchResult result, params IRecord[] expected)
		{
			foreach (IRecord record in expected)
			{
				if (!result.Contains(record))
				{
					Fail(string.Format("Expected record \"{0}\"!", record["Title"]));
				}
			}
			AssertEquals("result.Count", expected.Length, result.Count);
		}

		void DumpPostings(Postings[] postings)
		{
			System.Diagnostics.Trace.WriteLine("\n**************");
			foreach (Postings p in postings)
			{
				System.Diagnostics.Trace.WriteLine(p.ToString());
			}
			System.Diagnostics.Trace.WriteLine("**************");
		}
	}
}
