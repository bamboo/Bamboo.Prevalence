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
	using System.Collections;

	[Serializable()]
	public class ShoppingCart 
	{
	
		private readonly ArrayList items;
	
		public ShoppingCart() 
		{
			this.items = new ArrayList();
		}

		public void addItem(CartItem item) 
		{
			CartItem myItem = containsItem(item);
			if(myItem != null)
				myItem.setQuantity(myItem.getQuantity() + item.getQuantity());
			else
				this.items.Add(item);
		}
	
		public void removeItem(int itemIndex) 
		{
			CartItem cartItem = (CartItem)items[itemIndex];
			this.items.Remove(cartItem);
		}
	
		public CartItem[] getCartItems() 
		{
			if (items.Count > 0)
			{
				return (CartItem[]) this.items.ToArray(typeof(CartItem));
			}
			else
			{
				return new CartItem[0];
			}
		}
	
		public CartItem containsItem(CartItem item) 
		{
			CartItem myItem = null;
			for ( int i = 0; i < items.Count ; i++ ) 
			{	
				myItem = (CartItem)	items[i];
				if (myItem.item.Equals(item.item))
					return myItem;
			}
			return null;
		}
	}
}
