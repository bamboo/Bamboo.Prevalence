using System;
using Bamboo.Prevalence;
using TaskManagement.ObjectModel;

namespace TaskManagement.ObjectModel.Commands
{
	/// <summary>
	/// Adiciona um projeto ao sistema.
	/// </summary>
	[Serializable]
	public class AddProjectCommand : ICommand
	{
		protected Project _project;

		public AddProjectCommand(Project project)
		{
			if (null == project)
			{
				throw new ArgumentNullException("project");
			}
			_project = project;
		}

		public object Execute(object system)
		{
			TaskManagementSystem tasksystem = (TaskManagementSystem)system;
			tasksystem.Projects.Add(_project);
			return null;
		}
	}
}
