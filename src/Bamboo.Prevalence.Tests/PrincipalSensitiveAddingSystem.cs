using System;
using System.Security.Permissions;
using NUnit.Framework;
using Bamboo.Prevalence;
using Bamboo.Prevalence.Attributes;

namespace Bamboo.Prevalence.Tests
{
	/// <summary>
	/// IAddingSystem implementation that uses
	/// Role Based security. The attribute PrincipalSensitive
	/// guarantees that the principal associated with the
	/// command will be written to the command log.
	/// </summary>
	[Serializable]
	[TransparentPrevalence]
	[PrincipalSensitive]
	public class PrincipalSensitiveAddingSystem : System.MarshalByRefObject, IAddingSystem
	{
		private int _total;		
		
		[PrincipalPermission(SecurityAction.Demand, Role="Adder")]
		public int Add(int amount)
		{
			if (amount < 0)
			{
				throw new ArgumentOutOfRangeException("amount", amount, "amount must be positive!");
			}

			_total += amount;
			return _total;
		}

		public int Total
		{
			get
			{
				return _total;
			}
		}
	}
}
