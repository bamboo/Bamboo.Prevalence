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

namespace Bamboo.Prevalence.Implementation
{
	/// <summary>
	/// Base class for the file naming strategies.
	/// </summary>
	public class NumberedFileBase
	{
		/// <summary>
		/// Format specification for the names of the command log files.
		/// </summary>
		public const string LogFileNameFormat = "{0:00000000000000000000}.commandlog";

		/// <summary>
		/// Format specification for the names of the snapshot files.
		/// </summary>
		public const string SnapshotFileNameFormat = "{0:00000000000000000000}.snapshot";

		/// <summary>
		/// Directory where the files are stored.
		/// </summary>
		protected DirectoryInfo _prevalenceBase;

		/// <summary>
		/// Next logical file number.
		/// </summary>
		protected long _nextNumber;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="prevalenceBase"></param>
		/// <param name="nextNumber"></param>
		protected NumberedFileBase(DirectoryInfo prevalenceBase, long nextNumber)
		{
			_prevalenceBase = prevalenceBase;
			_nextNumber = nextNumber;
		}		

		/// <summary>
		/// Prevalence base folder.
		/// </summary>
		public DirectoryInfo PrevalenceBase
		{
			get
			{
				return _prevalenceBase;
			}
		}
	
		/// <summary>
		/// Formats a file name and returns the corresponding FileInfo
		/// object relative to _prevalenceBase.
		/// </summary>
		/// <param name="format"></param>
		/// <param name="number"></param>
		/// <returns></returns>
		protected FileInfo FormatFileInfo(string format, long number)
		{
			return new FileInfo(Path.Combine(_prevalenceBase.FullName, string.Format(format, number)));
		}		

		/// <summary>
		/// Returns the file logical number from its name.
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		protected static long NumberFromFileInfo(FileInfo info)
		{
			return long.Parse(Path.GetFileNameWithoutExtension(info.Name));
		}

		/// <summary>
		/// Formats a FileInfo with the current value
		/// of _nextNumber and increments it.
		/// </summary>
		/// <param name="format"></param>
		/// <returns></returns>
		protected FileInfo NextFileInfo(string format)
		{
			return FormatFileInfo(format, _nextNumber++);
		}
	}
}
