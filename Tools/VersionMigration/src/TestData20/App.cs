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
