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
	internal sealed class PendingCommandsEnumerator : System.Collections.IEnumerator, IDisposable
	{
		private BinaryFormatter _formatter;

		private System.IO.FileStream _currentLogStream;

		private Bamboo.Prevalence.ICommand _current;

		private NumberedFileFinder _fileFinder;

		internal PendingCommandsEnumerator(NumberedFileFinder finder, BinaryFormatter formatter)
		{
			if (null == finder)
			{
				throw new ArgumentNullException("finder");
			}
			if (null == formatter)
			{
				throw new ArgumentNullException("formatter");
			}
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

		public void Dispose()
		{
			CloseCurrentStream();
			_fileFinder = null;
		}

		private void CloseCurrentStream()
		{
			if (_currentLogStream != null)
			{
				_currentLogStream.Close();
			}
		}

		private bool IsAtEnd(System.IO.FileStream stream)
		{
			return stream.Position == stream.Length;
		}

		private System.IO.FileStream NextLogStream()
		{
			if (null == _fileFinder)
			{
				throw new ObjectDisposedException("PendingCommandsEnumerator");
			}

			CloseCurrentStream();

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
