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
	/// Models a query object. Query objects are used to
	/// read the state of the system. If your object needs
	/// to change the state of the system you should implement
	/// it as a command object (<see cref="ICommand"/>).
	/// </summary>
	/// <remarks>
	/// Although you can use command objects to query the
	/// system or even access the system's state directly
	/// (through <see cref="PrevalenceEngine.PrevalentSystem"/>),
	/// using query objects presents some benefits:
	/// <list type="">	
	/// <item>
	/// <para>By accessing the state of the system directly from
	/// your code you are responsible for synchronization (the
	/// prevalence engine guarantees serial command execution but
	/// if you access the system state directly you must make sure that
	/// no commands are changing it behind your back)
	/// </para>
	/// </item>
	/// <item>
	/// <para>If you later decide to access your prevalent system
	/// remotely, query objects are readily usable but any code
	/// that directly access the system's state through
	/// <see cref="PrevalenceEngine.PrevalentSystem" /> is not
	/// </para>
	/// </item>
	/// <item>
	/// <para>
	/// Unlike command objects, query objects are <b>NOT</b> saved
	/// to the command log thus yeilding better performance and reducing
	/// resource consumption
	/// </para>
	/// </item>
	/// <item>
	/// <para>And the best reason: multiple query objects
	/// are allowed to execute
	/// at the same time - in paralel -  while command objects
	/// have their execution serialized (see <see cref="PrevalenceEngine"/>
	/// remarks for details)</para>
	/// </item>
	/// </list>
	/// </remarks>
	/// <example>
	/// <code language="C#">
	/// public class QueryTotal : Bamboo.Prevalence.IQuery
	/// {
	///		object Bamboo.Prevalence.IQuery.Execute(Bamboo.Prevalence.IPrevalentSystem system)
	///		{
	///			return ((AddingSystem)system).Total;
	///		}
	///	}
	///	</code>
	/// </example>
	public interface IQuery
	{
		/// <summary>
		/// Executes the query logic.
		/// </summary>
		/// <param name="system">the prevalent system</param>
		/// <returns>query defined return value</returns>
		object Execute(Bamboo.Prevalence.IPrevalentSystem system);
	}
}
