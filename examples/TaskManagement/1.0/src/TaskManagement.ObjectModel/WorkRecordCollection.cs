using System;
using System.Collections;

namespace TaskManagement.ObjectModel
{
	/// <summary>
	/// Uma coleção de registros de trabalho.
	/// </summary>
	[Serializable]
	public class WorkRecordCollection : CollectionBase
	{
		public WorkRecordCollection()
		{
		}

		public WorkRecord this[int index]
		{
			get
			{
				return (WorkRecord)InnerList[index];
			}
		}

		internal void Add(WorkRecord record)
		{
			if (null == record)
			{
				throw new ArgumentNullException("record");
			}
			InnerList.Add(record);
		}
	}
}
