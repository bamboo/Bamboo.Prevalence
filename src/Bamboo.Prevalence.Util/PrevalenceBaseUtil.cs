#region License
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
#endregion

using System;
using System.IO;
using Bamboo.Prevalence;
namespace Bamboo.Prevalence.Util
{
	/// <summary>
	/// Summary description for PrevalenceBaseUtil.
	/// </summary>
	public class PrevalenceBaseUtil
	{
		/// <summary>
		/// Returns a list with all the files in the PrevalenceBase
		/// folder sorted by name.
		/// </summary>
		/// <param name="engine">the prevalence engine</param>
		/// <returns>list of files sorted by name</returns>
		public static FileInfo[] GetPrevalenceFilesSortedByName(PrevalenceEngine engine)
		{
			DirectoryInfo prevalenceBase = engine.PrevalenceBase;			
			FileInfo[] files = prevalenceBase.GetFiles("*.*");
			SortFilesByName(files);
			return files;
		}

		/// <summary>
		/// Returns a list with all the files that are no
		/// longer necessary to restore the state of
		/// the prevalence system.
		/// </summary>
		/// <param name="engine">the prevalence engine</param>
		/// <returns>list with files sorted by name</returns>
		public static FileInfo[] GetUnnecessaryPrevalenceFiles(PrevalenceEngine engine)
		{
			FileInfo[] all = GetPrevalenceFilesSortedByName(engine);			
			int lastSnapshotIndex = FindLastSnapshot(all);
			if (lastSnapshotIndex > 0)
			{	
				return GetFileInfoRange(all, 0, lastSnapshotIndex);				
			}
			else
			{
				return NullCleanUpPolicy.EmptyFileInfoArray;
			}
		}

		/// <summary>
		/// Creates a new array of count objects from
		/// files starting at index.
		/// </summary>
		/// <param name="files">source array</param>
		/// <param name="index">first index to copy</param>
		/// <param name="count">items to be copied</param>
		/// <returns>a new array with the specified elements</returns>
		public static FileInfo[] GetFileInfoRange(FileInfo[] files, int index, int count)
		{			
			FileInfo[] range = new FileInfo[count];
			Array.Copy(files, index, range, 0, count);
			return range;
		}

		/// <summary>
		/// Sorts the array by FileInfo.Name.
		/// </summary>
		/// <param name="files">array to be sorted in place</param>
		public static void SortFilesByName(FileInfo[] files)
		{			
			Array.Sort(files, Bamboo.Prevalence.Implementation.FileNameComparer.Default);
		}

		/// <summary>
		/// Returns the index of the last snapshot file in
		/// files.
		/// </summary>
		/// <param name="files"></param>
		/// <returns></returns>
		public static int FindLastSnapshot(FileInfo[] files)
		{
			for (int i=files.Length-1; i>-1; --i)
			{
				if (0 == String.Compare(files[i].Extension, ".snapshot", true))
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>
		/// Returns all snapshot files in the <see cref="PrevalenceEngine.PrevalenceBase"/>
		/// folder sorted by filename.
		/// </summary>
		/// <param name="engine">the prevalence engine</param>
		/// <returns></returns>
		public static FileInfo[] GetSnapshotFiles(PrevalenceEngine engine)
		{
			FileInfo[] files = engine.PrevalenceBase.GetFiles("*.snapshot");
			SortFilesByName(files);
			return files;
		}

		/// <summary>
		/// Returns the last (most recent) snapshot file in the <see cref="PrevalenceEngine.PrevalenceBase"/>
		/// folder or null when no snapshot exists.
		/// </summary>
		/// <param name="engine">the prevalence engine</param>
		/// <returns></returns>
		public static FileInfo FindLastSnapshot(PrevalenceEngine engine)
		{
			FileInfo[] snapshots = GetSnapshotFiles(engine);
			if (snapshots.Length > 0)
			{
				return snapshots[snapshots.Length-1];
			}
			return null;
		}
	}
}
