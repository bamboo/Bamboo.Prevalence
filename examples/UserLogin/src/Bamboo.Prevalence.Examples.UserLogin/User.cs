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
