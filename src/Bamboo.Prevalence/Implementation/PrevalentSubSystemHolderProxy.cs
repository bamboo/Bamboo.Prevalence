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
using System.Collections.Specialized;
using System.Runtime.Remoting.Messaging;
using Bamboo.Prevalence.Attributes;

namespace Bamboo.Prevalence.Implementation
{
	internal class PrevalentSubSystemHolderProxy : PrevalentSystemProxy
	{
		HybridDictionary _proxies;

		public PrevalentSubSystemHolderProxy(PrevalenceEngine engine, MarshalByRefObject system) : base(engine, system)
		{
		}

		public HybridDictionary Proxies
		{
			get
			{
				if (_proxies == null)
				{
					_proxies = new HybridDictionary();
				}

				return _proxies;
			}
		}

		protected override IMessage ExecuteQuery(IMethodCallMessage call)
		{
			_engine.BeforeQuery();

			try
			{
				IMethodReturnMessage message = (IMethodReturnMessage) base.InvokeSystem(call);

				if (IsSubSystem(call.MethodBase))
				{
					return new ReturnMessage(GetProxy(message), null, 0, call.LogicalCallContext, call);
				}
				else
				{
					return message;
				}
			}
			finally
			{
				_engine.AfterQuery();
			}
		}

		private object GetProxy(IMethodReturnMessage message)
		{
			string key = message.MethodName;

			if (!Proxies.Contains(key))
			{
				Proxies.Add(key, CreateProxy(message));
			}

			return Proxies[key];
		}

		private object CreateProxy(IMethodReturnMessage message)
		{
			SubSystemAttribute subSystemAttr = Attribute.GetCustomAttribute(GetMemberInfo(message.MethodBase), typeof(SubSystemAttribute)) as SubSystemAttribute;

			MarshalByRefObject subSystem = message.ReturnValue as MarshalByRefObject;

			if (subSystem == null)
			{
				throw new InvalidOperationException("SubSystem type must extend MarshalByRefObject to be used with TransparentPrevalenceEngine!");
			}

			return new PrevalentSubSystemProxy(_engine, subSystem, subSystemAttr.FieldName).GetTransparentProxy();
		}

		private bool IsSubSystem(MethodBase method)
		{
			return Attribute.IsDefined(GetMemberInfo(method), typeof(SubSystemAttribute), true);
		}

		private MemberInfo GetMemberInfo(MethodBase method)
		{
			MemberInfo member = method;

			if (IsPropertyGet(method))
			{
				member = _system.GetType().GetProperty(method.Name.Substring(4));
			}

			return member;
		}
	}
}
