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
	public class PurchaseOrder
	{
		public readonly long id;	
		public readonly Account account;
		public readonly ContactInfo shippingInfo;
		public readonly ContactInfo billingInfo;
		public readonly CartItem[] items;
		public readonly DateTime date;
		
		public PurchaseOrder(long id, Account account, 
			ContactInfo shippingInfo, ContactInfo billingInfo, 
			CartItem[] items, DateTime date) 
		{
			this.id = id;
			this.account = account;
			this.shippingInfo = shippingInfo;
			this.billingInfo = billingInfo;
			this.items = items;
			this.date = date;
		}
		
		public static bool checkCreate(long id, Account account, 
			ContactInfo shippingInfo, ContactInfo billingInfo, 
			CartItem[] items, DateTime date) 
		{
			if (account == null) return false;
			if (shippingInfo == null) return false;
			if (billingInfo == null) return false;
			if (items == null) return false;
			return true;
		}
			
		public double total() 
		{
			double total = 0d;
			
			for (int i = 0; i < items.Length; i++) 
			{
				total += items[i].price * items[i].getQuantity();				
			}
			
			return total;	
		}
	}
}
