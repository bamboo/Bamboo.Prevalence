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
						if (field.FieldType != value.GetType())
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
			throw new InvalidOperationException("ChainSelector not supported!");
		}
		#endregion
	}	
}
