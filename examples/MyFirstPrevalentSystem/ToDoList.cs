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
using System.Collections;
using Bamboo.Prevalence;

namespace MyFirstPrevalentSystem
{
	/// <summary>
	/// A task in my ToDoList.
	/// </summary>
	[Serializable]
	public class Task
	{
		private int _id;
		
		private string _summary;

		private bool _done;

		private DateTime _dateCreated;
		
		public Task()
		{
			_id = -1;			
		}
		
		public int ID
		{
			get
			{
				return _id;
			}

			set
			{
				if (-1 != _id)
				{
					throw new InvalidOperationException("ID cannot be changed!");
				}
				_id = value;
			}
		}
		
		public string Summary
		{
			get
			{
				return _summary;
			}
			
			set
			{
				_summary = value;
			}
		}

		public bool Done
		{
			get
			{
				return _done;
			}

			set
			{
				_done = value;
			}
		}

		public DateTime DateCreated
		{
			get
			{
				return _dateCreated;
			}

			set
			{
				_dateCreated = value;
			}
		}

		public void Validate()
		{
			if (null == _summary || 0 == _summary.Length)
			{
				throw new ApplicationException("Task.Summary is required!");
			}
		}
	}

	/// <summary>
	/// The prevalent system class.
	/// </summary>
	[Serializable]
	public class ToDoList : System.MarshalByRefObject
	{
		private int _nextTaskID;

		private Hashtable _tasks;

		public ToDoList()
		{
			_tasks = new Hashtable();
		}

		public void AddTask(Task task)
		{
			task.Validate();

			task.ID = _nextTaskID++;

			// we must use PrevalenceEngine.Now as our clock
			// if we want our system to be deterministic
			//
			task.DateCreated = PrevalenceEngine.Now;

			_tasks.Add(task.ID, task);
		}

		public IList PendingTasks
		{
			get
			{
				ArrayList pendingTasks = new ArrayList();
				foreach (Task task in _tasks.Values)
				{
					if (!task.Done)
					{
						pendingTasks.Add(task);
					}
				}
				return pendingTasks;
			}
		}

		public void MarkTaskAsDone(int taskID)
		{
			Task task = _tasks[taskID] as Task;
			if (null == task)
			{
				throw new ArgumentException("Task not found!", "taskID");
			}
			task.Done = true;
		}
	}
}
