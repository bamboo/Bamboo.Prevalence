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

namespace PetStoreWeb.Components
{
	using System;
	using System.Collections;
	using System.Configuration;
	using System.ComponentModel;
	using System.IO;
	using System.Threading;
	using System.Web;
	using System.Web.SessionState;
	using Bamboo.Prevalence;

	public class Prevayler : HttpApplication
	{
		protected static PrevalenceEngine engine;
		protected static Timer timer;

		//public Prevayler()
		//{
		//}	

		public static object Execute(ICommand command)
		{
			return engine.ExecuteCommand(command);
		}

		public static object system()
		{
			return engine.PrevalentSystem;
		}
		
		protected void Application_Start(Object sender, EventArgs e)
		{
			string datapath = ConfigurationSettings.AppSettings["DataPath"];
			long counter = Convert.ToInt64(ConfigurationSettings.AppSettings["DataSnapshot"]);
			long delay = 0;

			engine = PrevalenceActivator.CreateEngine(typeof(PetStore), datapath);
			timer = new Timer(new TimerCallback(TakeSnapshot), null, delay, counter);
		}
 
		protected void Application_End(Object sender, EventArgs e)
		{
			engine.TakeSnapshot();
			timer.Dispose();
		}

		private void TakeSnapshot(object state)
		{
			try { engine.TakeSnapshot(); }
			catch { }
			//_log.WriteEntry(x.ToString(), EventLogEntryType.Error);
		}
	}
}

