using System;
using System.Collections;

namespace Bamboo.Prevalence.Indexing.FullText
{
	/// <summary>
	/// Definition for the fields that will be used
	/// to compose a <see cref="FullTextSearchIndex"/>.
	/// </summary>
	[Serializable]
	public class IndexedFieldCollection : CollectionBase
	{
		public IndexedFieldCollection()
		{
		}

		public void Add(string field)
		{
			InnerList.Add(new IndexedField(field));
		}
	}
}
