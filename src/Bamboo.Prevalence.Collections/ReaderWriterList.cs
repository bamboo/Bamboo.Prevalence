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
