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
using Bamboo.Prevalence;
using Bamboo.Prevalence.Attributes;

namespace SamplePrevalentSystem
{
	[Serializable]
	public class Title
	{
		private Guid _id;

		private string _name;

		private string _summary;

		public Title()
		{
			_id = Guid.NewGuid();
		}		

		public Title(string name, string summary) : this()
		{
			_name = name;
			_summary = summary;
		}

		public Guid ID
		{
			get
			{
				return _id;
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

		public string Summary
		{
			get
			{
				return _summary;
			}

			set
			{
				_summary = value;
			}
		}
	}

	/// <summary>
	/// Library version 1.0
	/// </summary>
	[Serializable]
	[TransparentPrevalence]
	public class Library : System.MarshalByRefObject
	{	
		Hashtable _titles;

		public Library()
		{
			_titles = new Hashtable();
		}

		public void AddTitle(Title title)
		{
			_titles[title.ID] = title;
		}

		[Query]
		public IList GetTitles()
		{
			return ToArray(typeof(Title), _titles.Values);
		}

		private Array ToArray(System.Type type, ICollection items)
		{				
			Array array = Array.CreateInstance(type, items.Count);
			items.CopyTo(array, 0);
			return array;
		}
	}
}
