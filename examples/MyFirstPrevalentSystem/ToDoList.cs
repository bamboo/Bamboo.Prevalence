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

		internal void SetDone()
		{
			_done = true;
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
			task.SetDone();
		}
	}
}
