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
	//
	// This version includes the following
	// modifications:
	//	* _summary renamed to description
	//	* _publishDate field added that must be initialized
	// with value now
	//	* reviews field that must be initialized with
	// a new instance
	[Serializable]
	public class Title
	{
		private Guid _id;

		private string _name;

		private string _description;
		
		private System.DateTime _publishDate;

		private ArrayList _reviews;

		public Title()
		{
			_id = Guid.NewGuid();
			_reviews = new ArrayList();
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

		public string Description
		{
			get
			{
				return _description;
			}

			set
			{
				_description = value;
			}
		}

		public DateTime PublishDate
		{
			get
			{
				return _publishDate;
			}

			set
			{
				_publishDate = value;
			}
		}

		public IList Reviews
		{
			get
			{
				return _reviews.ToArray(typeof(TitleReview));
			}
		}

		internal void AddReview(TitleReview review)
		{
			_reviews.Add(review);
		}
	}

	[Serializable]
	public class TitleReview
	{
		private string _reviewer;

		private string _comments;

		private DateTime _creationTime;

		public TitleReview(string reviewer, string comments)
		{
			_reviewer = reviewer;
			_comments = comments;
			_creationTime = DateTime.Now;
		}

		public string Reviewer
		{
			get
			{
				return _reviewer;
			}
		}

		public string Comments
		{
			get
			{
				return _comments;
			}
		}

		public DateTime CreationTime
		{
			get
			{
				return _creationTime;
			}
		}
	}

	/// <summary>
	/// Library version 2.0
	/// * Library renamed to LibrarySystem
	/// </summary>
	[Serializable]
	[TransparentPrevalence]
	public class LibrarySystem : System.MarshalByRefObject
	{	
		Hashtable _titles;

		public LibrarySystem()
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

		public void AddReview(Guid titleID, TitleReview review)
		{
			Title title = _titles[titleID] as Title;
			if (null == title)
			{
				throw new ApplicationException("Title " + titleID + " not found!");
			}
			title.AddReview(review);
		}

		private Array ToArray(System.Type type, ICollection items)
		{				
			Array array = Array.CreateInstance(type, items.Count);
			items.CopyTo(array, 0);
			return array;
		}
	}
}
