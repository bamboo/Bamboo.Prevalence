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
using System.Reflection;
using Bamboo.Prevalence.VersionMigration;

namespace VersionMigrator
{
	/// <summary>
	/// VersionMigrator application.
	/// </summary>
	class App
	{
		[STAThread]
		static int Main(string[] args)
		{				
			try
			{
				MigrationProject project = ParseCommandLine(args);
				WriteLine("Bamboo.Prevalence Version Migrator 1.0");
				MigrationContext context = new MigrationContext(project);			
				context.Migrate();			
			}
			catch (Exception x)
			{
				WriteLine(x.Message);
				return -1;
			}
			
			return 0;
		}
		
		static MigrationProject ParseCommandLine(string[] args)
		{
			if (args.Length < 1)
			{
				CommandLineError();
			}
			
			MigrationProject project = MigrationProject.Load(args[0]);
			for (int i=1; i<args.Length; ++i)
			{
				string arg = args[i];
				string option = arg.Substring(0, 3);
				switch (option)
				{
					case "-s:":
					{
						project.SourceFile = arg.Substring(3);
						break;
					}
					
					case "-t:":
					{
						project.TargetFile = arg.Substring(3);
						break;
					}
					
					default:
					{
						CommandLineError();
						break;
					}
				}
			}
			return project;
		}
		
		static void CommandLineError()
		{
			throw new ApplicationException("VersionMigrator <MigrationProject.xml> [-s:<SourceFile>] [-t:<TargetFile>]");
		}
		
		static void Write(string text)
		{
			Console.Write(text);
		}

		static void WriteLine(string text)
		{
			Console.WriteLine(text);
		}
		
	}
}
