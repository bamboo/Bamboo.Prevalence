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
	/// Removes all files that are no longer necessary for 
	/// the prevalent system to recover.
	/// </summary>
	public class CleanUpAllFilesPolicy : ICleanUpPolicy
	{
		/// <summary>
		/// The one and only CleanUpAllFilesPolicy instance.
		/// </summary>
		public static readonly ICleanUpPolicy Default = new CleanUpAllFilesPolicy();

		private CleanUpAllFilesPolicy()
		{		
		}

		FileInfo[] ICleanUpPolicy.SelectFiles(PrevalenceEngine engine)
		{
			DirectoryInfo prevalenceBase = engine.PrevalenceBase;			
			FileInfo[] all = prevalenceBase.GetFiles("*.*");
			SortFilesByName(all);
			int lastSnapshotIndex = FindLastSnapshot(all);
			if (lastSnapshotIndex > 0)
			{				
				FileInfo[] files = new FileInfo[lastSnapshotIndex];
				Array.Copy(all, files, lastSnapshotIndex);
				return files;
			}
			else
			{
				return NullCleanUpPolicy.EmptyFileInfoArray;
			}
		}

		void SortFilesByName(FileInfo[] files)
		{			
			Array.Sort(files, Bamboo.Prevalence.Implementation.FileNameComparer.Default);
		}

		int FindLastSnapshot(FileInfo[] files)
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
	}
}
