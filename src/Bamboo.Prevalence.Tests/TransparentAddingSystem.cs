using System;
using NUnit.Framework;
using Bamboo.Prevalence;

namespace Bamboo.Prevalence.Tests
{
	/// <summary>
	/// An transparently prevalent class. The methods can be
	/// directly exposed to clients, there's no need to use command
	/// and query objects (they will be created automatically
	/// by the system as needed).
	/// </summary>
	[Serializable]
	public class TransparentAddingSystem : System.MarshalByRefObject, IAddingSystem
	{
		private int _total;

		public TransparentAddingSystem()
		{		
		}

		public int Total
		{
			// property accessor are treated as query objects (read lock)
			get
			{
				AssertIsPrevalenceEngineCall();

				return _total;
			}
		}

		// public methods are treated as command objects (write lock)
		// unless the attribute Query has been applied to the method.
		public int Add(int amount)
		{
			AssertIsPrevalenceEngineCall();

			if (amount < 0)
			{
				throw new ArgumentOutOfRangeException("amount", amount, "amount must be positive!");
			}
			_total += amount;
			return _total;
		}

		[Bamboo.Prevalence.Attributes.PassThrough]
		public void PassThroughMethod()
		{
			Assertion.AssertNull("PassThrough should prevent engine call!", PrevalenceEngine.Current);
		}

		private void AssertIsPrevalenceEngineCall()
		{
			// Just making sure that this call was intercepted and
			// the PrevalenceEngine is available!
			Assertion.AssertNotNull("PrevalenceEngine.Current", PrevalenceEngine.Current);
		}
	}
}
