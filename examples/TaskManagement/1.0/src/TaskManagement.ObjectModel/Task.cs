#region License
// Bamboo.Prevalence - a .NET object prevalence engine
// Copyright (C) 2002 Rodrigo B. de Oliveira
//
// Based on the original concept and implementation of Prevayler (TM)
// by Klaus Wuestefeld. Visit http://www.prevayler.org for details.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//
// As a special exception, if you link this library with other files to
// produce an executable, this library does not by itself cause the
// resulting executable to be covered by the GNU General Public License.
// This exception does not however invalidate any other reasons why the
// executable file might be covered by the GNU General Public License.
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
