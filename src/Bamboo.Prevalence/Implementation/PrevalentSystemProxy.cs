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

// Thanks to Jesse Ezell for coming up with the idea.

using System;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using Bamboo.Prevalence;
using Bamboo.Prevalence.Attributes;

namespace Bamboo.Prevalence.Implementation
{
	internal class PrevalentSystemProxy : RealProxy
	{
		private static Type ObjectType = typeof(object);

		private PrevalenceEngine _engine;

		private MarshalByRefObject _system;

		public PrevalentSystemProxy(PrevalenceEngine engine, MarshalByRefObject system) : base(system.GetType())
		{
			_engine = engine;
			_system = system;
		}

		public override System.Runtime.Remoting.Messaging.IMessage Invoke(System.Runtime.Remoting.Messaging.IMessage msg)
		{			
			IMethodCallMessage call = msg as IMethodCallMessage;

			try
			{		
				if (!IsPassThrough(call.MethodBase))
				{
					if (!IsNestedEngineCall())
					{						
						if (IsQuery(call.MethodBase))
						{
							return ExecuteQuery(call);
						}
						
						if (IsCommand(call.MethodBase))
						{
							return ExecuteCommand(call);						
						}
					}
				}
				return InvokeSystem(call);
			}
			catch (System.Reflection.TargetInvocationException x)
			{
				return new ReturnMessage(x.InnerException, call);
			}
			catch (Exception x)
			{
				return new ReturnMessage(x, call);
			}
		}

		private bool IsNestedEngineCall()
		{
			return _engine == PrevalenceEngine.Current;
		}

		private bool IsCommand(MethodBase method)
		{
			if (method.IsSpecialName)
			{
				if (method.Name.StartsWith("set_"))
				{
					return true;
				}

				// only property sets are commands
				return false;
			}

			// TODO: should we really consider any public
			// method a command?
			// wouldn't it be better to consider only the methods
			// declared by the type:
			// return method.DeclaringType == _system.GetType()
			return true;
		}
		
		private bool IsQuery(MethodBase method)
		{
			return IsPropertyGet(method) || Attribute.IsDefined(method, typeof(QueryAttribute));
		}

		private bool IsPassThrough(MethodBase method)
		{
			return Attribute.IsDefined(method, typeof(PassThroughAttribute)) ||
				method.DeclaringType == ObjectType;
		}

		private bool IsPropertyGet(MethodBase method)
		{
			return method.IsSpecialName && method.Name.StartsWith("get_");
		}

		private IMessage InvokeSystem(IMethodCallMessage call)
		{
			return RemotingServices.ExecuteMessage(_system, call);			
		}

		private IMessage ExecuteCommand(IMethodCallMessage call)
		{			
			object returnValue = _engine.ExecuteCommand(new MethodCallCommand(call.MethodName, call.Args));
			return new ReturnMessage(returnValue, null, 0, call.LogicalCallContext, call);
		}

		private IMessage ExecuteQuery(IMethodCallMessage call)
		{
			_engine.BeforeQuery();
			try
			{
				return InvokeSystem(call);
			}
			finally
			{
				_engine.AfterQuery();
			}
		}
	}
}
