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
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Configuration;

#region Mono Support
namespace System.IO
{
	using System.Runtime.CompilerServices;

	internal enum MonoIOError: int { ERROR_SUCCESS = 0, ERROR_ERROR = -1 };

	internal sealed class MonoIO
	{
		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		public static extern bool Flush(IntPtr handle, out MonoIOError error);
	}
}
#endregion

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
		
		private static HardFlushDelegate HardFlush = SelectHardFlushImpl();

		public CommandLogWriter(NumberedFileCreator creator, BinaryFormatter formatter)
		{
			_fileCreator = creator;
			_formatter = formatter;
		}

		/// <summary>
		/// Prevalence base folder.
		/// </summary>
		public DirectoryInfo PrevalenceBase
		{
			get
			{
				return _fileCreator.PrevalenceBase;
			}
		}

		/// <summary>
		/// Writes a command to the current log file.
		/// </summary>
		/// <param name="command">serializable command</param>
		public void WriteCommand(ICommand command)
		{
			/*
			CheckOutputLog();
			
			long current = _output.Position;
			try
			{
				_formatter.Serialize(_output, command);
				Flush(_output);
			}
			catch (Exception)
			{
				_output.SetLength(current);
				throw;
			}
			*/
			
			// New strategy for dealing with out of space errors,
			// sometimes the _output.SetLength above would fail
			// leaving the file corrupted.
			// This new strategy fixes the problem by always
			// growing the file before writing anything to it.
			
			MemoryStream stream = new MemoryStream();
			_formatter.Serialize(stream, command);
			byte[] bytes = stream.ToArray();
			
			CheckOutputLog();
			_output.SetLength(_output.Position + bytes.Length);
			_output.Write(bytes, 0, bytes.Length);
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
					BufferedStream bstream = new BufferedStream(stream);
					_formatter.Serialize(bstream, system);
					bstream.Flush();
					Flush(stream);				
				}
				tempFile.MoveTo(snapshotFile.FullName);
			}
			catch
			{
				// TODO: log errors...
				try
				{
					tempFile.Delete();
				}
				catch
				{
				}
				
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
			if (Bamboo.Prevalence.Configuration.PrevalenceSettings.FlushAfterCommand)
			{				
				HardFlush(stream);
			}
		}

		private System.IO.FileStream NextOutputLog()
		{				
			// FileShare.Read: don't prevent anyone from 
			// reading the log
			return _fileCreator.NewOutputLog().Open(
				FileMode.CreateNew, FileAccess.Write, FileShare.Read
				);
		}
		
#region Mono support

		delegate void HardFlushDelegate(System.IO.FileStream stream);
		
		private static HardFlushDelegate SelectHardFlushImpl()
		{
			// mono linux is 128
			if (128 == (int)Environment.OSVersion.Platform)
			{
				return new HardFlushDelegate(MonoHardFlush);
			}
			else
			{
				return new HardFlushDelegate(Win32HardFlush);
			}
		}

		[System.Security.SuppressUnmanagedCodeSecurity]
		private static void MonoHardFlush(System.IO.FileStream stream)
		{
			System.IO.MonoIOError result = System.IO.MonoIOError.ERROR_SUCCESS;
			System.IO.MonoIO.Flush(stream.Handle, out result);
			if (result != System.IO.MonoIOError.ERROR_SUCCESS)
			{				
				throw new System.IO.IOException(string.Format("Flush call failed with error {0}.", result));
			}			
		}

		[System.Security.SuppressUnmanagedCodeSecurity] // optimization...
		private static void Win32HardFlush(System.IO.FileStream stream)
		{
			if (0 == FlushFileBuffers(stream.Handle))
			{
				Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
			}
		}
		
		[DllImport("KERNEL32.DLL", EntryPoint="FlushFileBuffers", PreserveSig=true, CallingConvention=CallingConvention.Winapi, SetLastError=true)]
		private static extern int FlushFileBuffers(IntPtr handle);
#endregion
	}
}
