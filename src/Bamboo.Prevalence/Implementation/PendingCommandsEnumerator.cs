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
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Bamboo.Prevalence;

namespace Bamboo.Prevalence.Implementation
{
	/// <summary>
	/// Enumerates through all the commands in the pending logs
	/// returned by <see cref="NumberedFileFinder.NextPendingLog" />.
	/// </summary>
	internal sealed class PendingCommandsEnumerator : System.Collections.IEnumerator
	{
		private BinaryFormatter _formatter;

		private System.IO.FileStream _currentLogStream;

		private Bamboo.Prevalence.ICommand _current;

		private NumberedFileFinder _fileFinder;

		internal PendingCommandsEnumerator(NumberedFileFinder finder, BinaryFormatter formatter)
		{
			_fileFinder = finder;
			_formatter = formatter;	
		}

		#region Implementation of IEnumerator
		/// <summary>
		/// Throws InvalidOperationException.
		/// </summary>
		public void Reset()
		{
			throw new InvalidOperationException("Reset not supported!");
		}

		/// <summary>
		/// Moves to next command.
		/// </summary>
		/// <returns>true if there are any pending commands</returns>
		public bool MoveNext()
		{
			_current = NextCommand();
			return _current != null;
		}

		/// <summary>
		/// The current command or null (in the case there
		/// are no more pending commands).
		/// </summary>
		public object Current
		{
			get
			{
				return _current;
			}
		}
		#endregion

		private bool IsAtEnd(System.IO.FileStream stream)
		{
			return stream.Position == stream.Length;
		}

		private System.IO.FileStream NextLogStream()
		{
			if (_currentLogStream != null)
			{
				_currentLogStream.Close();
			}

			while (true)
			{
				System.IO.FileInfo nextLog = _fileFinder.NextPendingLog();
				if (nextLog.Exists)
				{
					if (nextLog.Length > 0)
					{
						// Open the log file with
						// FileShare.ReadWrite
						// because the crashed prevalence engine might
						// not have closed it
						// TODO: Is this really necessary/desired?
						return nextLog.Open(
							FileMode.Open, FileAccess.Read, FileShare.ReadWrite
							);
					}
				}
				else
				{
					break;
				}
			}

			return null;
		}

		private ICommand NextCommand()
		{
			if (null == _currentLogStream || IsAtEnd(_currentLogStream))
			{
				_currentLogStream = NextLogStream();			
				if (null == _currentLogStream)
				{
					return null;
				}
			}	

			return (ICommand)_formatter.Deserialize(_currentLogStream);
		}		
	}
}
