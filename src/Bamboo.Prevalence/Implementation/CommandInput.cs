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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Bamboo.Prevalence.Implementation
{
	/// <summary>
	/// Command log reader.
	/// </summary>
	internal sealed class CommandLogReader : System.Collections.IEnumerable
	{
		private NumberedFileFinder _fileFinder;	

		private BinaryFormatter _formatter;

		public CommandLogReader(DirectoryInfo prevalenceBase, BinaryFormatter formatter)
		{
			_fileFinder = new NumberedFileFinder(prevalenceBase);
			_formatter = formatter;
		}	

		public object ReadLastSnapshot()
		{
			FileInfo snapshot = _fileFinder.LastSnapshot;
			if (null != snapshot)
			{
				using (FileStream stream = snapshot.OpenRead())
				{
					const int BufferSize = 512*1024;
					return _formatter.Deserialize(new BufferedStream(stream, BufferSize));
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
