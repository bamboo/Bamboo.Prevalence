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
			if (!_paused)
			{	
				_paused = true;
				_now = DateTime.Now;
			}
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
		/// this will be the value returned by <see cref="Now" /></param>
		public void PauseAt(DateTime date)
		{
			_paused = true;
			_now = date;
		}
	}
}
