using System;
using Bamboo.Prevalence;
using TaskManagement.ObjectModel;

namespace TaskManagement.ObjectModel.Commands
{
	/// <summary>
	/// Adiciona um registro de horas trabalhadas a
	/// uma tarefa.
	/// </summary>
	[Serializable]
	public class AddWorkRecordCommand : ICommand
	{
		protected Guid _projectID;

		protected Guid _taskID;

		protected WorkRecord _record;

		public AddWorkRecordCommand(Guid projectID, Guid taskID, WorkRecord record)
		{
			if (null == record)
			{
				throw new ArgumentNullException("record");
			}

			_projectID = projectID;
			_taskID = taskID;
			_record = record;
		}

		public object Execute(object system)
		{
			TaskManagementSystem tasksystem = (TaskManagementSystem)system;
			Project project = tasksystem.Projects[_projectID];
			Task task = project.Tasks[_taskID];
			task.WorkRecords.Add(_record);
			return null;
		}
	}
}
