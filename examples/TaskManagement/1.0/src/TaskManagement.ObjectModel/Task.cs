using System;

namespace TaskManagement.ObjectModel
{
	/// <summary>
	/// Uma tarefa.
	/// </summary>
	[Serializable]
	public class Task
	{
		protected Guid _id;

		protected string _name;

		protected WorkRecordCollection _workRecords;

		/// <summary>
		/// Cria uma nova tarefa com o nome
		/// especificado.
		/// </summary>
		/// <param name="name"></param>
		public Task(string name)
		{
			_id = Guid.NewGuid();
			_name = name;
			_workRecords = new WorkRecordCollection();
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
		/// Nome desta tarefa.
		/// </summary>
		public string Name
		{
			get
			{
				return _name;
			}
		}

		/// <summary>
		/// Registros de horas trabalhadas nesta
		/// tarefa.
		/// </summary>
		public WorkRecordCollection WorkRecords
		{
			get
			{
				return _workRecords;
			}
		}
	}
}
