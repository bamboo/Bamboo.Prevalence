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

namespace Bamboo.Prevalence.Collections
{
	/// <summary>
	/// Compares objects by property (the property value
	/// must be IComparable).
	/// </summary>
	public class ObjectPropertyComparer : IComparer
	{
		PropertyInfo _property;		

		public ObjectPropertyComparer(Type objectType, string propertyName)
		{
			MemberInfo[] members = objectType.GetMember(propertyName, MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public);
			if (1 != members.Length)
			{
				throw new ArgumentException(string.Format("Could not resolve the property name \"{0}\"!", propertyName), propertyName);
			}
			_property = (PropertyInfo)members[0];
		}

		public ObjectPropertyComparer(PropertyInfo property)
		{
			if (null == property)
			{
				throw new ArgumentNullException("property");
			}
			_property = property;
		}
		
		public int Compare(object lhs, object rhs)
		{
			object lhsValue = _property.GetValue(lhs, null);
			object rhsValue = _property.GetValue(rhs, null);
			
			int value = 0;
			if (null != lhsValue)
			{
				value = ((IComparable)lhsValue).CompareTo(rhsValue);
			}
			else if (null != rhsValue)
			{
				value = ((IComparable)rhsValue).CompareTo(lhsValue);
				value *= -1; // inverts the value since we changed the comparison
			}
			return value;
		}
	}
}
