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
using System.Security.Principal;
using System.Threading;
using Bamboo.Prevalence;

namespace Bamboo.Prevalence.Attributes
{
	/// <summary>
	/// Marks a prevalent system class as sensitive to
	/// Thread.CurrentPrincipal. This attribute allows
	/// prevalent system classes to use the .NET
	/// Role Based security mechanism.
	/// </summary>
	/// <remarks>
	/// For this mechanism to work, Thread.CurrentPrincipal
	/// must be serializable. Further more, Thread.CurrentPrincipal
	/// serialization mechanism must permit the object to
	/// be successfully deserialized in a different process/time. <br />
	/// Unfortunately this restriction rules out
	/// System.Security.Principal.WindowsPrincipal.
	/// </remarks>
	/// <example>
	/// <code>
	/// <![CDATA[
	/// [Serializable]
	/// [PrincipalSensitive]
	/// public class MyPrevalentSystem : System.MarshalByRefObject
	/// {
	///		[PrincipalPermission(SecurityAction.Demand, Role="Administrator")]
	///		public void AddUser(User user)
	///		{
	///			...
	///		}
	///	}
	/// ]]>
	/// </code>
	/// </example>
	[Serializable]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]	
	public class PrincipalSensitiveAttribute : System.Attribute, ICommandDecorator
	{
		ICommand ICommandDecorator.Decorate(ICommand command)
		{
			IPrincipal principal = Thread.CurrentPrincipal;
			if (null == principal)
			{
				return command;
			}
			return new PrincipalRecoveryCommand(command, principal);
		}

		[Serializable]
		class PrincipalRecoveryCommand : ICommand
		{
			private ICommand _command;

			private IPrincipal _principal;

			public PrincipalRecoveryCommand(ICommand command, IPrincipal principal)
			{
				_command = command;
				_principal = principal;
			}

			object ICommand.Execute(object system)
			{
				IPrincipal saved = Thread.CurrentPrincipal;
				Thread.CurrentPrincipal = _principal;

				try
				{
					return _command.Execute(system);
				}
				finally
				{
					Thread.CurrentPrincipal = saved;
				}
			}
		}
	}
}
