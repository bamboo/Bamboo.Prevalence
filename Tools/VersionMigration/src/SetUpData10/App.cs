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
using System.Xml;
using System.IO;
using Bamboo.Prevalence;
using SamplePrevalentSystem;

namespace SetUpData10
{
	/// <summary>
	/// 
	/// </summary>
	class App
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			string prevalenceBase = Path.Combine(Environment.CurrentDirectory, "data");
			ClearPrevalenceBase(prevalenceBase);
			PrevalenceEngine engine = PrevalenceActivator.CreateEngine(typeof(Library), prevalenceBase);
			
			Library library = engine.PrevalentSystem as Library;

			Console.Write("Loading titles.xml... ");
			XmlDocument data = new XmlDocument();
			data.Load("titles.xml");
			Console.WriteLine("done!");

			foreach (XmlElement title in data.SelectNodes("//title"))
			{
				library.AddTitle(new Title(title.GetAttribute("name"), title.GetAttribute("summary")));
			}		

			Console.Write("Taking snapshot... ");
			engine.TakeSnapshot();
			Console.WriteLine("done!");

			Console.WriteLine("Version 1.0 data successfully set up!");
		}		

		private static void ClearPrevalenceBase(string prevalenceBase)
		{
			if (Directory.Exists(prevalenceBase))
			{
				Directory.Delete(prevalenceBase, true);
			}
		}
	}
}
