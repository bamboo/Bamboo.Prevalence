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
	/// A more convenient IList implementation.
	/// </summary>
	[Serializable]
	public class List : IList
	{
		protected static Random _random = new Random();

		#region DisposableObject implementation
		class DisposableObject : IDisposable
		{
			public static IDisposable Default = new DisposableObject();

			public void Dispose()
			{
			}
		}
		#endregion

		#region protected fields
		protected ArrayList _list;
		#endregion
		public List(int initialCapacity)
		{
			_list = new ArrayList(initialCapacity);
		}
		
		public List()
		{
			_list = new ArrayList();
		}

		public List(ICollection collection)
		{
			_list = new ArrayList(collection);
		}		

		public List(params object[] items)
		{
			_list = new ArrayList(items);
		}

		#region synchronization methods
		public virtual void AcquireReaderLock()
		{
		}

		public virtual void ReleaseReaderLock()
		{
		}

		public virtual void AcquireWriterLock()
		{
		}

		public virtual void ReleaseWriterLock()
		{
		}

		public virtual IDisposable ReaderLock
		{
			get
			{
				return DisposableObject.Default;
			}
		}

		public virtual IDisposable WriterLock
		{
			get
			{
				return DisposableObject.Default;
			}
		}
		#endregion

		#region non synchronized methods
		/// <summary>
		/// Allows bypassing the synchronization mechanism
		/// giving direct access to the underlying list.
		/// </summary>
		public IList InnerList
		{
			get
			{
				return _list;
			}
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

		public object[] ToShuffledArray()
		{			
			return (object[])ToShuffledArray(typeof(object));
		}
		
		public Array ToShuffledArray(Type elementType)
		{
			List copy = new List(ToArray());
			Array target = Array.CreateInstance(elementType, copy.Count);
			for (int i=0; i<target.Length; ++i)
			{
				target.SetValue(copy.PopAny(), i);
			}
			return target;
		}
		
		public List Reverse()
		{
			AcquireReaderLock();
			try
			{				
				List reversed = new List(_list);
				reversed._list.Reverse();
				return reversed;
			}
			finally
			{
				ReleaseReaderLock();
			}
		}

		/// <summary>
		/// Selects an random item from the
		/// list.
		/// </summary>
		/// <returns></returns>
		public object Choose()
		{
			AcquireReaderLock();
			try
			{
				if (_list.Count > 0)
				{
					lock (_random)
					{
						return _list[_random.Next(_list.Count)];
					}
				}
				return null;
			}
			finally
			{
				ReleaseReaderLock();
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		public Array Choose(int count)
		{
			if (_list.Count > count)
			{
				List copy = new List(_list);
				object[] items = new object[count];
				for (int i=0; i<count; ++i)
				{
					items[i] = copy.PopAny();
				}
				return items;
			}
			else
			{
				return ToShuffledArray();
			}
		}

		/// <summary>
		/// Selects a random item from the list and
		/// removes it.
		/// </summary>
		/// <returns>null if the list is empty or the item removed otherwise</returns>
		public object PopAny()
		{
			AcquireWriterLock();
			try
			{
				object item = null;

				if (_list.Count > 0)
				{		
					int index = -1;
					lock (_random)
					{
						index = _random.Next(_list.Count);
					}
					item = _list[index];
					_list.RemoveAt(index);
				}
				return item;
			}
			finally
			{
				ReleaseWriterLock();
			}
		}

		public Array ToSortedArray(IComparer comparer, Type returnItemType)
		{
			Array array = ToArray(returnItemType);
			Array.Sort(array, comparer);
			return array;
		}

		public object[] ToSortedArray(IComparer comparer)
		{
			return (object[])ToSortedArray(comparer, typeof(object));
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
			return -1 != IndexOf(predicate);
		}
		
		public object[] GetRange(int index, int count)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", index, "Index can't be negative!");
			}
			
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", count, "Count can't be negative!");
			}
			
			object[] range = null;
			AcquireReaderLock();
			try
			{				
				if (index > _list.Count - 1)
				{
					range = new object[0];
				}
				else
				{
					int avail = _list.Count - index;
					count = Math.Min(count, avail);
					range = new object[count];
					for (int i=0; i<count; ++i)
					{
						range[i] = _list[index + i];
					}
				}
			}
			finally
			{
				ReleaseReaderLock();
			}
			return range;
		}

		public object Find(Predicate predicate)
		{
			int index = IndexOf(predicate);
			return index != -1 ? _list[index] : null;
		}

		public int IndexOf(Predicate predicate)
		{
			if (null == predicate)
			{
				throw new ArgumentNullException("predicate");
			}

			AcquireReaderLock();
			try
			{
				for (int i=0; i<_list.Count; ++i)
				{
					if (predicate(_list[i]))
					{
						return i;
					}
				}
				return -1;
			}
			finally
			{
				ReleaseReaderLock();
			}			
		}

		public object[] Collect(Predicate predicate)
		{
			return (object[])Collect(predicate, typeof(object));
		}

		public System.Array Collect(Predicate predicate, Type returnItemType)
		{
			if (null == predicate)
			{
				throw new ArgumentNullException("predicate");
			}

			if (null == returnItemType)
			{
				throw new ArgumentNullException("returnItemType");
			}

			ArrayList items = new ArrayList();
			InnerCollect(items, predicate);			
			return items.ToArray(returnItemType);
		}

		public void Collect(IList list, Predicate predicate)
		{
			if (null == list)
			{
				throw new ArgumentNullException("list");
			}

			if (null == predicate)
			{
				throw new ArgumentNullException("predicate");
			}

			InnerCollect(list, predicate);
		}

		protected void InnerCollect(IList list, Predicate predicate)
		{
			AcquireReaderLock();
			try
			{
				foreach (object item in _list)
				{
					if (predicate(item))
					{
						list.Add(item);
					}
				}
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

		public virtual bool IsSynchronized
		{
			get
			{
				return false;
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
		
		public void AddRange(IList range)
		{
			if (null == range)
			{
				throw new ArgumentNullException("range");
			}
			
			if (0 == range.Count)
			{
				return;
			}
			
			AcquireWriterLock();
			try
			{
				_list.AddRange(range);
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
