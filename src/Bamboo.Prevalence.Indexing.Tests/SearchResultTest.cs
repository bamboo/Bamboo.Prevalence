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
using Bamboo.Prevalence.Indexing;
using Bamboo.Prevalence.Indexing.Records;

namespace Bamboo.Prevalence.Indexing.Tests
{
	class AgeFilter : ISearchHitFilter
	{
		int _min;
		int _max;

		public AgeFilter(int min, int max)
		{
			_min = min;
			_max = max;
		}

		public bool Test(SearchHit hit)
		{
			int age = (int)hit.Record["Age"];
			return (age >= _min && age <= _max);
		}
	}

	class NameComparer : System.Collections.IComparer
	{
		public int Compare(object a, object b)
		{
			HashtableRecord r1 = (HashtableRecord)a;
			HashtableRecord r2 = (HashtableRecord)b;
			return ((IComparable)r1["Name"]).CompareTo(r2["Name"]);
		}
	}

	/// <summary>
	/// Summary description for SearchResultTest.
	/// </summary>
	[TestFixture]
	public class SearchResultTest : Assertion
	{
		HashtableRecord _record1;

		HashtableRecord _record2;

		HashtableRecord _record3;

		SearchResult _result;

		[SetUp]
		public void SetUp()
		{
			_record1 = CreateRecord("Teresa", 43);
			_record2 = CreateRecord("M�rcia", 26);
			_record3 = CreateRecord("Bamboo", 27);

			_result = new SearchResult();
			_result.Add(new SearchHit(_record1));
			_result.Add(new SearchHit(_record2));
			_result.Add(new SearchHit(_record3));
		}

		[Test]
		public void TestToRecordArray()
		{
			HashtableRecord[] records = (HashtableRecord[])_result.ToRecordArray(typeof(HashtableRecord));
			AssertEquals(3, records.Length);
			AssertSame(_record1, records[0]);
			AssertSame(_record2, records[1]);
			AssertSame(_record3, records[2]);			
		}

		[Test]
		public void TestSortByField()
		{
			_result.SortByField("Name");
			AssertSearchHits(_result, _record3, _record2, _record1);

			_result.SortByField("Age");
			AssertSearchHits(_result, _record2, _record3, _record1);
		}

		[Test]
		public void TestSort()
		{
			_result.Sort(new NameComparer());
			AssertSearchHits(_result, _record3, _record2, _record1);
		}

		[Test]
		public void TestFilter()
		{
			AssertSearchHits(_result.Filter(new AgeFilter(25, 28)),
				_record2, _record3);
		}

		[Test]
		public void TestIntersect()
		{
			SearchResult other = new SearchResult();
			other.Add(new SearchHit(_record3));
			other.Add(new SearchHit(_record1));
			
			AssertSearchHits(_result.Intersect(other), _record1, _record3);

			other = new SearchResult();
			other.Add(new SearchHit(_record2));
			
			AssertSearchHits(_result.Intersect(other), _record2);

			AssertEquals(0, _result.Intersect(new SearchResult()).Count);
			AssertSearchHits(_result.Intersect(_result), _record1, _record2, _record3);
		}

		[Test]
		public void TestForEach()
		{
			int i=0;
			IRecord[] expected = { _record1, _record2, _record3 };
			foreach (SearchHit hit in _result)
			{
				AssertEquals(expected[i++], hit.Record);
			}
		}

		void AssertSearchHits(SearchResult result, params IRecord[] expectedRecords)
		{
			AssertEquals(expectedRecords.Length, result.Count);
			for (int i=0; i<expectedRecords.Length; ++i)
			{
				IRecord expected = expectedRecords[i];
				IRecord actual = result[i].Record;
				AssertEquals(string.Format("{0} != {1}", expected["Name"], actual["Name"]), expected, actual);
			}
		}

		HashtableRecord CreateRecord(string name, int age)
		{
			HashtableRecord record = new HashtableRecord();
			record["Name"] = name;
			record["Age"] = age;
			return record;
		}
	}
}
