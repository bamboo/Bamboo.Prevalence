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

using System;
using System.Collections;
using Bamboo.Prevalence.Attributes;

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
			[PassThrough]
			add
			{
				lock (this)
				{
					_changedEventHandler = (AddressBookChangedEventHandler)Delegate.Combine(_changedEventHandler, value);
				}
			}

			[PassThrough]
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
