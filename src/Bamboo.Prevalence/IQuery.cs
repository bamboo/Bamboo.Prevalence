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
	///		object Bamboo.Prevalence.IQuery.Execute(object system)
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
		object Execute(object system);
	}
}
