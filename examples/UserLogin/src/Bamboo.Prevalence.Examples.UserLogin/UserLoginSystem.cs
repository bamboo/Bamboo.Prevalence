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
using System.Collections;
using Bamboo.Prevalence.Attributes;

namespace Bamboo.Prevalence.Examples.UserLogin
{
	[Serializable]
	public class UserLoginSystem : System.MarshalByRefObject
	{	
		Hashtable _users;
		
		public UserLoginSystem()
		{			
			_users = new Hashtable(CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
			
			// admin user is added by default with empty password
			AddUser(new User("administrator", "Administrator", ""));
		}
		
		public void AddUser(User user)
		{
			if (null == user)
			{
				throw new ArgumentNullException("user");
			}
			
			if (_users.ContainsKey(user.Email))
			{
				throw new ApplicationException(string.Format("The user {0} is already registered!", user.Email));			
			}
			
			_users[user.Email] = user;
		}
		
		[Query]
		public User LogonUser(string email, string password)
		{		
			User user = GetUser(email);
			if (!user.CheckPassword(password))
			{
				throw new ApplicationException("Invalid password!");
			}
			return user;
		}
		
		[Query]
		public User GetUser(string email)
		{
			User user = (User)_users[email];
			if (null == user)
			{
				throw new ApplicationException(string.Format("User {0} not found!", email));
			}
			return user;
		}
	}
}
