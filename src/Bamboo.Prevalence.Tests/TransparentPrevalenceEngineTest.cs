using System;
using NUnit.Framework;
using Bamboo.Prevalence;

namespace Bamboo.Prevalence.Tests
{
	/// <summary>
	/// Summary description for TransparentPrevalenceEngineTest.
	/// </summary>
	[TestFixture]
	public class TransparentPrevalenceEngineTest : AbstractPrevalenceEngineTest
	{
		protected override System.Type PrevalentSystemType
		{
			get
			{
				return typeof(TransparentAddingSystem);
			}
		}

		protected TransparentAddingSystem AddingSystem
		{
			get
			{
				return _engine.PrevalentSystem as TransparentAddingSystem;
			}
		}

		protected override void Add(int amount, int expected)
		{
			AssertEquals("Add", expected, AddingSystem.Add(amount));
		}

		protected override void AssertTotal(int expected)
		{
			AssertEquals("Total", expected, AddingSystem.Total);
		}

		[Test]
		public void TestPassThroughAttribute()
		{			
			AddingSystem.PassThroughMethod();
		}
	}
}
