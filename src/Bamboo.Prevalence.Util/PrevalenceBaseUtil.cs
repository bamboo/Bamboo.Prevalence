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
