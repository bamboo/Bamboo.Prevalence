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
			_record2 = CreateRecord("Márcia", 26);
			_record3 = CreateRecord("Bamboo", 27);

			_result = new SearchResult();
			_result.Add(new SearchHit(_record1));
			_result.Add(new SearchHit(_record2));
			_result.Add(new SearchHit(_record3));
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
