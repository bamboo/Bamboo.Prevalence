using System;

namespace TaskManagement.ObjectModel
{
	/// <summary>
	/// Um sistema para gerenciamento de tarefas.
	/// </summary>
	[Serializable]
	public class TaskManagementSystem
	{
		protected ProjectCollection _projects;

		/// <summary>
		/// Cria um novo sistema para gerenciamento
		/// de tarefas.
		/// </summary>
		public TaskManagementSystem()
		{
			_projects = new ProjectCollection();
		}

		/// <summary>
		/// Cadastro de projetos.
		/// </summary>
		public ProjectCollection Projects
		{
			get
			{
				return _projects;
			}
		}
	}
}
