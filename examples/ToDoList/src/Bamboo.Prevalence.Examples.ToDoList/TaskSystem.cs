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

using System;
using Bamboo.Prevalence;

namespace Bamboo.Prevalence.Examples.ToDoList
{
	/// <summary>
	/// Manages a list of tasks. The system is never accessed
	/// directly by client code but through command and query
	/// objects.
	/// </summary>
	[Serializable]
	public class TaskSystem
	{
		private System.Collections.Hashtable _tasks;

		private long _nextTaskID;

		public TaskSystem()
		{
			_tasks = new System.Collections.Hashtable();	
		}

		internal long AddTask(Task task)
		{
			task.Initialize(_nextTaskID++, PrevalenceEngine.Now);			
			_tasks[task.ID] = task;
			return task.ID;
		}
		
		internal System.Collections.IList GetAllTasks()
		{
			// if we wanted our task system to
			// be more robust we could return
			// clones of the tasks objects instead of
			// returning the original references.
			// Right now I prefer
			// to trust the UI developers and
			// assume they won't be changing the
			// objects behind our back. 
			// If you have comments/suggestions
			// please drop me an email at
			// rodrigobamboo@users.sourceforge.net.
			Task[] tasks = new Task[_tasks.Count];
			_tasks.Values.CopyTo(tasks, 0);
			return tasks;
		}
	}
}
