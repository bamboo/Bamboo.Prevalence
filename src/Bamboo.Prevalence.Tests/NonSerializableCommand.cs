using System;

namespace Bamboo.Prevalence.Tests
{
	/// <summary>
	/// A non serializable command to make sure Bamboo.Prevalence
	/// is able to safely and gracefully recover from such
	/// situation.
	/// </summary>
	public class NonSerializableCommand : Bamboo.Prevalence.ICommand
	{
		public object Execute(object system)
		{
			return null;
		}
	}
}
