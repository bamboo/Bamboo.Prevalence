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
