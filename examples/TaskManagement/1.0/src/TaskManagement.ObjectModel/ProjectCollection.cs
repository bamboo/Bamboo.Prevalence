using System;
using System.Collections;

namespace TaskManagement.ObjectModel
{
	/// <summary>
	/// Uma coleção de projetos.
	/// </summary>
	[Serializable]
	public class ProjectCollection : CollectionBase
	{
		public ProjectCollection()
		{
		}

		public Project this[int index]
		{
			get
			{
				return (Project)InnerList[index];
			}
		}


		/// <summary>
		/// Retorna o projeto de identificador
		/// indicado.
		/// </summary>
		/// <exception cref="ObjectNotFoundException">
		/// caso não exista projeto algum de identificador
		/// igual ao indicado
		/// </exception>
		public Project this[Guid id]
		{
			get
			{
				foreach (Project project in InnerList)
				{
					if (id == project.ID)
					{
						return project;
					}
				}
				throw new ObjectNotFoundException(string.Format("O projeto de ID {0} não foi encontrado!", id));
			}
		}

		internal void Add(Project project)
		{
			if (null == project)
			{
				throw new ArgumentNullException("project");
			}

			InnerList.Add(project);
		}
	}
}
