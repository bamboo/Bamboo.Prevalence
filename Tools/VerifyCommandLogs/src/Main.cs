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
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

class App
{
	public static void Main(string[] args)
	{
		if (2 != args.Length)
		{
			Console.WriteLine("VerifyCommandLogs <systemFolder> <assembly folder>");
			return;
		}
		
		string systemFolder = args[0];
		string assemblyFolder = args[1];
		new App(systemFolder, assemblyFolder).Run();		
	}
	
	string _systemFolder;
	string _assemblyFolder;
	
	public App(string systemFolder, string assemblyFolder)
	{
		_systemFolder = systemFolder;
		_assemblyFolder = assemblyFolder;
	}
	
	public void Run()
	{		
		InstallAssemblyResolver();
		try
		{
			foreach (string fname in Directory.GetFiles(_systemFolder, "*.commandlog"))
			{
				Verify(fname);
			}
		}
		finally
		{
			RemoveAssemblyResolver();
		}
	}
	
	void Verify(string fname)
	{		
		Trace("Checking {0}...", fname);
		using (FileStream stream = File.OpenRead(fname))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			while (stream.Position < stream.Length)
			{
				long position = stream.Position;
				
				try
				{
					object command = formatter.Deserialize(stream);
					if (null != command)
					{
						Trace("{0} command successfully deserialized.", command);
					}
				}
				catch (Exception x)
				{
					Trace("{0}:{1}: {2}", fname, position, x);
					break;
				}
			}
		}
	}
	
	Assembly AppDomain_AssemblyResolve(object sender, ResolveEventArgs e)
	{
		Trace("Resolving assembly {0}...", e.Name);
		
		string name = e.Name.Split(',')[0];
		string path = Path.Combine(_assemblyFolder, name + ".dll");
		Trace("Loading {0}...", path);
		return Assembly.LoadFrom(path);
	}
	
	void InstallAssemblyResolver()
	{
		AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AppDomain_AssemblyResolve);
	}
	
	void RemoveAssemblyResolver()
	{
		AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(AppDomain_AssemblyResolve);
	}
	
	void Trace(string format, params object[] args)
	{
		Console.WriteLine(format, args);
	}
}
