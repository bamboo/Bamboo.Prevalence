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
	public class Account
	{
		private String login;
		private String password;
		private ContactInfo contactInfo;
		private CreditCard creditCard;
		private Category preferredCategory;
		private bool listFeatureActive;
		private bool bannerFeatureActive;

		public static bool checkUpdate(ContactInfo contactInfo, 
			CreditCard creditCard, Category category) 
		{
			if ( contactInfo == null) return false;
			if ( creditCard == null) return false;
			if ( category == null) return false;
			return true;
		}
		
		public Account(String login, String password) 
		{
			this.login = login;
			this.password = password;
		}
		
		public void update(ContactInfo contactInfo, CreditCard creditCard, 
			Category category, bool listFeature, bool bannerFeature) 
		{
			this.contactInfo = contactInfo;
			this.creditCard = creditCard;
			this.preferredCategory = category;
			this.listFeatureActive = listFeature;
			this.bannerFeatureActive = bannerFeature;
		}
		
		public String getLogin() 
		{
			return login;
		}

		public bool isPasswordCorrect(String password) 
		{
			return this.password.Equals(password);
		}

		public ContactInfo getContactInfo() 
		{
			return contactInfo;
		}

		public CreditCard getCreditCard() 
		{
			return creditCard;
		}

		public bool isBannerFeatureActive() 
		{
			return bannerFeatureActive;
		}

		public bool isListFeatureActive() 
		{
			return listFeatureActive;
		}

		public Category getPreferredCategory() 
		{
			return preferredCategory;
		}
	}
}
