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
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Bamboo.Prevalence.VersionMigration
{
	/// <summary>
	/// MigrationContext.
	/// </summary>
	public class MigrationContext
	{
		private Assembly _targetAssembly;

		private string _from;

		private string _to;

		private bool _overwriteFiles;

		private MigrationPlan _plan;

		private Stack _objects;

		private Stack _serializationInfo;

		private Stack _fields;

		private Hashtable _serializableFieldsCache;		

		public MigrationContext(MigrationPlan plan)
		{
			if (null == plan)
			{
				throw new ArgumentNullException("plan");
			}

			_plan = plan;
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

			set
			{
				_targetAssembly = value;
			}
		}

		public string From
		{
			get
			{
				return _from;
			}

			set
			{
				_from = value;
			}
		}

		public string To
		{
			get
			{
				return _to;
			}

			set
			{
				_to = value;
			}
		}

		public bool OverwriteFiles
		{
			get
			{
				return _overwriteFiles;
			}

			set
			{
				_overwriteFiles = value;
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

		public bool IsTypeMappingAvailable(Type type)
		{
			return _plan.IsTypeMappingAvailable(type);
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
			CheckProperties();

			InstallAssemblyResolver();
			try
			{
				object graph = ReadObject();
				WriteObject(graph);
			}
			finally
			{
				UninstallAssemblyResolver();
			}
		}		

		public object ChangeType(object value, Type conversionType)
		{
			return Convert.ChangeType(value, conversionType, _plan.Culture);
		}

		internal void EnterObject(object obj, SerializationInfo info)
		{
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
			_fields.Push(field);
		}

		internal void LeaveField()
		{
			_fields.Pop();
		}

		private object ReadObject()
		{
			return _plan.Deserialize(this);
		}

		private void WriteObject(object graph)
		{
			FileStream stream = CreateTargetFile();
			new BinaryFormatter().Serialize(stream, graph);
		}

		private FileStream CreateTargetFile()
		{
			CreateDirectoryIfNeeded(Path.GetDirectoryName(_to));
			return new FileStream(_to, GetCreationFileMode(), FileAccess.Write, FileShare.None);
		}

		private FileMode GetCreationFileMode()
		{
			return _overwriteFiles ? FileMode.Create : FileMode.CreateNew;
		}

		private void CreateDirectoryIfNeeded(string directory)
		{			
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}
		}

		private void CheckProperties()
		{			
			AssertFieldIsSet("From", _from);
			AssertFieldIsSet("To", _to);			
			AssertFieldIsSet("TargetAssembly", _targetAssembly);
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

		private void AssertFieldIsSet(string name, object value)
		{
			if (null == value)
			{
				FieldNotSetError(name);
			}
		}

		private void AssertFieldIsSet(string name, string value)
		{
			if (null == value || 0 == value.Length)
			{
				FieldNotSetError(name);
			}
		}

		private void FieldNotSetError(string name)
		{
			throw new ApplicationException("The field " + name + " must be set!");
		}

		private void InstallAssemblyResolver()
		{
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ResolveAssembly);
		}

		private Assembly ResolveAssembly(object sender, ResolveEventArgs e)
		{			
			if (e.Name.StartsWith(_targetAssembly.GetName().Name ))
			{
				return _targetAssembly;
			}
			return null;
		}

		private void UninstallAssemblyResolver()
		{
			AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(ResolveAssembly);
		}
	}
}
