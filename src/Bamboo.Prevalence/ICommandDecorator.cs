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
