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
