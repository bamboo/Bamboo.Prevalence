using System;
using System.Collections;

namespace TaskManagement.ObjectModel
{
	/// <summary>
	/// Uma coleção de tarefas.
	/// </summary>
	[Serializable]
	public class TaskCollection : CollectionBase
	{
		public TaskCollection()
		{
		}

		public Task this[int index]
		{
			get
			{
				return (Task)InnerList[index];
			}
		}

		public Task this[Guid id]
		{
			get
			{
				foreach (Task task in InnerList)
				{
					if (id == task.ID)
					{
						return task;
					}
				}
				throw new ObjectNotFoundException(string.Format("A tarefa de ID {0} não foi encontrada!", id));
			}
		}

		internal void Add(Task task)
		{
			if (null == task)
			{
				throw new ArgumentNullException("task");
			}
			InnerList.Add(task);
		}
	}
}
