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
