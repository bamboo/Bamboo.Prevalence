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
