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
