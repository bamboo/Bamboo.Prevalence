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
