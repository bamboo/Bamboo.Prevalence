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
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;

namespace Bamboo.Prevalence.VersionMigration
{
	/// <summary>
	/// Models a plan for migrating
	/// a serialized object graph to a new version.
	/// </summary>
	public class MigrationPlan
	{
		public static MigrationPlan Load(string filename)
		{	
			XmlDocument document = new XmlDocument();
			document.Load(filename);			
			return new MigrationPlan(document.DocumentElement);
		}

		private TypeMappingCollection _typeMappings;

		private CultureInfo _culture;
		
		private ScriptCollection _scripts;

		public MigrationPlan()
		{
			_typeMappings = new TypeMappingCollection();			
			_scripts = new ScriptCollection();
		}

		public MigrationPlan(XmlElement planElement) : this()
		{
			Load(planElement);
		}		

		public bool IsTypeMappingAvailable(Type type)
		{
			return _typeMappings.Exists(type.FullName);
		}

		public TypeMapping GetTypeMapping(Type type)
		{
			return _typeMappings[type.FullName];
		}

		public TypeMappingCollection TypeMappings
		{
			get
			{
				return _typeMappings;
			}
		}
		
		public ScriptCollection Scripts
		{
			get
			{
				return _scripts;
			}
		}

		public CultureInfo Culture
		{
			get
			{
				return _culture;
			}
		}

		internal object Deserialize(MigrationContext context)
		{
			FileStream stream = File.OpenRead(context.SourceFile);

			BinaryFormatter formatter = new BinaryFormatter();			
			formatter.SurrogateSelector = new MigrationSurrogateSelector(context);
			formatter.Binder = new MigrationBinder(context);
			formatter.Context = new StreamingContext(StreamingContextStates.Persistence);

			return formatter.Deserialize(stream);
		}
		
		internal void SetUpScripts(MigrationContext context)
		{			
			string classNameFormat = "__script{0}__";
			for (int i=0; i<_scripts.Count; ++i)
			{
				string className = string.Format(classNameFormat, i);
				_scripts[i].SetUp(className, context);
			}			
		}

		private void Load(XmlElement planElement)
		{
			LoadCulture(planElement);
			LoadTypeMappings(planElement);
			LoadScripts(planElement);
		}

		private void LoadCulture(XmlElement element)
		{			
			string lang = element.GetAttribute("xml:lang");
			_culture = CultureInfo.CreateSpecificCulture(lang);
		}

		private void LoadTypeMappings(XmlElement planElement)
		{
			foreach (XmlElement item in planElement.SelectNodes("typeMapping"))
			{
				LoadTypeMapping(item);
			}
		}
		
		private void LoadScripts(XmlElement planElement)
		{
			foreach (XmlElement item in planElement.SelectNodes("script"))
			{
				_scripts.Add(new Script(item));
			}
		}

		private void LoadTypeMapping(XmlElement item)
		{
			TypeMapping mapping = new TypeMapping(item);
			_typeMappings.Add(mapping);
		}
	}
}
