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
using System.Threading;
using System.Runtime.Serialization;

namespace Bamboo.Prevalence.Collections
{
	/// <summary>
	/// A serializable version of a <see cref="ReaderWriterLock"  />
	/// </summary>
	[Serializable]
	public class SerializableReaderWriterLock
	{
		#region public static fields
		public static TimeSpan LockTimeout = TimeSpan.FromSeconds(3);
		#endregion

		#region protected fields
		[NonSerialized]
		protected ReaderWriterLock _lock;
		#endregion
		
		#region public helper classes
		/// <summary>
		/// Helper class to acquire/release a writer lock on a
		/// ReaderWriterList.
		/// </summary>
		public class WriterLockDisposer : IDisposable
		{
			protected ReaderWriterLock _lock;

			/// <summary>
			/// Acquires a writer lock on the list passed
			/// as argument.
			/// </summary>
			/// <param name="list"></param>
			public WriterLockDisposer(ReaderWriterLock lock_)
			{
				if (null == lock_)
				{
					throw new ArgumentNullException("lock_");
				}				

				lock_.AcquireWriterLock(LockTimeout);
				_lock = lock_;
			}

			/// <summary>
			/// Releases the writer lock on the list.
			/// </summary>
			public void Dispose()
			{
				if (null != _lock)
				{
					_lock.ReleaseWriterLock();
					_lock = null;
				}
			}
		}

		public class ReaderLockDisposer : IDisposable
		{
			protected ReaderWriterLock _lock;

			public ReaderLockDisposer(ReaderWriterLock lock_)
			{
				if (null == lock_)
				{
					throw new ArgumentNullException("lock_");
				}
				lock_.AcquireReaderLock(LockTimeout);
				_lock = lock_;
			}

			public void Dispose()
			{
				if (null != _lock)
				{
					_lock.ReleaseReaderLock();
					_lock = null;
				}
			}
		}
		#endregion
		
		public SerializableReaderWriterLock()
		{
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
				return new WriterLockDisposer(GetLock());
			}
		}

		/// <summary>
		/// <code>
		/// using (list.ReaderLock)
		/// {
		///		// do something...
		/// }
		/// </code>
		/// </summary>
		public IDisposable ReaderLock
		{
			get
			{
				return new ReaderLockDisposer(GetLock());
			}
		}
		
		public void AcquireReaderLock()
		{
			GetLock().AcquireReaderLock(LockTimeout);
		}

		public void ReleaseReaderLock()
		{
			GetLock().ReleaseReaderLock();
		}

		public void AcquireWriterLock()
		{
			GetLock().AcquireWriterLock(LockTimeout);
		}

		public void ReleaseWriterLock()
		{
			GetLock().ReleaseWriterLock();
		}		
		
		ReaderWriterLock GetLock()
		{
			lock (this)
			{
				if (null == _lock)
				{
					_lock = new ReaderWriterLock();
				}
				return _lock;
			}
		}

	}
}
