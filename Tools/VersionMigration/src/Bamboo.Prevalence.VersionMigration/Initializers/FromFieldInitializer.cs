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
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;
using Bamboo.Prevalence.VersionMigration;

namespace Bamboo.Prevalence.VersionMigration.Initializers
{
	/// <summary>
	/// Initializes a field with the value of another
	/// field (that must be present in the serialized
	/// state). Use it when you have renamed
	/// a persistent field.
	/// </summary>
	public class FromFieldInitializer : IFieldInitializer
	{
		private string _fieldName;

		public FromFieldInitializer(XmlElement element)
		{
			_fieldName = element.InnerText;
			if (0 == _fieldName.Length)
			{
				throw new ApplicationException(string.Format("fromField tag is empty at field {0}!", element.ParentNode.Attributes["name"].Value));
			}
		}

		public void InitializeField(MigrationContext context)
		{			
			FieldInfo field = context.CurrentField;
			SerializationInfo info = context.CurrentSerializationInfo;

			object value = info.GetValue(_fieldName, field.FieldType);
			field.SetValue(context.CurrentObject, value);
			context.Trace("Field {0} set to \"{1}\" loaded from field {2}.", field.Name, value, _fieldName);
		}
	}
}
