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
	/// Decorates a command before it is saved to
	/// the command log. This interface can be
	/// implemented only by attributes that should
	/// be applied to a prevalent system class.
	/// </summary>
	/// <remarks>
	/// Before a command gets written to the command
	/// log the PrevalenceEngine class allows it to
	/// be decorated in order to preserve any context
	/// information that its execution might be sensitive
	/// to. A good example would be a command that is 
	/// dependent upon the principal associated to
	/// the running thread. If this command is to be
	/// successfully re-executed at system recovery time,
	/// the principal must be saved.<br />
	/// See <see cref="Bamboo.Prevalence.Attributes.PrincipalSensitiveAttribute"/>.
	/// </remarks>
	public interface ICommandDecorator
	{
		/// <summary>
		/// Decorates the command.
		/// </summary>
		/// <returns>a new command object or command</returns>
		ICommand Decorate(ICommand command);
	}
}
