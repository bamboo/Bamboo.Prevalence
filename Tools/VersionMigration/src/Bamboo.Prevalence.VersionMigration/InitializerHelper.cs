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
using System.Xml;
using System.Reflection;

namespace Bamboo.Prevalence.VersionMigration
{
	/// <summary>
	/// Helper methods for the IFieldInitializer/IObjectInitializer
	/// interfaces.
	/// </summary>
	public class InitializerHelper
	{
		public static object LoadInitializer(Type type, XmlElement initializerElement)
		{
			ConstructorInfo constructor = type.GetConstructor(new Type[] { typeof(XmlElement) });
			if (null == constructor)
			{
				throw new ApplicationException(type + " must provide a public constructor taking a System.Xml.XmlElement!");
			}
			try
			{
				return constructor.Invoke(new object[] { initializerElement });
			}
			catch (TargetInvocationException x)
			{
				throw x.InnerException;
			}
		}

		public static object LoadInitializer(XmlElement element)
		{
			string typeName = element.GetAttribute("type");
			if (null == typeName || 0 == typeName.Length)
			{
				throw new ApplicationException(string.Format("Expected type attribute for element {0}!", element.LocalName));
			}

			Type type = Type.GetType(typeName);
			if (null == type)
			{
				throw new ApplicationException(string.Format("Type \"{0}\" not found!", typeName));
			}
			return InitializerHelper.LoadInitializer(type, element);
		}

		public static void SetField(object target, string name, object value)
		{			
			GetFieldInfo(target, name).SetValue(target, value);
		}

		public static object GetField(object target, string name)
		{
			return GetFieldInfo(target, name).GetValue(target);
		}

		public static FieldInfo GetFieldInfo(object target, string name)
		{
			if (null == target)
			{
				throw new ArgumentNullException("target");
			}

			FieldInfo fi = target.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			if (null == fi)
			{
				throw new ApplicationException(string.Format("Field {0} not found in type {1}!", name, target.GetType()));
			}
			return fi;
		}

		public static object Call(object target, string methodName, params object[] parameters)
		{
			MemberInfo[] found = target.GetType().GetMember(methodName, MemberTypes.Method, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			foreach (MethodInfo method in found)
			{
				if (CheckParameterTypes(method, parameters))
				{
					return method.Invoke(target, parameters);
				}
			}
			throw new ApplicationException(string.Format("No appropriate overload for the method {0} was found!", methodName));
		}

		static bool CheckParameterTypes(MethodInfo mi, object[] parameters)
		{
			ParameterInfo[] pis = mi.GetParameters();
			if (pis.Length != parameters.Length)
			{
				return false;
			}

			for (int i=0; i<parameters.Length; ++i)
			{
				if (null != parameters[i])
				{
					if (pis[i].ParameterType != parameters[i].GetType())
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
