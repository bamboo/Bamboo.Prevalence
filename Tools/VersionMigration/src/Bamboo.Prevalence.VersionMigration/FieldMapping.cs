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
using System.Reflection;
using System.Xml;
using Bamboo.Prevalence.VersionMigration.Initializers;

namespace Bamboo.Prevalence.VersionMigration
{
	/// <summary>
	/// FieldMapping.
	/// </summary>
	public class FieldMapping
	{
		public static readonly IFieldInitializer DefaultFieldInitializer = new Bamboo.Prevalence.VersionMigration.Initializers.DefaultInitializer();

		public static void RegisterInitializer(string tagName, Type type)
		{
			_initializers[tagName] = type;
		}		

		public static IFieldInitializer LoadRegisteredInitializer(XmlElement initializerElement)
		{
			Type type = _initializers[initializerElement.Name] as Type;
			if (null == type)
			{
				throw new ApplicationException("Field initializer not registered: " + initializerElement.Name);
			}

			ConstructorInfo constructor = type.GetConstructor(new Type[] { typeof(XmlElement) });
			if (null == constructor)
			{
				throw new ApplicationException(type + " must provide a public constructor taking a System.Xml.XmlElement!");
			}
			return (IFieldInitializer)constructor.Invoke(new object[] { initializerElement });
		}

		private static Hashtable _initializers;

		private string _fieldName;

		private IFieldInitializer _initializer;

		static FieldMapping()
		{
			_initializers = new Hashtable();
			_initializers["const"] = typeof(ConstInitializer);
			_initializers["fromField"] = typeof(FromFieldInitializer);
			_initializers["newObject"] = typeof(NewObjectInitializer);
			_initializers["new"] = typeof(NewObjectInitializer);
			_initializers["guid"] = typeof(GuidInitializer);
			_initializers["null"] = typeof(NullInitializer);
		}

		internal FieldMapping(XmlElement fieldElement)
		{
			Load(fieldElement);			
		}

		public string FieldName
		{
			get
			{
				return _fieldName;
			}
		}

		public IFieldInitializer Initializer
		{
			get
			{
				return _initializer;
			}
		}

		private void Load(XmlElement fieldElement)
		{
			_fieldName = fieldElement.GetAttribute("name");
			LoadInitializer(fieldElement);
		}

		private void LoadInitializer(XmlElement fieldElement)
		{
			XmlElement initializerElement = fieldElement.SelectSingleNode("*") as XmlElement;
			if (null == initializerElement)
			{
				_initializer = DefaultFieldInitializer;
			}
			else
			{
				_initializer = LoadRegisteredInitializer(initializerElement);
			}
		}
	}
}
