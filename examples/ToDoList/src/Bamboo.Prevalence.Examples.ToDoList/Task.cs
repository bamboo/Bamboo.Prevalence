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

namespace Bamboo.Prevalence.Examples.ToDoList
{
	/// <summary>
	/// Models a task.
	/// </summary>
	[Serializable]
	public class Task
	{
		public enum TaskPriority
		{
			Low,
			Normal,
			High
		}

		public enum TaskStatus
		{
			Uninitialized,
			Created,
			Assigned,
			Completed			
		}

		private long _id;

		private string _owner;

		private string _summary;

		private string _description;

		private int _estimatedHoursOfWork;

		private int _actualHoursOfWork;

		private DateTime _startDate;

		private DateTime _endDate;

		private DateTime _createdTime;		
		
		private TaskPriority _priority;

		private TaskStatus _status;

		public Task()
		{
			_id = -1;
			_priority = TaskPriority.Normal;
			_status = TaskStatus.Uninitialized;
		}

		public long ID
		{
			get
			{
				return _id;
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

		public string Description
		{
			get
			{
				return _description;
			}

			set
			{
				_description = value;
			}
		}

		public string Owner
		{
			get
			{
				return _owner;
			}
		}

		public DateTime CreatedTime
		{
			get
			{
				return _createdTime;
			}
		}

		public DateTime StartDate
		{
			get
			{
				return _startDate;
			}

			set
			{
				_startDate = value;
			}
		}

		public DateTime EndDate
		{
			get
			{
				return _endDate;
			}

			set
			{
				_endDate = value;
			}
		}

		public int EstimatedHoursOfWork
		{
			get
			{
				return _estimatedHoursOfWork;
			}

			set
			{
				_estimatedHoursOfWork = value;
			}
		}

		public int ActualHoursOfWork
		{
			get
			{
				return _actualHoursOfWork;
			}

			set
			{
				_actualHoursOfWork = value;
			}
		}

		public TaskPriority Priority
		{
			get
			{
				return _priority;
			}

			set
			{
				_priority = value;
			}
		}

		public TaskStatus Status
		{
			get
			{
				return _status;
			}
		}		

		internal void Assign(string owner)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner", "owner can't be null!");
			}

			if (owner.Length == 0)
			{
				throw new ArgumentOutOfRangeException("owner", owner, "owner can't be empty!");
			}

			if (_status != TaskStatus.Created)
			{
				throw new InvalidOperationException("Can't assign owner to a completed or already assigned task!");
			}

			_owner = owner;
			_status = TaskStatus.Assigned;
		}

		internal void Initialize(long id, DateTime created)
		{
			ValidateForInitialize();
			
			_status = TaskStatus.Created;
			_id = id;
			_createdTime = created;
		}

		private void ValidateForInitialize()
		{
			if (_status != TaskStatus.Uninitialized)
			{
				throw new InvalidOperationException("Task can't be initialized twice!");
			}

			AssertFieldIsSet("Task.Summary", _summary);			
			AssertFieldIsSet("Task.Description", _description);			
		}

		private void AssertFieldIsSet(string fieldName, string value)
		{
			if (value == null || value.Length == 0)
			{
				throw new ApplicationException(fieldName + " must be set!");
			}
		}
	}
}
