// project created on 11/21/2003 at 9:47 AM
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
