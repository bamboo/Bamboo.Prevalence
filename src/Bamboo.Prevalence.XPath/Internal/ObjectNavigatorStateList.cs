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
	internal class ObjectNavigatorStateList : ObjectNavigatorState
	{
		IList _children;		

		internal ObjectNavigatorStateList(ObjectNavigationContext context, ObjectNavigatorState parent, object node, string name) :
			base(context, parent, node, name)
		{
			_children = (IList)node;			
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
			return MoveToChild(0);
		}

		internal override ObjectNavigatorState MoveToNext()
		{			
			return _parent.MoveToChild(_index + 1);
		}

		internal override ObjectNavigatorState MoveToChild(int index)
		{
			while (index < _children.Count)
			{
				object child = _children[index];
				if (null != child)
				{
					ObjectNavigatorState state = CreateElementState(_context, this, child, child.GetType().Name);
					state.Index = index;
					return state;
				}
				++index;
			}
			return null;
		}
	}

}
