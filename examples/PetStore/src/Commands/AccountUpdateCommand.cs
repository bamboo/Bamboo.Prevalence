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
