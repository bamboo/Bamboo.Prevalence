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
