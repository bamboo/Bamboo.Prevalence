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
		/// Identificador �nico deste objeto.
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
		/// Tarefas que comp�em este projeto.
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
