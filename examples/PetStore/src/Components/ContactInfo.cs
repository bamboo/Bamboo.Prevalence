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

namespace PetStoreWeb.Components
{
	using System;

	[Serializable()]
	public class ContactInfo
	{
		public readonly String firstName;
		public readonly String lastName;
		public readonly String phone;
		public readonly String email;
		public readonly String street;
		public readonly String city;
		public readonly String state;
		public readonly String postalcode;
		public readonly String country;

		public ContactInfo(String firstName, String lastName, String phone, 
			String email, String street, String city, String state, 
			String postalcode, String country) 
		{
			this.firstName = Utilities.nullToString(firstName);
			this.lastName = Utilities.nullToString(lastName);
			this.phone = Utilities.nullToString(phone);
			this.email = Utilities.nullToString(email);
			this.street = Utilities.nullToString(street);
			this.city = Utilities.nullToString(city);
			this.state = Utilities.nullToString(state);
			this.postalcode = Utilities.nullToString(postalcode);
			this.country = Utilities.nullToString(country);
		}
	
		public String getEmail() 
		{
			return email;
		}
	}
}

