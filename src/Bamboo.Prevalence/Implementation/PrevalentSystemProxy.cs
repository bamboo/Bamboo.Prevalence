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
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using Bamboo.Prevalence;
using Bamboo.Prevalence.Attributes;

namespace Bamboo.Prevalence.Implementation
{
	internal class PrevalentSystemProxy : RealProxy
	{
		private PrevalenceEngine _engine;

		private object _system;

		public PrevalentSystemProxy(PrevalenceEngine engine, object system) : base(system.GetType())
		{
			_engine = engine;
			_system = system;
		}

		public override System.Runtime.Remoting.Messaging.IMessage Invoke(System.Runtime.Remoting.Messaging.IMessage msg)
		{			
			IMethodCallMessage call = msg as IMethodCallMessage;

			try
			{
				// if this is not a nested engine call...
				if (_engine != PrevalenceEngine.Current)
				{
					if (!IsPassThrough(call.MethodBase))
					{
						if (IsQuery(call.MethodBase))
						{
							return ExecuteQuery(call);
						}
						else if (IsCommand(call.MethodBase))
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
			return Attribute.IsDefined(method, typeof(PassThroughAttribute));
		}

		private bool IsPropertyGet(MethodBase method)
		{
			return method.IsSpecialName && method.Name.StartsWith("get_");
		}

		private IMessage InvokeSystem(IMethodCallMessage call)
		{
			object returnValue = call.MethodBase.Invoke(_system, BindingFlags.Instance, null, call.Args, null);
			return new ReturnMessage(returnValue, null, 0, call.LogicalCallContext, call);			
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
