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
