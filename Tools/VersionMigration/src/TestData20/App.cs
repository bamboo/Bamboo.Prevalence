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
using System.Collections;
using System.Globalization;
using System.IO;
using System.Xml;
using NUnit.Framework;
using Bamboo.Prevalence;
using SamplePrevalentSystem;

namespace TestData20
{
	/// <summary>
	/// 
	/// </summary>
	class App : Assertion
	{
		private static readonly CultureInfo PortugueseCulture = CultureInfo.CreateSpecificCulture("pt");

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			string prevalenceBase = Path.Combine(Environment.CurrentDirectory, "data");			
			PrevalenceEngine engine = PrevalenceActivator.CreateEngine(typeof(LibrarySystem), prevalenceBase);
			
			LibrarySystem library = engine.PrevalentSystem as LibrarySystem;
			IList titles = library.GetTitles();

			XmlDocument data = new XmlDocument();
			data.Load("titles.xml");

			XmlNodeList expectedTitles = data.SelectNodes("//title");

			AssertEquals("titles.Count", expectedTitles.Count, titles.Count);

			foreach (XmlElement expected in expectedTitles)
			{
				Title title = FindByName(titles, expected.GetAttribute("name") + "*");				
				AssertTitle(expected, title);
			}
		}

		private static Title FindByName(IList titles, string name)
		{			
			foreach (Title title in titles)
			{
				if (title.Name.Equals(name))
				{
					return title;
				}
			}
			Assert("Expected title: " + name, false);
			return null;
		}

		private static void AssertTitle(XmlElement expected, Title actual)
		{	
			// the script code will add an * to every name...			
			AssertEquals("title.Name", expected.GetAttribute("name") + "*", actual.Name);
			
			AssertEquals("title.Description", expected.GetAttribute("summary"), actual.Description);
			AssertEquals("title.PublishDate", ToDateTime(expected.GetAttribute("publishDate")), actual.PublishDate);
			AssertEquals("title.Reviews.Count", 0, actual.Reviews.Count);
		}

		private static DateTime ToDateTime(string date)
		{
			return DateTime.Parse(date, PortugueseCulture);
		}
	}
}
