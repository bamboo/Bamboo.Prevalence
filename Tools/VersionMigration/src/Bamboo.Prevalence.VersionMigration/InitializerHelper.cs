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
			target.GetType().GetField(name).SetValue(target, value);
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
