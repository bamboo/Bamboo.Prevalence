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
using Bamboo.Prevalence.VersionMigration;

namespace Bamboo.Prevalence.VersionMigration.Initializers
{
	/// <summary>
	/// DefaultInitializer.
	/// </summary>
	public class DefaultInitializer : IFieldInitializer
	{
		public void InitializeField(MigrationContext context)
		{			
			FieldInfo field = context.CurrentField;
			SerializationInfo info = context.CurrentSerializationInfo;

			try
			{
				field.SetValue(context.CurrentObject, info.GetValue(field.Name, field.FieldType));
			}
			catch (InvalidCastException)
			{
				object value = info.GetValue(field.Name, typeof(object));
				context.Trace("Failed to deserialize field {0} of type {1} from value {2} of type {3}!", field.Name, field.FieldType, value, null != value ? value.GetType().ToString() : "null");
				throw;
			}
		}
	}
}
