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

		/// <summary>
		/// Create a new navigator for the object graph
		/// starting at node. The node's name is nodeName.
		/// </summary>
		/// <param name="node">root</param>
		/// <param name="nodeName">root's name</param>
		public XPathObjectNavigator(object node, string nodeName)
		{
			_context = new ObjectNavigationContext();
			_context.NameTable.Add(string.Empty);			
			_root = new ObjectNavigatorStateRoot(_context, node, nodeName);
			_state = _root.MoveToFirstChild();
			_lang = _context.NameTable.Add("en-US");
		}

		/// <summary>
		/// Create a new navigator for the object graph
		/// starting at node. The node name will be
		/// node.GetType().Name.
		/// </summary>
		/// <param name="node">root</param>
		public XPathObjectNavigator(object node) : this(node, null)
		{
		}

		/// <summary>
		/// copy constructor.
		/// </summary>
		/// <param name="other">navigator to be copied</param>
		public XPathObjectNavigator(XPathObjectNavigator other)
		{
			_context = other._context;
			_state = other._state;
			_root = other._root;
			_lang = other._lang;
		}

		/// <summary>
		/// Selects a single object from the current node.
		/// </summary>
		/// <param name="xpath">selection expression</param>
		/// <returns>the first object returned by the
		/// expression or null</returns>
		public object SelectObject(string xpath)
		{
			XPathNodeIterator i = Select(xpath);
			if (i.MoveNext())
			{
				return ((XPathObjectNavigator)i.Current).Node;
			}
			return null;
		}

		/// <summary>
		/// Selects a group of objects from the current node.
		/// </summary>
		/// <param name="xpath">selection expression</param>
		/// <returns>an array with all the objects returned
		/// by the expression</returns>
		public object[] SelectObjects(string xpath)
		{
			System.Collections.ArrayList result = new System.Collections.ArrayList();
			XPathNodeIterator i = Select(xpath);
			while (i.MoveNext())
			{
				result.Add(((XPathObjectNavigator)i.Current).Node);
			}
			return (object[])result.ToArray(typeof(object));
		}

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.BaseURI" /> for details.
		/// </summary>
		public override string BaseURI
		{
			get
			{
				Trace("get_BaseURI");
				return _context.NameTable.Get(string.Empty);
			}
		}

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.Clone" /> for details.
		/// </summary>
		public override System.Xml.XPath.XPathNavigator Clone()
		{
			Trace("Clone");
			return new XPathObjectNavigator(this);
		}

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.GetAttribute(string, string)" /> for details.
		/// </summary>
		/// <remarks>No attributes are returned.</remarks>
		public override string GetAttribute(string localName, string namespaceURI)
		{
			Trace("GetAttribute");
			return string.Empty;
		}

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.GetNamespace(string)" /> for details.
		/// </summary>
		/// <remarks>Namespace is always empty</remarks>
		public override string GetNamespace(string name)
		{
			Trace("GetNamespace");
			return _context.NameTable.Get(string.Empty);
		}

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.HasAttributes" /> for details.
		/// </summary>
		/// <remarks>false</remarks>
		public override bool HasAttributes
		{
			get
			{
				Trace("HasAttributes");
				return false;
			}
		}

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.HasChildren" /> for details.
		/// </summary>
		public override bool HasChildren
		{
			get
			{
				Trace("HasChildren");
				return false;
			}
		}

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.IsEmptyElement" /> for details.
		/// </summary>
		public override bool IsEmptyElement
		{
			get
			{
				Trace("IsEmptyElement");
				return true;
			}
		}

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.IsSamePosition" /> for details.
		/// </summary>
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

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.LocalName" /> for details.
		/// </summary>
		public override string LocalName
		{
			get
			{
				Trace("get_LocalName");
				return _state.Name;
			}
		}

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.Name" /> for details.
		/// </summary>
		/// <remarks>Same as LocalName</remarks>
		public override string Name
		{
			get
			{
				Trace("get_Name");
				return _state.Name;
			}
		}

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.NamespaceURI" /> for details.
		/// </summary>
		/// <remarks>Always empty</remarks>
		public override string NamespaceURI
		{
			get
			{
				Trace("get_NamespaceURI");
				return _context.NameTable.Get(string.Empty);
			}
		}

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.NameTable" /> for details.
		/// </summary>
		public override System.Xml.XmlNameTable NameTable
		{
			get
			{
				Trace("get_NameTable");
				return _context.NameTable;
			}
		}

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.MoveTo" /> for details.
		/// </summary>
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

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.MoveToAttribute" /> for details.
		/// </summary>
		public override bool MoveToAttribute(string localName, string namespaceURI)
		{
			Trace("MoveToAttribute");
			return false;
		}

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.MoveToFirst" /> for details.
		/// </summary>
		/// <remarks>Not supported.</remarks>
		public override bool MoveToFirst()
		{
			Trace("MoveToFirst");
			return false;
		}

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.MoveToFirstAttribute" /> for details.
		/// </summary>
		public override bool MoveToFirstAttribute()
		{
			Trace("MoveToFirstAttribute");
			return false;
		}

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.MoveToFirstChild" /> for details.
		/// </summary>
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

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.MoveToFirstNamespace" /> for details.
		/// </summary>
		public override bool MoveToFirstNamespace(System.Xml.XPath.XPathNamespaceScope namespaceScope)
		{
			Trace("MoveToFirstNamespace");
			return false;
		}

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.MoveToId" /> for details.
		/// </summary>
		/// <remarks>Not supported.</remarks>
		public override bool MoveToId(string id)
		{
			Trace("MoveToId");
			return false;
		}

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.MoveToNamespace(string)" /> for details.
		/// </summary>
		public override bool MoveToNamespace(string name)
		{
			Trace("MoveToNamespace");
			return false;
		}

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.MoveToNext" /> for details.
		/// </summary>
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

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.MoveToNextAttribute" /> for details.
		/// </summary>
		public override bool MoveToNextAttribute()
		{
			Trace("MoveToNextAttribute");
			return false;
		}

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.MoveToNextNamespace" /> for details.
		/// </summary>
		public override bool MoveToNextNamespace(System.Xml.XPath.XPathNamespaceScope namespaceScope)
		{
			Trace("MoveToNextNamespace");
			return false;
		}

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.MoveToParent" /> for details.
		/// </summary>
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

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.MoveToPrevious" /> for details.
		/// </summary>
		/// <remarks>Not supported.</remarks>
		public override bool MoveToPrevious()
		{
			Trace("MoveToPrevious");
			return false;
		}

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.MoveToRoot" /> for details.
		/// </summary>
		public override void MoveToRoot()
		{
			Trace("MoveToRoot");
			_state = _root;
		}

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.NodeType" /> for details.
		/// </summary>
		public override System.Xml.XPath.XPathNodeType NodeType
		{
			get
			{
				Trace("get_NodeType");
				return _state.NodeType;
			}
		}

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.Value" /> for details.
		/// </summary>
		public override string Value
		{
			get
			{
				Trace("get_Value");
				return _state.Value;
			}
		}

		/// <summary>
		/// The current object.
		/// </summary>
		public object Node
		{
			get
			{
				return _state.Node;
			}
		}

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.XmlLang" /> for details.
		/// </summary>
		public override string XmlLang
		{
			get
			{
				return _lang;
			}
		}

		/// <summary>
		/// See <see cref="System.Xml.XPath.XPathNavigator.Prefix" /> for details.
		/// </summary>
		/// <remarks>Always empty.</remarks>
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
