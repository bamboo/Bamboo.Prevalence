using System;

namespace Bamboo.Prevalence.Tests
{
	/// <summary>
	/// IAddingSystem interface. Allow our commands to be applied to both
	/// AddingSystem and TransparentAddingSystem.
	/// </summary>
	public interface IAddingSystem
	{
		int Total
		{
			get;
		}

		int Add(int amount);
	}
}
