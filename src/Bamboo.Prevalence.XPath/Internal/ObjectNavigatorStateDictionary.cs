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
	internal class ObjectNavigatorStateDictionary : ObjectNavigatorState
	{
		IDictionary _dictionary;

		IList _children;		

		internal ObjectNavigatorStateDictionary(ObjectNavigationContext context, ObjectNavigatorState parent, object node, string name) :
			base(context, parent, node, name)
		{
			_dictionary = (IDictionary)node;
			_children = new ArrayList(_dictionary.Keys);
		}

		internal override XPathNodeType NodeType
		{
			get
			{
				return XPathNodeType.Element;
			}
		}

		internal override ObjectNavigatorState MoveToFirstChild()
		{		
			if (_children.Count > 0)
			{
				return CreateElementState(_context, this, _dictionary[_children[0]], _children[0].ToString());
			}
			return null;
		}

		internal override ObjectNavigatorState MoveToNext()
		{			
			return _parent.MoveToChild(_index + 1);
		}

		internal override ObjectNavigatorState MoveToChild(int index)
		{
			if (index < _children.Count)
			{
				object child = _children[index];
				ObjectNavigatorState state = CreateElementState(_context, this, _dictionary[child], child.ToString());
				state.Index = index;
				return state;
			}
			return null;
		}
	}

}