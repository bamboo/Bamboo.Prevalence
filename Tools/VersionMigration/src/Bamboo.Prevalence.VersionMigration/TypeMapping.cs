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
using System.Xml;
using Bamboo.Prevalence.VersionMigration.Initializers;

namespace Bamboo.Prevalence.VersionMigration
{
	/// <summary>
	/// Type mapping.
	/// </summary>
	public class TypeMapping
	{
		public static readonly TypeMapping Default = new TypeMapping();

		private string _typeName;

		private FieldMappingCollection _fieldMappings;

		private StringCollection _aliases;

		private string _assemblyName;

		private IObjectInitializer _initializer;

		internal TypeMapping()
		{
			_fieldMappings = new FieldMappingCollection();
			_aliases = new StringCollection();
		}

		internal TypeMapping(XmlElement element) : this()
		{
			Load(element);
		}

		public IObjectInitializer Initializer
		{
			get
			{
				return _initializer;
			}
		}

		public string TypeName
		{
			get
			{
				return _typeName;
			}
		}

		public string AssemblyName
		{
			get
			{
				return _assemblyName;
			}
		}
		
		public StringCollection Aliases
		{
			get
			{
				return _aliases;
			}
		}

		public FieldMappingCollection FieldMappings
		{
			get
			{
				return _fieldMappings;
			}
		}

		public IFieldInitializer GetFieldInitializer(string fieldName)
		{
			FieldMapping mapping = _fieldMappings[fieldName];
			if (null == mapping)
			{
				return FieldMapping.DefaultFieldInitializer;
			}
			return mapping.Initializer;
		}

		private void Load(XmlElement element)
		{
			_typeName = element.GetAttribute("type");
			_assemblyName = element.GetAttribute("assembly");
			LoadInitializer(element);
			LoadFieldMappings(element);
			LoadAliases(element);
		}

		void LoadInitializer(XmlElement element)
		{
			XmlElement initializerElement = element.SelectSingleNode("initializer") as XmlElement;
			if (null != initializerElement)
			{
				_initializer = (IObjectInitializer)InitializerHelper.LoadInitializer(initializerElement);
			}
		}

		private void LoadFieldMappings(XmlElement element)
		{
			foreach (XmlElement item in element.SelectNodes("field"))
			{
				LoadFieldMapping(item);
			}
		}

		private void LoadFieldMapping(XmlElement item)
		{
			_fieldMappings.Add(new FieldMapping(item));
		}

		private void LoadAliases(XmlElement element)
		{
			foreach (XmlElement item in element.SelectNodes("alias"))
			{
				if (0 == item.InnerText.Length)
				{
					throw new ApplicationException("alias node is empty at node " + element.Name + "!");
				}
				_aliases.Add(item.InnerText);
			}
		}
	}
}
