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

namespace Bamboo.Prevalence.Util
{
	/// <summary>
	/// Removes unnecessary files older than <see cref="MaxAge"/>.
	/// </summary>
	public class OldFilesCleanUpPolicy : AbstractCleanUpPolicy
	{
		TimeSpan _maxAge;

		/// <summary>
		/// Creates a new policy to remove unnecessary files older
		/// than maxAge.
		/// </summary>
		/// <param name="maxAge">maximum age for files</param>
		public OldFilesCleanUpPolicy(TimeSpan maxAge)
		{
			_maxAge = maxAge;
		}

		/// <summary>
		/// Maximum age for files
		/// </summary>
		public TimeSpan MaxAge
		{
			get
			{
				return _maxAge;
			}
		}

		/// <summary>
		/// Returns a list with all unnecessary files older
		/// than <see cref="MaxAge"/>.
		/// </summary>
		/// <param name="engine">the prevalence engine</param>
		/// <returns></returns>
		public override System.IO.FileInfo[] SelectFiles(Bamboo.Prevalence.PrevalenceEngine engine)
		{
			FileInfo[] unnecessary = GetUnnecessaryPrevalenceFiles(engine);
			int index = FindFirstFileOlderThanPeriod(unnecessary);
			if (index > 0)
			{
				return GetFileInfoRange(unnecessary, 0, index+1);
			}
			return NullCleanUpPolicy.EmptyFileInfoArray;
		}

		int FindFirstFileOlderThanPeriod(FileInfo[] files)
		{
			DateTime cutoffDate = DateTime.Now - _maxAge;
			for (int i=files.Length-1; i>-1; --i)
			{
				FileInfo fi = files[i];
				if (fi.LastWriteTime <= cutoffDate)
				{
					return i;
				}
			}
			return -1;
		}
	}
}
