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
	/// Summary description for NumberedFileBase.
	/// </summary>
	internal class NumberedFileBase
	{
		public const string LogFileNameFormat = "{0:00000000000000000000}.commandlog";

		public const string SnapshotFileNameFormat = "{0:00000000000000000000}.snapshot";

		protected DirectoryInfo _prevalenceBase;

		protected long _nextNumber;

		protected NumberedFileBase(DirectoryInfo prevalenceBase, long nextNumber)
		{
			_prevalenceBase = prevalenceBase;
			_nextNumber = nextNumber;
		}		
	
		protected FileInfo FormatFileInfo(string format, long number)
		{
			return new FileInfo(Path.Combine(_prevalenceBase.FullName, string.Format(format, number)));
		}		

		protected static long NumberFromFileInfo(FileInfo info)
		{
			return long.Parse(Path.GetFileNameWithoutExtension(info.Name));
		}

		protected FileInfo NextFileInfo(string format)
		{
			return FormatFileInfo(format, _nextNumber++);
		}
	}
}
