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
