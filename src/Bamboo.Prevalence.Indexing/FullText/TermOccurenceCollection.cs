using System;
using System.Collections;
using System.Text;

namespace Bamboo.Prevalence.Indexing.FullText
{
	[Serializable]
	public class TermOccurrenceCollection : CollectionBase
	{
		public TermOccurrenceCollection()
		{
		}

		public void Add(IndexedField field, int position)
		{
			foreach (TermOccurrence to in InnerList)
			{
				if (to.Field == field)
				{
					to.Add(position);
					return;
				}
			}
			InnerList.Add(new TermOccurrence(field, position));
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			builder.Append("[");
			foreach (TermOccurrence to in InnerList)
			{
				builder.Append(to.ToString());
				builder.Append(", ");
			}
			builder.Append("]");
			return builder.ToString();
		}
	}
}
