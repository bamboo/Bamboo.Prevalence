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

namespace PetStoreWeb.Commands
{
	using System;
	using Bamboo.Prevalence;
	using PetStoreWeb.Components;

	[Serializable()]
	public class AccountUpdateCommand : ICommand
	{
	
		private String login;
		private ContactInfo contactInfo;
		private CreditCard creditCard;
		private Category preferredCategory;
		private bool listFeatureActive;
		private bool bannerFeatureActive;
	
		public AccountUpdateCommand(Account account, ContactInfo contactInfo, CreditCard creditCard, Category category, bool listFeature, bool bannerFeature) 
		{
		
			if (account == null) 
				throw new ArgumentException("Account cannot be null");
		
			this.login = account.getLogin();
		
			this.contactInfo = contactInfo;
			this.creditCard = creditCard;
			this.preferredCategory = category;
			this.listFeatureActive = listFeature;
			this.bannerFeatureActive = bannerFeature;
		}

		public object Execute(object system) 
		{
		
			Account account = ((PetStore)system).getAccount(this.login);
		
			if(!(Account.checkUpdate(this.contactInfo, this.creditCard, this.preferredCategory)))
				throw new ArgumentException("Illegal Command parameter. Update was not performed.");
		
			account.update(this.contactInfo, this.creditCard, this.preferredCategory, this.listFeatureActive, this.bannerFeatureActive);

			return account;
		}
	}
}
