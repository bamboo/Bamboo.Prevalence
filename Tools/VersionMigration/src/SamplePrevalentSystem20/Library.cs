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
	/// <summary>
	/// Base class introduced...
	/// </summary>
	[Serializable]
	public class NamedObject
	{
		protected string _name;

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
	}

	//
	// This version includes the following
	// modifications:
	//	* _summary renamed to description
	//	* _publishDate field added that must be initialized
	// with value now
	//	* reviews field that must be initialized with
	// a new instance
	[Serializable]
	public class Title : NamedObject
	{
		private Guid _id;		

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
