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
