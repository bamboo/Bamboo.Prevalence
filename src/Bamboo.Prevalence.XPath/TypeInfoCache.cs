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
	public interface IValueProvider
	{
		string Name
		{
			get;
		}

		object GetValue(object instance);
	}

	public class PropertyInfoValueProvider : IValueProvider
	{
		PropertyInfo _pi;

		public PropertyInfoValueProvider(PropertyInfo pi)
		{
			_pi = pi;
		}

		public string Name
		{
			get
			{
				return _pi.Name;
			}
		}

		public object GetValue(object instance)
		{
			return _pi.GetValue(instance, (object[])null);
		}
	}

	public class FieldInfoValueProvider : IValueProvider
	{
		FieldInfo _fi;

		public FieldInfoValueProvider(FieldInfo fi)
		{
			_fi = fi;
		}

		public string Name
		{
			get
			{
				return _fi.Name;
			}
		}

		public object GetValue(object instance)
		{
			return _fi.GetValue(instance);
		}
	}

	/// <summary>
	/// A cache for the navigable properties of a type.
	/// </summary>
	public class TypeInfoCache
	{
		/// <summary>
		/// For types with no navigable properties (like primitives).
		/// </summary>
		public static IValueProvider[] EmptyValueProviderArray = new IValueProvider[0];

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
		public IValueProvider[] GetNavigableProperties(object o)
		{
			IValueProvider[] properties = (IValueProvider[])_cache[o];
			if (null == properties)
			{
				properties = FindNavigableProperties(o);
				_cache[o] = properties;
			}
			return properties;
		}

		private IValueProvider[] FindNavigableProperties(object o)
		{
			if (o.GetType().IsPrimitive)
			{
				return EmptyValueProviderArray;
			}

			ArrayList children = new ArrayList();

			BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
			foreach (MemberInfo mi in o.GetType().FindMembers(MemberTypes.Property | MemberTypes.Field, flags, null, null))
			{
				PropertyInfo pi = mi as PropertyInfo;
				if (null != pi)
				{
					if (pi.CanRead && 0 == pi.GetGetMethod().GetParameters().Length)
					{					
						children.Add(new PropertyInfoValueProvider(pi));
					}
				}
				else
				{
					children.Add(new FieldInfoValueProvider((FieldInfo)mi));
				}
			}

			return (IValueProvider[])children.ToArray(typeof(IValueProvider));
		}
	}
}
