// Bamboo.Prevalence - a .NET object prevalence engine
// Copyright (C) 2002 Rodrigo B. de Oliveira (rodrigobamboo@hotmail.com)
//
// Based upon the original concept and implementation
// by Klaus Wuestefeld.
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

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Bamboo.Prevalence.Implementation
{
	/// <summary>
	/// Command log reader.
	/// </summary>
	internal class CommandLogReader : System.Collections.IEnumerable
	{
		private NumberedFileFinder _fileFinder;	

		private BinaryFormatter _formatter;

		public CommandLogReader(DirectoryInfo prevalenceBase)
		{
			_fileFinder = new NumberedFileFinder(prevalenceBase);
			_formatter = new BinaryFormatter();
		}	

		public Bamboo.Prevalence.IPrevalentSystem ReadLastSnapshot()
		{
			FileInfo snapshot = _fileFinder.LastSnapshot;
			if (null != snapshot)
			{
				using (FileStream stream = snapshot.OpenRead())
				{
					return _formatter.Deserialize(stream) as Bamboo.Prevalence.IPrevalentSystem;
				}
			}
			return null;
		}	

		internal CommandLogWriter ToWriter()
		{
			return new CommandLogWriter(_fileFinder.ToFileCreator(), _formatter);
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return new PendingCommandsEnumerator(_fileFinder, _formatter);
		}
	}
}
