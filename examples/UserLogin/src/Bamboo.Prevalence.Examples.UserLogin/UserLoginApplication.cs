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
using System.Configuration;
using System.Web;
using System.Web.Security;
using Bamboo.Prevalence;

namespace Bamboo.Prevalence.Examples.UserLogin.Web
{
	public class UserLoginApplication : HttpApplication
	{
		static PrevalenceEngine _engine;
		
		public static PrevalenceEngine UserLoginEngine
		{
			get
			{
				return _engine;
			}
		}
		
		public static UserLoginSystem UserLoginSystem
		{
			get
			{
				return (UserLoginSystem)_engine.PrevalentSystem;
			}
		}
		
		protected void Application_Start(object sender, EventArgs args)
		{			
			_engine = PrevalenceActivator.CreateTransparentEngine(typeof(UserLoginSystem), PrevalenceBase);
		}
		
		protected void Application_AuthenticateRequest(object sender, EventArgs args)
		{

			//
			// Here's the magic:
			// 	we translate an asp.net provided IPrincipal to
			//  our own custom IPrincipal implementation...
			//
			HttpContext context = HttpContext.Current;
			System.Security.Principal.IPrincipal user = context.User;
			
			//Console.WriteLine("Application_AuthenticateRequest: User={0}", user);
			if (null != user && user.Identity.IsAuthenticated)
			{
				//Console.WriteLine("User.Identity.Name={0}", user.Identity.Name);
				
				try
				{
					context.User = UserLoginSystem.GetUser(user.Identity.Name);
				}
				catch (ApplicationException)
				{
					// user was removed?
					FormsAuthentication.SignOut();
					
					context.User = null;
				}
			}			
		}

		string PrevalenceBase
		{
			get
			{
				string prevalenceBase = ConfigurationSettings.AppSettings["Bamboo.Prevalence.BaseFolder"];
				if (null == prevalenceBase)
				{
					// I don't think you'll want your
					// prevalence base exposed on the web...
					// remember, this is just an example.
					prevalenceBase = Server.MapPath("/prevalence");
				}
				return prevalenceBase;
			}
		}

	}
}
