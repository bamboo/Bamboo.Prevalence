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
using System.Diagnostics;
using System.IO;
using System.Threading;
using Bamboo.Prevalence;

namespace Bamboo.Prevalence.Util
{
	/// <summary>
	/// Takes periodic snapshots of a PrevalenceEngine instance
	/// and applies a clean up policy to the prevalence base folder.
	/// </summary>
	/// <example>
	/// <code>
	/// SnapshotTaker _taker;
	/// 
	/// void Application_Start()
	/// {
	///		_engine = PrevalenceActivactor.CreateEngine(typeof(FooSystem), "c:\\foosystem");
	///		_taker = new SnapshotTaker(_engine, TimeSpan.FromHours(12));
	/// }
	/// 
	/// void Application_End()
	/// {
	///		_taker.Dispose();
	/// }
	/// </code>
	/// </example>
	public class SnapshotTaker : System.IDisposable
	{		
		Timer _timer;

		ICleanUpPolicy _cleanUpPolicy;

		/// <summary>
		/// Creates a new SnapshotTaker for the PrevalenceEngine engine
		/// with a period between snapshots equals to period but with no file
		/// cleanup policy (no files will be deleted from the prevalence base folder).
		/// </summary>
		/// <param name="engine">prevalence engine to take snapshots from</param>
		/// <param name="period">period between snapshots</param>
		public SnapshotTaker(PrevalenceEngine engine, TimeSpan period) : this(engine, period, NullCleanUpPolicy.Default)
		{			
		}

		/// <summary>
		/// Creates a new SnapshotTaker for the PrevalenceEngine engine
		/// with a period between snapshots equals to period. Files
		/// will be removed according to the specified cleanUpPolicy object.
		/// </summary>
		/// <param name="engine">prevalence engine to take snapshots from</param>
		/// <param name="period">period between snapshots</param>
		/// <param name="cleanUpPolicy">clean up policy</param>
		public SnapshotTaker(PrevalenceEngine engine, TimeSpan period, ICleanUpPolicy cleanUpPolicy)
		{
			if (null == engine)
			{
				throw new ArgumentNullException("engine");
			}			

			if (null == cleanUpPolicy)
			{
				throw new ArgumentNullException("cleanUpPolicy");
			}

			_cleanUpPolicy = cleanUpPolicy;
			_timer = new Timer(new TimerCallback(OnTimer), engine, period, period);			
		}

		/// <summary>
		/// Cancels the snapshot taking process.
		/// </summary>
		public void Dispose()
		{
			lock (this)
			{
				if (null != _timer)
				{
					_timer.Dispose();
					_timer = null;						
				}				
			}
		}

		void OnTimer(object state)
		{	
			PrevalenceEngine engine = (PrevalenceEngine)state;
			try
			{				
				engine.TakeSnapshot();
				DeleteFiles(_cleanUpPolicy.SelectFiles(engine));
			}
			catch (Exception /*ignored*/)
			{				
				// a callback?
			}
		}

		void DeleteFiles(FileInfo[] files)
		{
			foreach (FileInfo fi in files)
			{
				fi.Delete();
			}
		}
	}
}
