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
