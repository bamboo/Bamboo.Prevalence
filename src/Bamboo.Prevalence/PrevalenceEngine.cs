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
using Bamboo.Prevalence.Implementation;

namespace Bamboo.Prevalence
{
	/// <summary>
	/// Object prevalence engine. Provides transparent
	/// object persistence to deterministic systems that
	/// implement the <see cref="IPrevalentSystem" /> interface.
	/// </summary>
	/// <remarks> 
	/// <p>Any change to the prevalent system must be represented by a
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
	/// way multiple query objects are allowed to execute in paralel
	/// and command objects are only allowed to execute one by one.
	/// </p>
	/// <p>
	/// The great news is that if you design your prevalent system
	/// in such a way that its state
	/// is only manipulated and queried through command and query 
	/// objects then you don't have to worry about synchronization
	/// anymore, to
	/// a simple example of such a system please refer to the
	/// Bamboo.Prevalence.Tests project.
	/// </p>
	/// </remarks>
	public class PrevalenceEngine
	{
		private IPrevalentSystem _system;		

		private CommandLogWriter _commandLog;

		private ReaderWriterLock _lock;		

		private static readonly System.LocalDataStoreSlot _sharedObjectSlot = Thread.AllocateDataSlot();

		/// <summary>
		/// Creates a new prevalence engine for the prevalent system
		/// type specified by the systemType argument.
		/// </summary>
		/// <remarks>
		/// The prevalence log files will be read from/written to the
		/// prevalenceBase directory.<br />
		/// If the directory does not exist it will be created.<br />
		/// If there are any log files already in the directory they will
		/// be used to restore the state of the system.<br />
		/// </remarks>
		/// <param name="systemType">prevalent system type, the type
		/// must implement the IPrevalentSystem interface and be serializable</param>
		/// <param name="prevalenceBase">directory where to store log files</param>
		public PrevalenceEngine(System.Type systemType, string prevalenceBase)
		{	
			AssertParameterNotNull("systemType", systemType);			

			CommandLogReader reader = new CommandLogReader(CheckPrevalenceBase(prevalenceBase));
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
		/// The prevalent system.
		/// </summary>
		public IPrevalentSystem PrevalentSystem
		{
			get
			{
				return _system;
			}
		}

		/// <summary>
		/// The clock. See <see cref="IPrevalentSystem.Clock"/>
		/// for details.
		/// </summary>
		public AlarmClock Clock
		{
			get
			{
				return _system.Clock;
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
			AssertParameterNotNull("command", command);		

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
			AssertParameterNotNull("query", query);

			AcquireReaderLock();
			try
			{
				ShareCurrentObject();
				return query.Execute(_system);
			}
			finally
			{
				UnshareCurrentObject();
				ReleaseReaderLock();
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

		private DirectoryInfo CheckPrevalenceBase(string prevalenceBase)
		{
			AssertParameterNotNull("prevalenceBase", prevalenceBase);

			DirectoryInfo di = new DirectoryInfo(prevalenceBase);
			if (!di.Exists)
			{
				di.Create();
			}

			return di;
		}

		private void AssertParameterNotNull(string paramName, object parameter)
		{
			if (null == parameter)
			{
				throw new ArgumentNullException(paramName);
			}
		}

		private void RecoverSystem(System.Type systemType, CommandLogReader reader)
		{
			_system = reader.ReadLastSnapshot();			
			if (null == _system)
			{
				_system = (IPrevalentSystem)System.Activator.CreateInstance(systemType);
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
