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
