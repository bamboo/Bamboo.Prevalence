using System;
using System.Security;
using System.Security.Principal;
using System.Threading;
using NUnit.Framework;

namespace Bamboo.Prevalence.Tests
{
	/// <summary>
	/// PrincipalSensitivePrevalenceEngineTest.
	/// </summary>
	[TestFixture]
	public class PrincipalSensitivePrevalenceEngineTest : AbstractPrevalenceEngineTest
	{
		private IPrincipal _adder;

		private IPrincipal _anonymous;

		protected override System.Type PrevalentSystemType
		{
			get
			{
				return typeof(PrincipalSensitiveAddingSystem);
			}
		}

		protected IAddingSystem AddingSystem
		{
			get
			{
				return _engine.PrevalentSystem as IAddingSystem;
			}
		}

		protected override void Add(int amount, int expectedTotal)
		{
			IPrincipal saved = Thread.CurrentPrincipal;

			Thread.CurrentPrincipal = _adder;
			AssertEquals("Add", expectedTotal, AddingSystem.Add(amount));

			Thread.CurrentPrincipal = saved;
		}

		protected override void AssertTotal(int total)
		{
			AssertEquals("AssertTotal", total, AddingSystem.Total);
		}

		[SetUp]
		public override void SetUp()
		{
			base.SetUp();

			string[] roles = new string[] { "Adder" };
			_adder = new GenericPrincipal(new GenericIdentity("adder"), roles);
			_anonymous = new GenericPrincipal(new GenericIdentity("anonymous"), new string[0]);
		}

		[Test]
		[ExpectedException(typeof(SecurityException))]
		public void TestAnonymousAdd()
		{	
			Thread.CurrentPrincipal = _anonymous;
			AddingSystem.Add(10);
		}
	}
}
