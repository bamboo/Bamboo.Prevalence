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
using System.Text;
using System.Security.Principal;
using System.Security.Cryptography;

namespace Bamboo.Prevalence.Examples.UserLogin
{
	/// <summary>
	/// A very simple implementation of IPrincipal and IIdentity.
	/// You might want to extend it to support roles, custom fields, etc.
	/// In this implementation, the original user password is never stored
	/// (neither in the commandlog or snapshot files), this is both good
	/// and bad.
	/// </summary>
	[Serializable]
	public class User : IPrincipal, IIdentity, IComparable
	{	
		string _email;
		string _fullname;
		byte[] _passwordHash;
		
		public User(string email) : this(email, email, null)
		{
		}

		public User(string email, string fullname) : this(email, fullname, null)
		{			
		}

		public User(string email, string fullname, string password)
		{
			if (null == email || email.Length == 0)
			{
				throw new ArgumentException("email");
			}

			if (null == fullname || fullname.Length == 0)
			{
				throw new ArgumentException("fullname");
			}

			_email = email;
			_fullname = fullname;
			SetPassword(password);
		}
		
		public string Email
		{
			get
			{
				return _email;
			}
		}

		public string FullName
		{
			get
			{
				return _fullname;
			}
		}

		public override string ToString()
		{
			return _fullname;
		}		

		public void SetPassword(string password)
		{
			_passwordHash = ComputeHash(password);
		}

		public bool CheckPassword(string password)
		{
			byte[] passwordHash = ComputeHash(password);
			return Equals(passwordHash, _passwordHash);
		}	
		
		#region Implementation of IIdentity
		public bool IsAuthenticated
		{
			get
			{
				return true;
			}
		}

		public string Name
		{
			get
			{
				return _email;
			}
		}

		public string AuthenticationType
		{
			get
			{
				return "Bamboo.Prevalence.Examples.UserLogin";
			}
		}
		#endregion

		#region Implementation of IPrincipal
		public bool IsInRole(string roleName)
		{			
			return roleName == "User";
		}

		public System.Security.Principal.IIdentity Identity
		{
			get
			{
				return this;
			}
		}
		#endregion

		#region IComparable Members

		public int CompareTo(object obj)
		{
			return FullName.ToLower().CompareTo(((User)obj).FullName.ToLower());
		}
		
		#endregion

		static byte[] ComputeHash(string text)
		{
			if (null == text || 0 == text.Length)
			{
				return new byte[0];
			}

			byte[] bytes = Encoding.UTF8.GetBytes(text);			
			return MD5.Create().ComputeHash(bytes);
		}

		static bool Equals(byte[] lhs, byte[] rhs)
		{
			if (lhs.Length != rhs.Length)
			{
				return false;
			}

			for (int i = 0; i<lhs.Length; ++i)
			{
				if (lhs[i] != rhs[i])
				{
					return false;
				}
			}
			return true;
		}		
	}
}
