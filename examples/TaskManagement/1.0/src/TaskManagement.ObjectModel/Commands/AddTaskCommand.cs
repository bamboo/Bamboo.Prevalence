using System;
using Bamboo.Prevalence;
using TaskManagement.ObjectModel;

namespace TaskManagement.ObjectModel.Commands
{
	/// <summary>
	/// Adiciona uma tarefa a um projeto.
	/// </summary>
	[Serializable]
	public class AddTaskCommand : ICommand
	{
		protected Guid _projectID;

		protected Task _task;

		public AddTaskCommand(Guid projectID, Task task)
		{
			if (null == task)
			{
				throw new ArgumentNullException("task");
			}

			_projectID = projectID;
			_task = task;
		}

		public object Execute(object system)
		{
			TaskManagementSystem tasksystem = (TaskManagementSystem)system;
			Project project = tasksystem.Projects[_projectID];
			project.Tasks.Add(_task);
			return null;
		}
	}
}
