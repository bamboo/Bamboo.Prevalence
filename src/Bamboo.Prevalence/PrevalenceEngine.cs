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
using System.IO;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Bamboo.Prevalence.Implementation;

namespace Bamboo.Prevalence
{
	/// <summary>
	/// Object prevalence engine. Provides transparent
	/// object persistence to deterministic systems.
	/// </summary>
	/// <remarks> 
	/// <p>The prevalent system class must be serializable 
	/// and deterministic: two instances of the same
	/// class when exposed to the same set of commands and queries
	/// must reach the same final state and yield the same query
	/// results.
	/// </p>
	/// <p>In order to be deterministic, the prevalent system class
	/// must use <see cref="PrevalenceEngine.Clock"/> or
	/// <see cref="PrevalenceEngine.Now"/> for all its date/time
	/// related functions.
	/// </p>
	/// <p>
	/// Any change to the prevalent system must be represented by a
	/// serializable command object (<see cref="ICommand"/>)
	/// executed through <see cref="ExecuteCommand" />.
	/// </p>
	/// <p>Queries to the prevalent system are best implemented
	/// as query objects (<see cref="IQuery"/>) executed through
	/// <see cref="ExecuteQuery"/>.
	/// </p>
	/// <p>
	/// The prevalence engine employs a reader/writer lock strategy
	/// to coordenate the work of command and query objects - that
	/// way multiple query objects are allowed to execute in paralell
	/// and command objects are only allowed to execute one by one.
	/// </p>
	/// <p>
	/// The great news is that if you design your prevalent system
	/// in such a way that its state
	/// is only manipulated and queried through command and query 
	/// objects you don't have to worry about synchronization,
	/// to a simple example of such a system please refer to the
	/// Bamboo.Prevalence.Tests project.
	/// </p>
	/// <p>
	/// Optionally, if you don't like the idea of designing your system around
	/// command and query objects and would rather use simple method calls
	/// onto your prevalent system class, take a look
	/// at <see cref="PrevalenceActivator.CreateTransparentEngine" />
	/// or <see cref="Bamboo.Prevalence.Attributes.TransparentPrevalenceAttribute"/>.
	/// </p>
	/// </remarks>
	public class PrevalenceEngine
	{
		/// <summary>
		/// the prevalent system.
		/// </summary>
		protected object _system;		

		private CommandLogWriter _commandLog;

		private ReaderWriterLock _lock;

		private AlarmClock _clock;

		private static readonly System.LocalDataStoreSlot _sharedObjectSlot = Thread.AllocateDataSlot();
		
		/// <summary>
		/// See <see cref="PrevalenceActivator.CreateEngine(System.Type, string)"/>
		/// </summary>
		/// <param name="systemType"></param>
		/// <param name="prevalenceBase"></param>
		/// <param name="formatter"></param>
		internal PrevalenceEngine(System.Type systemType, string prevalenceBase, BinaryFormatter formatter)
		{				
			_clock = new AlarmClock();

			CommandLogReader reader = new CommandLogReader(CheckPrevalenceBase(prevalenceBase), formatter);
			RecoverSystem(systemType, reader);
			_commandLog = reader.ToWriter();
			_lock = new ReaderWriterLock();			
		}

		/// <summary>
		/// Returns the current executing PrevalenceEngine when 
		/// called inside ICommand and IQuery objects Execute method.
		/// </summary>
		public static PrevalenceEngine Current
		{
			get
			{
				return Thread.GetData(_sharedObjectSlot) as PrevalenceEngine;
			}
		}

		/// <summary>
		/// The current system time.
		/// </summary>
		public static System.DateTime Now
		{
			get
			{
				return Current.Clock.Now;
			}
		}

		/// <summary>
		/// The prevalent system.
		/// </summary>
		public virtual object PrevalentSystem
		{
			get
			{
				return _system;
			}
		}

		/// <summary>
		/// The clock.
		/// </summary>
		public AlarmClock Clock
		{
			get
			{
				return _clock;
			}
		}

		/// <summary>
		/// Executes a serializable command object. If the
		/// command throws an exception it WILL NOT be
		/// saved to the output log therefore any 
		/// exception throwing command MUST NOT cause
		/// any change to the system state to occur.
		/// </summary>
		/// <param name="command">serializable command object that will be executed</param>
		/// <returns>the ICommand.Execute return value</returns>
		public object ExecuteCommand(ICommand command)
		{				
			Assertion.AssertParameterNotNull("command", command);		

			AcquireWriterLock();
			try
			{
				return DoExecuteCommand(command);
			}
			finally
			{
				ReleaseWriterLock();
			}			
		}

		/// <summary>
		/// Executes a query object against the prevalent system.
		/// </summary>
		/// <param name="query">the query object</param>
		/// <returns>query object return value</returns>
		public object ExecuteQuery(Bamboo.Prevalence.IQuery query)
		{
			Assertion.AssertParameterNotNull("query", query);

			BeforeQuery();

			try
			{				
				return query.Execute(_system);
			}
			finally
			{				
				AfterQuery();
			}
		}

		/// <summary>
		/// Writes a snapshot of the current state
		/// of the system to disk to speed up the next
		/// start up.
		/// </summary>
		public void TakeSnapshot()
		{
			AcquireReaderLock();
			
			try
			{				
				_commandLog.TakeSnapshot(_system);
			}
			finally
			{
				ReleaseReaderLock();
			}
		}

		/// <summary>
		/// Closes the output log file so that it can be
		/// read/copied/deleted. Useful for testing.
		/// </summary>
		public void HandsOffOutputLog()
		{
			AcquireReaderLock();

			try
			{
				_commandLog.CloseOutputLog();
			}
			finally
			{
				ReleaseReaderLock();
			}
		}

		internal void BeforeQuery()
		{
			ShareCurrentObject();
			AcquireReaderLock();
		}

		internal void AfterQuery()
		{
			ReleaseReaderLock();
			UnshareCurrentObject();
		}

		private DirectoryInfo CheckPrevalenceBase(string prevalenceBase)
		{

			DirectoryInfo di = new DirectoryInfo(prevalenceBase);
			if (!di.Exists)
			{
				di.Create();
			}

			return di;
		}		

		private void RecoverSystem(System.Type systemType, CommandLogReader reader)
		{
			_system = reader.ReadLastSnapshot();			
			if (null == _system)
			{
				_system = System.Activator.CreateInstance(systemType);
			}
			RecoverCommands(reader);
		}
		
		private void RecoverCommands(CommandLogReader reader)
		{
			ShareCurrentObject();

			try
			{
				Clock.Pause();

				foreach (ICommand command in reader)
				{
					command.Execute(_system);
				}		
                
				Clock.Resume();
			}
			finally
			{
				UnshareCurrentObject();
			}
		}

		private object DoExecuteCommand(ICommand command)
		{
			object returnValue;
			
			Clock.Pause();
			_commandLog.WriteCommand(new ClockRecoveryCommand(command, Clock.Now));
			try
			{		
				ShareCurrentObject();
				returnValue = command.Execute(_system);					
			}
			catch
			{
				_commandLog.UndoWriteCommand();
				throw;
			}
			finally
			{
				UnshareCurrentObject();
				Clock.Resume();
			}

			return returnValue;
		}

		private void AcquireReaderLock()
		{
			_lock.AcquireReaderLock(-1);
		}

		private void ReleaseReaderLock()
		{
			_lock.ReleaseReaderLock();
		}

		private void AcquireWriterLock()
		{
			_lock.AcquireWriterLock(-1);
		}

		private void ReleaseWriterLock()
		{
			_lock.ReleaseWriterLock();
		}

		private void ShareCurrentObject()
		{
			Thread.SetData(_sharedObjectSlot, this);
		}

		private void UnshareCurrentObject()
		{
			Thread.SetData(_sharedObjectSlot, null);
		}
	}
}
