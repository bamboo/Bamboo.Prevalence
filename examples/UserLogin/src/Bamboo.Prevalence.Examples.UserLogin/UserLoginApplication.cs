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
			if (null != user && user.Identity.IsAuthenticated)
			{
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
