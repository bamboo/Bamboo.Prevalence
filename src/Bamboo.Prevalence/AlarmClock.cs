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

namespace Bamboo.Prevalence
{
	/// <summary>
	/// A clock that can be used by a prevalent system for all
	/// its date/time related functions.
	/// </summary>	
	public class AlarmClock
	{
		private bool _paused;

		private DateTime _now;

		/// <summary>
		/// Creates a new clock.
		/// </summary>
		public AlarmClock()
		{
			_paused = false;
		}

		/// <summary>
		/// The current system/time.
		/// </summary>
		public DateTime Now
		{
			get
			{
				if (_paused)
				{
					return _now;
				}
				return DateTime.Now;
			}
		}

		/// <summary>
		/// Pauses the clock.  The property <see cref="Now" />
		/// will keep on
		/// returning the same value until <see cref="Resume" />
		/// or <see cref="Recover" /> gets called.
		/// </summary>
		/// <remarks>
		/// You should not call this method. It is used internally by the prevalence engine
		/// to execute a command as if in a specific moment in time.
		/// </remarks>
		public void Pause()
		{
			if (_paused)
			{
				throw new InvalidOperationException("AlarmClock is already paused!");
			}

			_paused = true;
			_now = DateTime.Now;
		}

		/// <summary>
		/// Resumes the clock. The property Now returns to
		/// its normal behavior.
		/// </summary>
		/// <remarks>
		/// You should not call this method. It is used internally by the prevalence engine
		/// to execute a command as if in a specific moment in time.
		/// </remarks>
		public void Resume()
		{
			if (!_paused)
			{
				throw new InvalidOperationException("AlarmClock is not paused!");
			}
			_paused = false;
		}

		/// <summary>
		/// Sets the clock to a specific DateTime value. This
		/// method can only be called when paused.
		/// </summary>
		/// <remarks>
		/// You should not call this method. It is used internally by the prevalence engine
		/// to execute a command as if in a specific moment in time.
		/// </remarks>
		/// <param name="date">the specific date/time value the clock should be set to,
		/// this will be the value returned by <see cref="Now"/></param>		
		public void Recover(DateTime date)
		{
			if (!_paused)
			{
				throw new InvalidOperationException("The clock must be paused before Recover can be called!");
			}

			if (date > DateTime.Now)
			{
				throw new ArgumentOutOfRangeException("date", date, "Can't recover to a date in the future!");
			}

			_now = date;
		}
		
		/// <summary>
		/// This method can be used for testing date sensitive code.
		/// </summary>
		/// <remarks>
		/// You should not call this method in production code. It's meant
		/// to be used in test cases only.
		/// </remarks>
		/// <param name="date">the specific date/time value the clock should be set to,
		/// this weill be the value returned by <see cref="Now" /></param>
		public void PauseAt(DateTime date)
		{
			_paused = true;
			_now = date;
		}
	}
}
