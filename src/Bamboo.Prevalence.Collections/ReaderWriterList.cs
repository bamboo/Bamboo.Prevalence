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
using System.Threading;
using System.Runtime.Serialization;

namespace Bamboo.Prevalence.Collections
{
	/// <summary>
	/// Maps an object to another.
	/// </summary>
	public delegate object Mapping(object value);

	/// <summary>
	/// An action that can be applied to an object.
	/// </summary>
	public delegate void Action(object value);

	/// <summary>
	/// An object test predicate.
	/// </summary>
	public delegate bool Predicate(object value);

	/// <summary>
	/// Collection implementation synchronized by a <see cref="ReaderWriterLock"/>
	/// object.
	/// All the methods in the collection can be safely used by multiple threads.
	/// </summary>
	[Serializable]
	public class ReaderWriterList : IList, IDeserializationCallback
	{
		#region public helper classes
		public class WriterLockDisposer : IDisposable
		{
			protected ReaderWriterList _list;

			public WriterLockDisposer(ReaderWriterList list)
			{
				if (null == list)
				{
					throw new ArgumentNullException("list");
				}				

				list.AcquireWriterLock();
				_list = list;
			}

			public void Dispose()
			{
				if (null != _list)
				{
					_list.ReleaseWriterLock();
					_list = null;
				}
			}
		}
		#endregion

		#region public static fields
		public static TimeSpan LockTimeout = TimeSpan.FromSeconds(3);
		#endregion

		#region protected fields
		protected ArrayList _list;

		protected ReaderWriterLock _lock;
		#endregion

		#region public constructors
		public ReaderWriterList()
		{		
			_list = new ArrayList();
			_lock = new ReaderWriterLock();
		}

		public ReaderWriterList(ICollection collection)
		{
			_list = new ArrayList(collection);
			_lock = new ReaderWriterLock();
		}		

		#endregion

		#region locking methods
		public void AcquireReaderLock()
		{
			_lock.AcquireReaderLock(LockTimeout);
		}

		public void ReleaseReaderLock()
		{
			_lock.ReleaseReaderLock();
		}

		public void AcquireWriterLock()
		{
			_lock.AcquireWriterLock(LockTimeout);
		}

		public void ReleaseWriterLock()
		{
			_lock.ReleaseWriterLock();
		}		

		/// <summary>
		/// Returns a write lock object.
		/// 
		/// This property allows the use of this idiom:
		/// <code>
		/// using (list.WriterLock)
		/// {
		///		// do something...
		/// }
		/// </code>
		/// </summary>
		public IDisposable WriterLock
		{
			get
			{
				return new WriterLockDisposer(this);
			}
		}

		#endregion
		
		#region Implementation of IDeserializationCallback
		public void OnDeserialization(object sender)
		{
			_lock = new ReaderWriterLock();
		}
		#endregion

		#region utility methods
		public void Sort()
		{
			AcquireWriterLock();
			try
			{
				_list.Sort();
			}
			finally
			{
				ReleaseWriterLock();
			}
		}

		public void Sort(IComparer comparer)
		{
			AcquireWriterLock();
			try
			{
				_list.Sort(comparer);
			}
			finally
			{
				ReleaseWriterLock();
			}
		}

		public System.Array ToArray(Type type)
		{
			AcquireReaderLock();
			try
			{
				return _list.ToArray(type);
			}
			finally
			{
				ReleaseReaderLock();
			}
		}

		public object[] ToArray()
		{
			AcquireReaderLock();
			try
			{
				return _list.ToArray();
			}
			finally
			{
				ReleaseReaderLock();
			}
		}

		public ReaderWriterList Clone()
		{
			return new ReaderWriterList(_list);
		}

		/// <summary>
		/// Maps every object in the list according
		/// to the mapping delegate in the context of
		/// a writer lock.
		/// </summary>
		/// <param name="mapping"></param>
		public void Map(Mapping mapping)
		{
			if (null == mapping)
			{
				throw new ArgumentNullException("mapping");
			}

			AcquireWriterLock();
			try
			{
				for (int i=0; i<_list.Count; ++i)
				{
					_list[i] = mapping(_list[i]);
				}
			}
			finally
			{
				ReleaseWriterLock();
			}
		}

		/// <summary>
		/// Maps every object in this list to an equivalent (Equals(item) returns
		/// true) in the list passed as argument. The entire mapping
		/// operation is accomplished in the context of a writer lock.
		/// </summary>
		/// <param name="list"></param>
		public void Map(IEnumerable list)
		{
			if (null == list)
			{
				throw new ArgumentNullException("list");
			}

			AcquireWriterLock();
			try
			{
				for (int i=0; i<_list.Count; ++i)
				{
					object existing = _list[i];
					if (null != existing)
					{
						foreach (object item in list)
						{
							if (existing.Equals(item))
							{
								_list[i] = item;
								break;
							}
						}
					}
				}
			}
			finally
			{
				ReleaseWriterLock();
			}
		}

		/// <summary>
		/// Applies an action to every object in the list in 
		/// the context of a writer lock.
		/// </summary>
		/// <param name="action"></param>
		public void Apply(Action action)
		{
			if (null == action)
			{
				throw new ArgumentNullException("action");
			}

			AcquireWriterLock();
			try
			{
				foreach (object item in _list)
				{
					action(item);
				}
			}
			finally
			{
				ReleaseWriterLock();
			}
		}

		public bool Any(Predicate predicate)
		{
			if (null == predicate)
			{
				throw new ArgumentNullException("predicate");
			}

			AcquireReaderLock();
			try
			{
				foreach (object item in _list)
				{
					if (predicate(item))
					{
						return true;
					}
				}
				return false;
			}
			finally
			{
				ReleaseReaderLock();
			}			
		}

		#endregion

		#region Implementation of ICollection
		public int Count
		{
			get
			{
				AcquireReaderLock();
				try
				{
					return _list.Count;
				}
				finally
				{
					ReleaseReaderLock();
				}
			}
		}		

		public void CopyTo(System.Array array, int index)
		{
			AcquireReaderLock();
			try
			{
				_list.CopyTo(array, index);
			}
			finally
			{
				ReleaseReaderLock();
			}
		}

		public bool IsSynchronized
		{
			get
			{
				return true;
			}
		}

		public object SyncRoot
		{
			get
			{
				return null;
			}
		}
		#endregion

		#region Implementation of IEnumerable
		public System.Collections.IEnumerator GetEnumerator()
		{
			return ToArray().GetEnumerator();
		}
		#endregion

		#region Implementation of IList
		public void RemoveAt(int index)
		{
			AcquireWriterLock();
			try
			{
				_list.RemoveAt(index);
			}
			finally
			{
				ReleaseWriterLock();
			}
		}

		public void Insert(int index, object value)
		{
			AcquireWriterLock();
			try
			{
				_list.Insert(index, value);
			}
			finally
			{
				ReleaseWriterLock();
			}
		}

		public void Remove(object value)
		{
			AcquireWriterLock();
			try
			{
				_list.Remove(value);
			}
			finally
			{
				ReleaseWriterLock();
			}
		}

		public bool Contains(object value)
		{
			AcquireReaderLock();
			try
			{
				return _list.Contains(value);
			}
			finally
			{
				ReleaseReaderLock();
			}
		}

		public void Clear()
		{
			AcquireWriterLock();
			try
			{
				_list.Clear();
			}
			finally
			{
				ReleaseWriterLock();
			}
		}

		public int IndexOf(object value)
		{
			AcquireReaderLock();
			try
			{
				return _list.IndexOf(value);
			}
			finally
			{
				ReleaseReaderLock();
			}
		}

		public int Add(object value)
		{
			AcquireWriterLock();
			try
			{
				return _list.Add(value);
			}
			finally
			{
				ReleaseWriterLock();
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public object this[int index]
		{
			get
			{
				AcquireReaderLock();
				try
				{
					return _list[index];
				}
				finally
				{
					ReleaseReaderLock();
				}
			}

			set
			{
				AcquireWriterLock();
				try
				{
					_list[index] = value;
				}
				finally
				{
					ReleaseWriterLock();
				}
			}
		}

		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}
		#endregion
	}
}
