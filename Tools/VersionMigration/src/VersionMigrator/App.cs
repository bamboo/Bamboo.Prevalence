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
