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
			return LoadInitializer(type, initializerElement);
		}

		public static IFieldInitializer LoadInitializer(Type type, XmlElement initializerElement)
		{
			return (IFieldInitializer)InitializerHelper.LoadInitializer(type, initializerElement);
		}

		private static Hashtable _initializers;

		private string _fieldName;

		private IFieldInitializer _initializer;

		static FieldMapping()
		{
			_initializers = new Hashtable();
			_initializers["const"] = typeof(ConstInitializer);
			_initializers["fromField"] = typeof(FromFieldInitializer);
			_initializers["from"] = typeof(FromFieldInitializer);
			_initializers["newObject"] = typeof(NewObjectInitializer);
			_initializers["new"] = typeof(NewObjectInitializer);
			_initializers["guid"] = typeof(GuidInitializer);
			_initializers["null"] = typeof(NullInitializer);
			_initializers["custom"] = typeof(CustomInitializer);
			_initializers["sequential"] = typeof(SequentialInitializer);
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
