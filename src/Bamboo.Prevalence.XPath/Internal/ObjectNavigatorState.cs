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
using System.Xml;
using System.Xml.XPath;

namespace Bamboo.Prevalence.XPath.Internal
{
	internal abstract class ObjectNavigatorState
	{
		protected int _index;

		protected string _name;

		protected ObjectNavigatorState _parent;		

		protected object _node;

		protected ObjectNavigationContext _context;

		public ObjectNavigatorState(ObjectNavigationContext context, ObjectNavigatorState parent, object node, string name)
		{			
			_parent = parent;
			_context = context;
			_node = node;
			_name = _context.NameTable.Add(name);
			_index = 0;
		}

		internal int Index
		{
			get
			{
				return _index;
			}

			set
			{
				_index = value;
			}
		}

		internal virtual bool IsSamePosition(ObjectNavigatorState other)
		{
			return other._node == _node &&
				other._index == _index &&
				other._name == _name &&
				other._parent == _parent;
		}

		internal virtual string Name
		{
			get
			{
				return _name;
			}
		}

		internal virtual string Value
		{
			get
			{
				return _node.ToString();
			}
		}

		internal ObjectNavigatorState Parent
		{
			get
			{
				return _parent;
			}
		}

		internal object Node
		{
			get
			{
				return _node;
			}
		}

		internal abstract XPathNodeType NodeType
		{
			get;
		}

		internal virtual ObjectNavigatorState MoveToFirstChild()
		{
			return null;
		}

		internal virtual ObjectNavigatorState MoveToNext()
		{
			return null;
		}

		internal virtual ObjectNavigatorState MoveToChild(int index)
		{
			return null;
		}

		internal static ObjectNavigatorState CreateElementState(ObjectNavigationContext context, ObjectNavigatorState parent, object node, string name)
		{
			if (node is IDictionary)
			{
				return new ObjectNavigatorStateDictionary(context, parent, node, name);
			}
			else if (node is IList)
			{
				return new ObjectNavigatorStateList(context, parent, node, name);
			}
			else
			{
				return new ObjectNavigatorStateItem(context, parent, node, name);
			}
		}
	}
}
