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
using System.Collections;

namespace RemotingPrevalence
{
	[Serializable]
	public class Contact
	{
		private long _id;

		private string _name;

		private string _email;

		public Contact()
		{
			_id = -1;
		}

		public long ID
		{
			get
			{
				return _id;
			}

			set
			{
				if (-1 != _id)
				{
					throw new InvalidOperationException("ID cannot be changed!");
				}
				_id = value;
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}

			set
			{
				_name = value;
			}
		}

		public string Email
		{
			get
			{
				return _email;
			}

			set
			{
				_email = value;
			}
		}

		public void Validate()
		{
			AssertFieldNotEmpty("Name", _name);
			AssertFieldNotEmpty("Email", _email);
		}

		private void AssertFieldNotEmpty(string fieldName, string value)
		{
			if (null == value || 0 == value.Length)
			{
				throw new ApplicationException(fieldName + " is required!");
			}
		}
	}	

	public delegate void AddressBookChangedEventHandler();

	[Serializable]
	public class AddressBook : System.MarshalByRefObject
	{
		private Hashtable _contacts;

		private long _nextID;			
	
		[NonSerialized] // make sure we do not try to serialize the delegate list...
		private AddressBookChangedEventHandler _changedEventHandler = null;

		public AddressBook()
		{
			_contacts = new Hashtable();
		}

		public void AddContact(Contact contact)
		{
			contact.Validate();

			contact.ID = _nextID++;

			_contacts.Add(contact.ID, contact);

			OnAddressBookChanged();
		}

		public void RemoveContact(long id)
		{
			Contact contact = _contacts[id] as Contact;

			if (null == contact)
			{
				throw new ArgumentException("Contact not found!", "id");
			}		

			_contacts.Remove(id);

			OnAddressBookChanged();
		}

		public IList Contacts
		{
			get
			{
				Contact[] contacts = new Contact[_contacts.Count];
				_contacts.Values.CopyTo(contacts, 0);
				return contacts;				
			}
		}

		// we have to synchronize the access to the delegate
		// ourselves since the prevalence engine knows nothing
		// about delegates...
		public event AddressBookChangedEventHandler Changed
		{
			add
			{
				lock (this)
				{
					_changedEventHandler = (AddressBookChangedEventHandler)Delegate.Combine(_changedEventHandler, value);
				}
			}

			remove
			{
				lock (this)
				{
					_changedEventHandler = (AddressBookChangedEventHandler)Delegate.Remove(_changedEventHandler, value);
				}
			}
		}

		protected void OnAddressBookChanged()
		{
			try
			{
				lock (this)
				{
					if (null != _changedEventHandler)
					{
						_changedEventHandler();
					}
				}
			}
			catch
			{
			}
		}

		[Bamboo.Prevalence.Attributes.PassThrough]
		public override object InitializeLifetimeService()
		{
			// object lives till the end of AppDomain...
			return null;
		}
	}
}
