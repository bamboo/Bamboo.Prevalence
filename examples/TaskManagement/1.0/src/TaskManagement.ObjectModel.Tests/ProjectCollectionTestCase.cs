using System;
using NUnit.Framework;
using TaskManagement.ObjectModel;

namespace TaskManagement.ObjectModel.Tests
{
	/// <summary>
	/// Testes para ProjectCollection.
	/// </summary>
	[TestFixture]
	public class ProjectCollectionTestCase : Assertion
	{
		ProjectCollection _collection;

		[SetUp]
		public void SetUp()
		{
			_collection = new ProjectCollection();
		}

		[Test]
		public void TestConstruct()
		{
			AssertEquals("A coleção deve estar vazia!", 0, _collection.Count);
		}
	}
}
