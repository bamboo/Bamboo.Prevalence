namespace Bamboo.Prevalence.Attributes
{
	using System;

	[Serializable]
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
	public class SubSystemAttribute : Attribute
	{
		private string _fieldName;

		public SubSystemAttribute(string fieldName)
		{
			_fieldName = fieldName;
		}

		public string FieldName
		{
			get
			{
				return _fieldName;
			}
		}
	}
}
