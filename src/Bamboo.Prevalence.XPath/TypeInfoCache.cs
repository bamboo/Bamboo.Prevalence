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
using System.Collections;
using System.Reflection;

namespace Bamboo.Prevalence.XPath
{
	/// <summary>
	/// A cache for the navigable properties of a type.
	/// </summary>
	public class TypeInfoCache
	{
		/// <summary>
		/// For types with no navigable properties (like primitives).
		/// </summary>
		public static PropertyInfo[] EmptyPropertyInfoArray = new PropertyInfo[0];

		System.Collections.Hashtable _cache;

		/// <summary>
		/// Constructs an empty TypeInfoCache.
		/// </summary>
		public TypeInfoCache()
		{
			_cache = new System.Collections.Hashtable();
		}

		/// <summary>
		/// Return the navigable properties for the object passed
		/// as argument. Any readable public property is considered
		/// navigable.
		/// </summary>
		/// <param name="o">object</param>
		/// <returns>array of navigable properties</returns>
		public PropertyInfo[] GetNavigableProperties(object o)
		{
			PropertyInfo[] properties = (PropertyInfo[])_cache[o];
			if (null == properties)
			{
				properties = FindNavigableProperties(o);
				_cache[o] = properties;
			}
			return properties;
		}

		private PropertyInfo[] FindNavigableProperties(object o)
		{
			if (o.GetType().IsPrimitive)
			{
				return EmptyPropertyInfoArray;
			}

			ArrayList children = new ArrayList();

			BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
			foreach (MemberInfo mi in o.GetType().FindMembers(MemberTypes.Property, flags, null, null))
			{
				PropertyInfo pi = mi as PropertyInfo;
				if (pi.CanRead && 0 == pi.GetGetMethod().GetParameters().Length)
				{					
					children.Add(pi);
				}
			}

			return (PropertyInfo[])children.ToArray(typeof(PropertyInfo));
		}
	}
}
