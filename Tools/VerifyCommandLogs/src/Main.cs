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
