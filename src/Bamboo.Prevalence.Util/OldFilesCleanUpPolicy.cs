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
