using System;

namespace Bamboo.Prevalence.Indexing.FullText
{
	/// <summary>
	/// Summary description for IndexedField.
	/// </summary>
	[Serializable]
	public class IndexedField
	{
		string _name;

		public IndexedField(string name)
		{
			_name = name;
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}

		public override string ToString()
		{
			return "<IndexedField \"" + _name + "\">";
		}
	}
}
