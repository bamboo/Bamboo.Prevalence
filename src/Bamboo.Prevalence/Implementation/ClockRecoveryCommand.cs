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

namespace Bamboo.Prevalence.Implementation
{
	/// <summary>
	/// Command surrogate that allows the execution of a 
	/// command as if in a specific moment in time.
	/// </summary>
	[Serializable]
	internal sealed class ClockRecoveryCommand : Bamboo.Prevalence.ICommand
	{
		private Bamboo.Prevalence.ICommand _command;

		private System.DateTime _dateTime;

		public ClockRecoveryCommand(Bamboo.Prevalence.ICommand command, System.DateTime dateTime)
		{
			_command = command;
			_dateTime = dateTime;
		}

		object Bamboo.Prevalence.ICommand.Execute(object system)
		{
			PrevalenceEngine.Current.Clock.Recover(_dateTime);
			return _command.Execute(system);
		}
	}
}
