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
using System.Timers;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using Bamboo.Prevalence;

namespace RemotingPrevalence
{
	/// <summary>	
	/// </summary>
	public class Server
	{		
		public static void Main(string[] args)
		{				
			ChannelServices.RegisterChannel(new TcpChannel(8080));
			
			PrevalenceEngine engine = PrevalenceActivator.CreateTransparentEngine(typeof(AddressBook), Path.Combine(Environment.CurrentDirectory, "data"));
			AddressBook book = engine.PrevalentSystem as AddressBook;

			// Let's take a complete snapshot of the system
			// each 30 seconds...
			SnapshotTaker st = new SnapshotTaker(engine, 30000);

			ObjRef reference = RemotingServices.Marshal(book, "AddressBook", typeof(AddressBook));
			Console.WriteLine("server running... press <ENTER> to finish");			
			Console.ReadLine();		
	
			RemotingServices.Unmarshal(reference);
		}
	}

	class SnapshotTaker
	{
		PrevalenceEngine _engine;
		Timer _timer;

		public SnapshotTaker(PrevalenceEngine engine, double interval)
		{			
			_engine = engine;
			_timer = new Timer(interval);
			_timer.AutoReset = true;
			_timer.Elapsed += new ElapsedEventHandler(Elapsed);
			_timer.Start();
		}

		private void Elapsed(object sender, ElapsedEventArgs args)
		{
			Console.Write("Taking system snapshot... ");
			_engine.TakeSnapshot();
			Console.WriteLine("Done.");
		}
	}
}
