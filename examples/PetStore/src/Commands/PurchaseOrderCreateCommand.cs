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
	public class PurchaseOrderCreateCommand : ICommand 
	{
	
		String accountLogin;
		ContactInfo shippingInfo;
		ContactInfo billingInfo; 
		CartItem[] items;

		public PurchaseOrderCreateCommand(Account account, ContactInfo shippingInfo, ContactInfo billingInfo, CartItem[] items) 
		{
			this.accountLogin = account.getLogin();
			this.shippingInfo = shippingInfo;
			this.billingInfo = billingInfo;
			this.items = items;	
		}

		public object Execute(object system)
		{
			PurchaseOrder purchaseOrder = null;
			PetStore ps  = (PetStore) system;
			long id = ps.nextPurchaseOrderId();
			if (!PurchaseOrder.checkCreate(id, ps.getAccount(accountLogin), this.shippingInfo, this.billingInfo, this.items, PrevalenceEngine.Now)) 
			{
				throw new ArgumentException ("Invalid purchase order parameters.");			
			}
			purchaseOrder = new PurchaseOrder(id, ps.getAccount(accountLogin), shippingInfo, billingInfo, items, PrevalenceEngine.Now);
			ps.addPurchaseOrder(purchaseOrder);
			return purchaseOrder;
		}
	}
}