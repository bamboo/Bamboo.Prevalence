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
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Bamboo.Prevalence.Implementation
{
	/// <summary>
	/// Command log writer.
	/// </summary>
	internal sealed class CommandLogWriter
	{
		private FileStream _output;

		private BinaryFormatter _formatter;

		private NumberedFileCreator _fileCreator;		

		public CommandLogWriter(NumberedFileCreator creator, BinaryFormatter formatter)
		{
			_fileCreator = creator;
			_formatter = formatter;			
		}

		/// <summary>
		/// Writes a command to the current log file. If necessary
		/// UndoWriteCommand can be called before the next WriteCommand
		/// call to undo this operation.
		/// </summary>
		/// <param name="command">serializable command</param>
		public void WriteCommand(ICommand command)
		{
			CheckOutputLog();

			_formatter.Serialize(_output, command);
			Flush(_output);
		}

		public void TakeSnapshot(object system)
		{
			CloseOutputLog();

			FileInfo snapshotFile = _fileCreator.NewSnapshot();
			FileInfo tempFile = CreateTempForSnapshotFile(snapshotFile);
			try
			{				
				using (System.IO.FileStream stream = tempFile.OpenWrite())
				{				
					_formatter.Serialize(stream, system);
					Flush(stream);				
				}
				tempFile.MoveTo(snapshotFile.FullName);
			}
			catch
			{
				// TODO: log errors...
				try { tempFile.Delete(); } catch { }
				throw;
			}
		}	

		public void CloseOutputLog()
		{
			if (null != _output)
			{				
				_output.Close();
				_output = null;
			}
		}

		private void CheckOutputLog()
		{
			if (null == _output)
			{
				_output = NextOutputLog();
			}
		}

		private static FileInfo CreateTempForSnapshotFile(FileInfo info)
		{
			return new FileInfo(Path.ChangeExtension(info.FullName, "tmp"));
		}

		private static void Flush(System.IO.FileStream stream)
		{
			stream.Flush();
			FlushFileBuffers(stream);			
		}

		private static void FlushFileBuffers(System.IO.FileStream stream)
		{
			if (0 == FlushFileBuffers(stream.Handle))
			{
				Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
			}
		}

		// TODO: Remove if at all possible this Win32 dependency
		[DllImport("KERNEL32.DLL", EntryPoint="FlushFileBuffers", PreserveSig=true, CallingConvention=CallingConvention.Winapi, SetLastError=true)]
		private static extern int FlushFileBuffers(IntPtr handle);

		private System.IO.FileStream NextOutputLog()
		{				
			// FileShare.Read: don't prevent anyone from 
			// reading the log
			return _fileCreator.NewOutputLog().Open(
				FileMode.CreateNew, FileAccess.Write, FileShare.Read
				);
		}
	}
}
