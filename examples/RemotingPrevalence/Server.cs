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
