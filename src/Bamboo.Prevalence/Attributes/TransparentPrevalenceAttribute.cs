using System;

namespace Bamboo.Prevalence.Attributes
{
	/// <summary>
	/// Marks a class as requiring transparent prevalence. When
	/// this attribute is present the class will always be activated
	/// through <see cref="PrevalenceActivator.CreateTransparentEngine"/>.
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Class)]
	public class TransparentPrevalenceAttribute : System.Attribute
	{		
	}
}
