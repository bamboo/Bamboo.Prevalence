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
	/// Models the strategy for naming files in 
	/// a prevalence base.
	/// </summary>
	public sealed class NumberedFileFinder : NumberedFileBase
	{		
		private FileInfo _lastSnapshot;

		/// <summary>
		/// Creates a new name strategy for the directory prevalenceBase.
		/// </summary>
		/// <param name="prevalenceBase">directory where the files will be stored</param>
		public NumberedFileFinder(DirectoryInfo prevalenceBase) :
			base(prevalenceBase, 0)
		{			
			FindLastSnapshot();
			_nextNumber = _lastSnapshot == null ? 0 : NumberFromFileInfo(_lastSnapshot);
		}	
	
		/// <summary>
		/// The last snapshot file.
		/// </summary>
		public FileInfo LastSnapshot
		{
			get
			{
				return _lastSnapshot;
			}
		}

		internal FileInfo NextPendingLog()
		{
			FileInfo pendingLog = FormatFileInfo(LogFileNameFormat, _nextNumber + 1);
			if (pendingLog.Exists)
			{
				++_nextNumber;				
			}
			return pendingLog;
		}

		internal NumberedFileCreator ToFileCreator()
		{
			return new NumberedFileCreator(_prevalenceBase, _nextNumber + 1);
		}

		private void FindLastSnapshot()
		{
			System.IO.FileInfo[] files = _prevalenceBase.GetFiles("*.snapshot");			
			if (files.Length > 0)
			{
				Array.Sort(files, FileNameComparer.Default);
				_lastSnapshot = files[files.Length-1];
			}
		}
	}

	/// <summary>
	/// Compares System.IO.FileInfo objects by name.
	/// </summary>
	public class FileNameComparer : System.Collections.IComparer
	{	
		/// <summary>
		/// The one and only FileNameComparer instance.
		/// </summary>
		public static readonly System.Collections.IComparer Default = new FileNameComparer();

		private FileNameComparer()
		{
		}

		#region Implementation of IComparer
		int System.Collections.IComparer.Compare(object lhs, object rhs)
		{
			// We know lhs and rhs can never be null
			FileInfo lhsFileInfo = (FileInfo)lhs;
			FileInfo rhsFileInfo = (FileInfo)rhs;
			return lhsFileInfo.Name.CompareTo(rhsFileInfo.Name);
		}	
		#endregion
	}
}
