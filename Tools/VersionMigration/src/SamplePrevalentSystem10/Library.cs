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
