#region License
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
#endregion

using System;
using System.Xml;
using System.Xml.XPath;
using Bamboo.Prevalence.XPath.Internal;

namespace Bamboo.Prevalence.XPath
{
	/// <summary>
	/// XPathNavigator implementation over an arbitrary
	/// object graph.
	/// </summary>
	/// <example>
	/// <code>
	/// Address address = new Address("Al. Calderão Branco", 784);
	/// Customer customer = new Customer("Rodrigo", "Oliveira", address);
	/// 
	/// XPathObjectNavigator context = new XPathObjectNavigator(customer);
	/// XPathNodeIterator i = context.Select("/Customer/Address/Street");			
	/// AssertEquals(1, i.Count);
	/// AssertEquals(true, i.MoveNext());			
	/// AssertEquals(customer.Address.Street, i.Current.Value);
	/// AssertEquals(customer.Address.Street, ((XPathObjectNavigator)i.Current).Node);
	/// </code>
	/// </example>
	public class XPathObjectNavigator : XPathNavigator
	{	
		ObjectNavigatorState _state;

		ObjectNavigatorState _root;

		ObjectNavigationContext _context;

		string _lang;

		public XPathObjectNavigator(object node, string nodeName)
		{
			_context = new ObjectNavigationContext();
			_context.NameTable.Add(string.Empty);			
			_root = new ObjectNavigatorStateRoot(_context, node, nodeName);
			_state = _root.MoveToFirstChild();
			_lang = _context.NameTable.Add("en-US");
		}

		public XPathObjectNavigator(object node) : this(node, null)
		{
		}

		public XPathObjectNavigator(XPathObjectNavigator other)
		{
			_context = other._context;
			_state = other._state;
			_root = other._root;
			_lang = other._lang;
		}

		public object SelectObject(string xpath)
		{
			XPathNodeIterator i = Select(xpath);
			if (i.MoveNext())
			{
				return ((XPathObjectNavigator)i.Current).Node;
			}
			return null;
		}

		public override string BaseURI
		{
			get
			{
				Trace("get_BaseURI");
				return _context.NameTable.Get(string.Empty);
			}
		}

		public override System.Xml.XPath.XPathNavigator Clone()
		{
			Trace("Clone");
			return new XPathObjectNavigator(this);
		}

		public override string GetAttribute(string localName, string namespaceURI)
		{
			Trace("GetAttribute");
			return string.Empty;
		}

		public override string GetNamespace(string name)
		{
			Trace("GetNamespace");
			return _context.NameTable.Get(string.Empty);
		}

		public override bool HasAttributes
		{
			get
			{
				Trace("HasAttributes");
				return false;
			}
		}

		public override bool HasChildren
		{
			get
			{
				Trace("HasChildren");
				return false;
			}
		}

		public override bool IsEmptyElement
		{
			get
			{
				Trace("IsEmptyElement");
				return true;
			}
		}

		public override bool IsSamePosition(System.Xml.XPath.XPathNavigator other)
		{
			Trace("IsSamePosition");
			XPathObjectNavigator x = other as XPathObjectNavigator;
			if (null == x)
			{
				return false;
			}
			return _state.IsSamePosition(x._state);
		}

		public override string LocalName
		{
			get
			{
				Trace("get_LocalName");
				return _state.Name;
			}
		}

		public override string Name
		{
			get
			{
				Trace("get_Name");
				return _state.Name;
			}
		}

		public override string NamespaceURI
		{
			get
			{
				Trace("get_NamespaceURI");
				return _context.NameTable.Get(string.Empty);
			}
		}

		public override System.Xml.XmlNameTable NameTable
		{
			get
			{
				Trace("get_NameTable");
				return _context.NameTable;
			}
		}

		public override bool MoveTo(System.Xml.XPath.XPathNavigator other)
		{
			Trace("MoveTo");
			XPathObjectNavigator navigator = other as XPathObjectNavigator;
			if (null == other)
			{
				return false;
			}
			_state = navigator._state;
			_root = navigator._root;
			_context = navigator._context;
			return true;
		}

		public override bool MoveToAttribute(string localName, string namespaceURI)
		{
			Trace("MoveToAttribute");
			return false;
		}

		public override bool MoveToFirst()
		{
			Trace("MoveToFirst");
			return false;
		}

		public override bool MoveToFirstAttribute()
		{
			Trace("MoveToFirstAttribute");
			return false;
		}

		public override bool MoveToFirstChild()
		{
			Trace("MoveToFirstChild");
			ObjectNavigatorState newstate = _state.MoveToFirstChild();
			if (null == newstate)
			{
				return false;
			}
			_state = newstate;
			return true;
		}

		public override bool MoveToFirstNamespace(System.Xml.XPath.XPathNamespaceScope namespaceScope)
		{
			Trace("MoveToFirstNamespace");
			return false;
		}

		public override bool MoveToId(string id)
		{
			Trace("MoveToId");
			return false;
		}

		public override bool MoveToNamespace(string name)
		{
			Trace("MoveToNamespace");
			return false;
		}

		public override bool MoveToNext()
		{
			Trace("MoveToNext");
			ObjectNavigatorState newstate = _state.MoveToNext();
			if (null != newstate)
			{
				_state = newstate;
				return true;
			}
			return false;
		}

		public override bool MoveToNextAttribute()
		{
			Trace("MoveToNextAttribute");
			return false;
		}

		public override bool MoveToNextNamespace(System.Xml.XPath.XPathNamespaceScope namespaceScope)
		{
			Trace("MoveToNextNamespace");
			return false;
		}

		public override bool MoveToParent()
		{
			Trace("MoveToParent");
			if (null != _state.Parent)
			{
				_state = _state.Parent;
				return true;
			}
			return false;
		}

		public override bool MoveToPrevious()
		{
			Trace("MoveToPrevious");
			return false;
		}

		public override void MoveToRoot()
		{
			Trace("MoveToRoot");
			_state = _root;
		}

		public override System.Xml.XPath.XPathNodeType NodeType
		{
			get
			{
				Trace("get_NodeType");
				return _state.NodeType;
			}
		}

		public override string Value
		{
			get
			{
				Trace("get_Value");
				return _state.Value;
			}
		}

		public object Node
		{
			get
			{
				return _state.Node;
			}
		}

		public override string XmlLang
		{
			get
			{
				return _lang;
			}
		}

		public override string Prefix
		{
			get
			{
				Trace("get_Prefix");
				return _context.NameTable.Get(string.Empty);
			}
		}	
		
		private void Trace(string format, params object[] args)
		{
			System.Diagnostics.Trace.WriteLine(string.Format(format, args));
		}
	}
}
