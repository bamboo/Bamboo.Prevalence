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

namespace Bamboo.Prevalence.Implementation
{
	/// <summary>
	/// Command surrogate that allows the execution of a 
	/// command as if in a specific moment in time.
	/// </summary>
	[Serializable]
	internal sealed class ContextRecoveryCommand : Bamboo.Prevalence.ICommand
	{
		private Bamboo.Prevalence.ICommand _command;

		private System.DateTime _dateTime;		

		public ContextRecoveryCommand(Bamboo.Prevalence.ICommand command, System.DateTime dateTime)
		{
			_command = command;
			_dateTime = dateTime;			
		}
		
		internal DateTime DateTime
		{
			get
			{
				return _dateTime;
			}
		}
		
		internal ICommand Command
		{
			get
			{
				return _command;
			}
		}

		object Bamboo.Prevalence.ICommand.Execute(object system)
		{
			PrevalenceEngine.Current.Clock.Recover(_dateTime);
			return _command.Execute(system);
		}

		/// <summary>
		/// Delegate to the inner command ToString method.
		/// </summary>
		/// <returns>inner command's string representation</returns>
		public override string ToString()
		{
			return string.Format("{0}: {1}", _dateTime, _command);
		}
	}
}
