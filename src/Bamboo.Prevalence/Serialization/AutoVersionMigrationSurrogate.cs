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

namespace Bamboo.Prevalence.Serialization
{
	/// <summary>
	/// Serialization surrogate and selector that allows any new version of a
	/// serialiazable class to be loaded from an outdated serialized
	/// representation.
	/// </summary>
	/// <remarks>
	/// Any new class fields not present in the serialization data will be
	/// left uninitialized. Removed fields are ignored (no exceptions).
	/// </remarks>
	public class AutoVersionMigrationSurrogate : ISerializationSurrogate, ISurrogateSelector
	{	
		private Assembly _assemblyToMigrate;

		/// <summary>
		/// Create a surrogate that will be used to serialize/deserialize all the
		/// types in the assembly specified.
		/// </summary>
		/// <param name="assemblyToMigrate"></param>
		public AutoVersionMigrationSurrogate(Assembly assemblyToMigrate)
		{
			_assemblyToMigrate = assemblyToMigrate;
		}

		#region Implementation of ISerializationSurrogate
		void System.Runtime.Serialization.ISerializationSurrogate.GetObjectData(object obj, System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{		
			Type objectType  = obj.GetType();
			foreach (FieldInfo member in objectType.FindMembers(MemberTypes.Field, BindingFlags.Instance|BindingFlags.NonPublic|BindingFlags.Public, null, null))
			{
				if (member.IsDefined(typeof(NonSerializedAttribute), false))
				{
					continue;
				}
				
				info.AddValue(member.Name, member.GetValue(obj));
			}
		}

		object System.Runtime.Serialization.ISerializationSurrogate.SetObjectData(object obj, System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context, System.Runtime.Serialization.ISurrogateSelector selector)
		{
			Type objectType = obj.GetType();			
			
			foreach (SerializationEntry entry in info)
			{					
				MemberInfo[] members = objectType.GetMember(entry.Name, MemberTypes.Field, BindingFlags.NonPublic|BindingFlags.Public|BindingFlags.Instance);
				if (members.Length > 0)
				{
					FieldInfo field = (FieldInfo)members[0];
					object value = entry.Value;
					if (null != value)
					{
						if (!field.FieldType.IsInstanceOfType(value))
						{
							value = Convert.ChangeType(value, field.FieldType);
						}
					}
					field.SetValue(obj, value);
				}
			}
			return null;
		}	
		#endregion

		#region Implementation of ISurrogateSelector
		System.Runtime.Serialization.ISurrogateSelector ISurrogateSelector.GetNextSelector()
		{
			return null;
		}

		System.Runtime.Serialization.ISerializationSurrogate ISurrogateSelector.GetSurrogate(System.Type type, System.Runtime.Serialization.StreamingContext context, out System.Runtime.Serialization.ISurrogateSelector selector)
		{
			if (type.Assembly == _assemblyToMigrate)
			{
				selector = this;
				return this;
			}
			selector = null;
			return null;
		}

		void ISurrogateSelector.ChainSelector(System.Runtime.Serialization.ISurrogateSelector selector)
		{
			throw new NotImplementedException("ChainSelector not supported!");
		}
		#endregion
	}	
}
