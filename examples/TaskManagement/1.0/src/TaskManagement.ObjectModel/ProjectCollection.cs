#region license
// Bamboo.Prevalence - a .NET object prevalence engine
// Copyright (C) 2004 Rodrigo B. de Oliveira
//
// Based on the original concept and implementation of Prevayler (TM)
// by Klaus Wuestefeld. Visit http://www.prevayler.org for details.
//
// Permission is hereby granted, free of charge, to any person 
// obtaining a copy of this software and associated documentation 
// files (the "Software"), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge, 
// publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included 
// in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY 
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// Contact Information
//
// http://bbooprevalence.sourceforge.net
// mailto:rodrigobamboo@users.sourceforge.net
#endregion

using System;
using System.Collections;

namespace TaskManagement.ObjectModel
{
	/// <summary>
	/// Uma cole��o de projetos.
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
		/// caso n�o exista projeto algum de identificador
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
				throw new ObjectNotFoundException(string.Format("O projeto de ID {0} n�o foi encontrado!", id));
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
