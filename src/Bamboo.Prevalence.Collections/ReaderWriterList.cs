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

namespace Bamboo.Prevalence.Collections
{
	/// <summary>
	/// Collection implementation synchronized by a <see cref="SerializableReaderWriterLock"/>
	/// object.
	/// All the methods in the collection can be safely used by multiple threads.
	/// </summary>
	[Serializable]
	public class ReaderWriterList : List
	{		
		protected SerializableReaderWriterLock _lock;

		#region public constructors
		public ReaderWriterList()
		{				
			_lock = new SerializableReaderWriterLock();
		}

		public ReaderWriterList(ICollection collection) : base(collection)
		{			
			_lock = new SerializableReaderWriterLock();
		}		

		#endregion

		#region locking methods
		public override void AcquireReaderLock()
		{
			_lock.AcquireReaderLock();
		}

		public override void ReleaseReaderLock()
		{
			_lock.ReleaseReaderLock();
		}

		public override void AcquireWriterLock()
		{
			_lock.AcquireWriterLock();
		}

		public override void ReleaseWriterLock()
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
		public override IDisposable WriterLock
		{
			get
			{
				return _lock.WriterLock;
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
		public override IDisposable ReaderLock
		{
			get
			{
				return _lock.ReaderLock;
			}
		}

		#endregion

		#region list overrides
		public override bool IsSynchronized
		{
			get
			{
				return true;
			}
		}
		#endregion
	}
}
