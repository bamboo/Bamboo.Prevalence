using System;

namespace TaskManagement.ObjectModel
{
	/// <summary>
	/// Um projeto.
	/// </summary>
	[Serializable]
	public class Project
	{
		protected Guid _id;

		protected string _name;

		protected TaskCollection _tasks;

		/// <summary>
		/// Cria um novo projeto com o nome
		/// especificado.
		/// </summary>
		/// <param name="name"></param>
		public Project(string name)
		{
			_id = Guid.NewGuid();
			_name = name;
			_tasks = new TaskCollection();
		}

		/// <summary>
		/// Identificador único deste objeto.
		/// </summary>
		public Guid ID
		{
			get
			{
				return _id;
			}
		}

		/// <summary>
		/// Nome deste projeto.
		/// </summary>
		public string Name
		{
			get
			{
				return _name;
			}
		}

		/// <summary>
		/// Tarefas que compõem este projeto.
		/// </summary>
		public TaskCollection Tasks
		{
			get
			{
				return _tasks;
			}
		}
	}
}
