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

namespace Bamboo.Prevalence
{
	/// <summary>
	/// Models a command object. Command objects are
	/// the entities that apply changes to the 
	/// state of a prevalent system. If your object doesn't
	/// change the state of the system you should consider
	/// implementing it as a query object (<see cref="IQuery"/>).
	/// </summary>
	/// <remarks>
	/// In addition to implementing this interface
	/// the command object must also be serializable
	/// (command objects are written to a log before execution).
	/// <br />
	/// Also, the use of explicit interface implementation is
	/// advised, see the example below. Explicit interface
	/// implementation prevents a user from accidentally calling
	/// the Execute method.
	/// </remarks>
	/// <example>
	/// <code language="C#">
	/// [Serializable]
	/// public class AddCommand : ICommand
	/// {
	///		private int _amount;
	///		
	///		public AddCommand(int amount)
	///		{
	///			_amount = amount;
	///		}
	///		
	///		/// The explicit interface implementation
	///		/// shown here is preferred since 
	///		/// it prevents the user from calling
	///		/// the Execute method directly.
	///		object ICommand.Execute(PrevalentSystem system)
	///		{
	///			((AddingSystem)system).Add(amount);
	///		}
	///	}
	/// </code>
	/// </example>
	public interface ICommand
	{
		/// <summary>
		/// Executes the command logic.
		/// </summary>
		/// <remarks>
		/// You must ensure that if this method
		/// throws any exceptions it will not
		/// change the system state.
		/// See <see cref="PrevalenceEngine.ExecuteCommand"/>
		/// for details.
		/// </remarks>
		/// <param name="system">the prevalent system</param>
		/// <returns>command defined return value</returns>
		object Execute(IPrevalentSystem system);
	}
}
