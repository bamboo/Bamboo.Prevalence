#region License
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
#endregion

using System;
using System.Collections.Specialized;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Bamboo.Prevalence.VersionMigration
{
	/// <summary>
	/// Stores all the information necessary
	/// to migrate an object schema.	
	/// </summary>	
	/// <remarks>Relative filenames are always resolved
	/// against <see cref="FileName"/> prior to
	/// returning</remarks>
	public class MigrationProject
	{
		string _migrationPlan;

		string _mainAssembly;

		string _sourceFile;

		string _targetFile;

		bool _overwriteTargetFile;
		
		bool _isDirty;
		
		string _fileName;

		StringCollection _searchPath;

		/// <summary>
		/// Create an empty MigrationProject.
		/// </summary>
		public MigrationProject()
		{
			_fileName = string.Empty;
			_isDirty = false;
			_searchPath = new StringCollection();
		}

		/// <summary>
		/// Assembly search path.
		/// </summary>
		public StringCollection SearchPath
		{
			get
			{
				return _searchPath;
			}
		}

		/// <summary>
		/// Signals if the object has changed
		/// since it was loaded.
		/// </summary>
		[XmlIgnore]
		public bool IsDirty
		{
			get
			{
				return _isDirty;
			}

			set
			{
				_isDirty = value;
			}
		}

		/// <summary>
		/// Name of the file that object was loaded
		/// from.
		/// </summary>
		[XmlIgnore]
		public string FileName
		{
			get
			{
				return _fileName;
			}
		}

		/// <summary>
		/// Migration plan's path.
		/// </summary>
		public string MigrationPlan
		{
			get
			{
				return ResolvePath(_migrationPlan);
			}

			set
			{
				if (value != _migrationPlan)
				{
					_isDirty = true;
					_migrationPlan = value;
				}
			}
		}

		/// <summary>
		/// Main assembly's path.
		/// </summary>
		public string MainAssembly
		{
			get
			{
				return ResolvePath(_mainAssembly);
			}

			set
			{
				if (value != _mainAssembly)
				{
					_isDirty = true;
					_mainAssembly = value;
				}
			}
		}

		/// <summary>
		/// Path to the serialized object graph.
		/// </summary>
		public string SourceFile
		{
			get
			{
				return ResolvePath(_sourceFile);
			}

			set
			{
				if (value != _sourceFile)
				{
					_isDirty = true;
					_sourceFile = value;
				}
			}
		}

		/// <summary>
		/// Path of the resulting serialized
		/// object graph.
		/// </summary>
		public string TargetFile
		{
			get
			{
				return ResolvePath(_targetFile);
			}

			set
			{
				if (value != _targetFile)
				{
					_isDirty = true;
					_targetFile = value;
				}
			}
		}

		/// <summary>
		/// Should the target file be overwritten?
		/// </summary>
		public bool OverwriteTargetFile
		{
			get
			{
				return _overwriteTargetFile;
			}

			set
			{
				if (value != _overwriteTargetFile)
				{
					_isDirty = true;
					_overwriteTargetFile = value;
				}
			}
		}

		/// <summary>
		/// Check if all the necessary properties
		/// have been set (<see cref="MigrationPlan" />,
		/// <see cref="MainAssembly" />,
		/// <see cref="SourceFile" /> and <see cref="TargetFile" />)
		/// </summary>
		public void Validate()
		{
			AssertFieldIsSet("MigrationPlan", _migrationPlan);
			AssertFieldIsSet("MainAssembly", _mainAssembly);
			AssertFieldIsSet("SourceFile", _sourceFile);
			AssertFieldIsSet("TargetFile", _targetFile);
		}
		
		/// <summary>
		/// Save a xml representation of this object to
		/// a file.
		/// </summary>
		/// <param name="filename">path to the file this
		/// object should be xmlserialized to</param>
		public void Save(string filename)
		{
			using (FileStream stream = new FileStream(filename, FileMode.Create, FileAccess.Write))
			{
				CreateSerializer().Serialize(stream, this);
				stream.Flush();
				_fileName = filename;
				_isDirty = false;
			}
		}

		/// <summary>
		/// Call <see cref="Save"/> passing <see cref="FileName"/>
		/// as the argument.
		/// </summary>
		public void Save()
		{
			Save(_fileName);
		}

		/// <summary>
		/// Load a MigrationProject previously saved
		/// with <see cref="Save"/>.
		/// </summary>
		/// <param name="filename">path to the serialized
		/// MigrationProject</param>
		/// <returns>a MigrationProject</returns>
		public static MigrationProject Load(string filename)
		{
			using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
			{
				MigrationProject project = (MigrationProject)CreateSerializer().Deserialize(stream);
				project._fileName = filename;
				project._isDirty = false;
				if (null != project._mainAssembly || 0 != project._mainAssembly.Length)
				{
					project.SearchPath.Add(Path.GetDirectoryName(project._mainAssembly));
				}
				return project;
			}
		}

		public string ResolveAssembly(string name)
		{			
			foreach (string path in SearchPath)
			{
				string folder = ResolvePath(path);

				string fname = Path.Combine(folder, name + ".dll");
				if (File.Exists(fname))
				{
					return fname;
				}

				fname = Path.Combine(folder, name + ".exe");
				if (File.Exists(fname))
				{
					return fname;
				}
			}
			return null;
		}

		string ResolvePath(string fname)
		{
			if (string.Empty != _fileName)
			{
				if (!Path.IsPathRooted(fname))
				{
					return Path.Combine(Path.GetDirectoryName(_fileName), fname);
				}
			}
			return fname;
		}

		static XmlSerializer CreateSerializer()
		{
			return new XmlSerializer(typeof(MigrationProject));
		}

		void AssertFieldIsSet(string name, string value)
		{
			if (null == value || 0 == value.Length)
			{
				throw new ApplicationException(string.Format("The field {0} must be set!", name));
			}
		}
	}
}
