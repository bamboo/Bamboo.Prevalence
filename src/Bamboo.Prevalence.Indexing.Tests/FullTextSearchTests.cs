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

		HashtableRecord _record1;

		HashtableRecord _record2;

		HashtableRecord _record3;

		[SetUp]
		public void SetUp()
		{
			FullTextSearchIndex index = new FullTextSearchIndex();
			index.Fields.Add("Title");

			_index = index;
			
			_record1 = new HashtableRecord();
			_record1["Title"] = "Bolo de Chocolate";
			_record1["Calories"] = 300;
			_index.Add(_record1);
			DumpPostings(index.Postings);

			_record2 = new HashtableRecord();
			_record2["Title"] = "Bolo de Açafrão";
			_record2["Calories"] = 100;
			_index.Add(_record2);
			DumpPostings(index.Postings);

			_record3 = new HashtableRecord();
			_record3["Title"] = "Torta de Chocolate";
			_record3["Calories"] = 400;
			_index.Add(_record3);
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
		public void TestRemove()
		{
			_index.Remove(_record1);

			ISearchExpression expression = new FullTextSearchExpression("bolo chocolate");
			AssertSearchContains(_index.Search(expression), _record2, _record3);
		}

		[Test]
		public void TestUpdate()
		{
			_record1["Title"] = "Torta de Salsinha";
			_index.Update(_record1);

			ISearchExpression expression = new FullTextSearchExpression("torta salsinha");
			AssertSearchContains(_index.Search(expression), _record1, _record3);
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
				Assert(string.Format("result.Contains({0})", record), result.Contains(record));
			}
			AssertEquals("result.Count", expected.Length, result.Count);
		}

		void DumpPostings(Postings[] postings)
		{
			Console.WriteLine("\n**************");
			foreach (Postings p in postings)
			{
				Console.WriteLine(p.ToString());
			}
			Console.WriteLine("**************");
		}
	}
}
