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
