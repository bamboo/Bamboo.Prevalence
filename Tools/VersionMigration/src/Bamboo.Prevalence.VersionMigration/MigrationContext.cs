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
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Bamboo.Prevalence.VersionMigration
{
	public class AfterDeserializationEventArgs : System.EventArgs
	{
		protected object _object;

		public AfterDeserializationEventArgs(object object_)
		{
			_object = object_;
		}

		public object Object
		{
			get
			{
				return _object;
			}
		}
	}

	/// <summary>
	/// MigrationContext.
	/// </summary>
	public class MigrationContext
	{
		private Assembly _targetAssembly;

		private MigrationPlan _plan;

		private Stack _objects;

		private Stack _serializationInfo;

		private Stack _fields;

		private Hashtable _serializableFieldsCache;

		private MigrationProject _project;

		public event ResolveEventHandler ResolveAssembly;

		public event EventHandler AfterDeserialization;

		public MigrationContext(MigrationProject project)
		{
			if (null == project)
			{
				throw new ArgumentNullException("project");
			}

			_project = project;			
			_objects = new Stack();
			_serializationInfo = new Stack();
			_fields = new Stack();
			_serializableFieldsCache = new Hashtable();
		}

		public Assembly TargetAssembly
		{
			get
			{
				return _targetAssembly;
			}
		}

		public bool OverwriteTargetFile
		{
			get
			{
				return _project.OverwriteTargetFile;
			}
		}

		public string SourceFile
		{
			get
			{
				return _project.SourceFile;
			}
		}
		
		public MigrationPlan MigrationPlan
		{
			get
			{
				return _plan;
			}
		}

		public object CurrentObject
		{
			get
			{
				return _objects.Peek();
			}
		}

		public SerializationInfo CurrentSerializationInfo
		{
			get
			{
				return _serializationInfo.Peek() as SerializationInfo;
			}
		}

		public FieldInfo CurrentField
		{
			get
			{
				return _fields.Peek() as FieldInfo;
			}
		}

		public bool HasInitializers(Type type)
		{
			TypeMapping mapping = GetTypeMapping(type);
			if (null != mapping)
			{
				return mapping.Initializer != null || mapping.FieldMappings.Count > 0;
			}
			return false;
		}

		public TypeMapping GetTypeMapping(Type type)
		{
			return _plan.GetTypeMapping(type);
		}

		public FieldInfo[] GetSerializableFields(Type type)
		{
			FieldInfo[] fields = (FieldInfo[])_serializableFieldsCache[type];
			if (null == fields)
			{
				fields = BuildSerializableFieldsArray(type);
				_serializableFieldsCache[type] = fields;
			}
			return fields;
		}

		public void Migrate()
		{
			_project.Validate();
			
			InstallAssemblyResolver();
			try
			{
				LoadMigrationPlan();
				LoadMainAssembly();
				CompileAndInstallScriptEventHandlers();

				object graph = ReadObject();
				WriteObject(graph);
			}
			finally
			{
				UninstallAssemblyResolver();
			}
		}		

		/// <summary>
		/// Same as <see cref="InitializerHelper.SetField"/>(<see cref="CurrentObject"/>, name, value).
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void SetCurrentObjectField(string name, object value)
		{
			InitializerHelper.SetField(CurrentObject, name, value);
		}		

		public object ChangeType(object value, Type conversionType)
		{
			if (conversionType.IsEnum)
			{
				if (value is string)
				{
					return Enum.Parse(conversionType, (string)value);
				}
			}
			return Convert.ChangeType(value, conversionType, _plan.Culture);
		}

		internal void EnterObject(object obj, SerializationInfo info)
		{
			Trace("Migrating {0}...", obj.GetType().Name);

			_objects.Push(obj);
			_serializationInfo.Push(info);
		}

		internal void LeaveObject()
		{
			_objects.Pop();
			_serializationInfo.Pop();			
		}

		internal void EnterField(FieldInfo field)
		{
			Trace("Migrating field {0}...", field.Name);
			_fields.Push(field);
		}

		internal void LeaveField()
		{
			_fields.Pop();
		}

		private object ReadObject()
		{
			object returnValue = _plan.Deserialize(this);
			if (null != AfterDeserialization)
			{
				AfterDeserialization(this, new AfterDeserializationEventArgs(returnValue));
			}
			return returnValue;
		}

		private void WriteObject(object graph)
		{
			// thanks to Iulian Dan Repolschi for reminding me
			// how important it is to close a file :-)
			using (FileStream stream = CreateTargetFile())
			{
				new BinaryFormatter().Serialize(stream, graph);
			}
		}

		private FileStream CreateTargetFile()
		{
			CreateDirectoryIfNeeded(Path.GetDirectoryName(_project.TargetFile));
			return new FileStream(_project.TargetFile, GetCreationFileMode(),
									FileAccess.Write, FileShare.None);
		}

		private FileMode GetCreationFileMode()
		{
			return _project.OverwriteTargetFile ? FileMode.Create : FileMode.CreateNew;
		}

		private void CreateDirectoryIfNeeded(string directory)
		{			
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}
		}
		
		private FieldInfo[] BuildSerializableFieldsArray(Type type)
		{
			MemberInfo[] members = type.FindMembers(MemberTypes.Field,
				BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic,
				null, null);
			
			ArrayList fields = new ArrayList();
			foreach (FieldInfo field in members)
			{
				if (field.IsNotSerialized)
				{
					continue;
				}
				fields.Add(field);
			}
			return (FieldInfo[])fields.ToArray(typeof(FieldInfo));
		}	

		private void InstallAssemblyResolver()
		{
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(HandleResolveAssembly);		
		}

		private Assembly HandleResolveAssembly(object sender, ResolveEventArgs e)
		{			
			Assembly resolved = null;

			if (_targetAssembly != null)
			{
				if (e.Name.StartsWith(_targetAssembly.GetName().Name))
				{
					resolved = _targetAssembly;
				}
			}

			if (resolved == null)
			{
				string assemblyFile = _project.ResolveAssembly(e.Name.Split(',')[0]);
				if (null != assemblyFile)
				{
					resolved = Assembly.LoadFrom(assemblyFile);
				}
				
				if (resolved == null && null != ResolveAssembly)
				{
					return ResolveAssembly(sender, e);
				}
			}

			return resolved;
		}

		private void LoadMigrationPlan()
		{
			_plan = MigrationPlan.Load(_project.MigrationPlan);
		}

		private void LoadMainAssembly()
		{
			_targetAssembly = Assembly.LoadFrom(_project.MainAssembly);
		}

		private void UninstallAssemblyResolver()
		{
			AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(HandleResolveAssembly);
		}
		
		void CompileAndInstallScriptEventHandlers()
		{
			_plan.SetUpScripts(this);
		}

		public void Trace(string format, params object[] args)
		{
			Console.WriteLine(format, args);
		}
	}
}
