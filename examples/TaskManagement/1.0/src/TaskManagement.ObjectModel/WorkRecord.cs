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

namespace TaskManagement.ObjectModel
{
	/// <summary>
	/// Um registro de horas trabalhadas.
	/// </summary>
	[Serializable]
	public class WorkRecord
	{		
		protected DateTime _startTime;

		protected DateTime _endTime;

		/// <summary>
		/// Cria um novo registro para o per�odo indicado.
		/// </summary>
		/// <param name="startTime">in�cio do per�odo</param>
		/// <param name="endTime">fim do per�odo</param>
		public WorkRecord(DateTime startTime, DateTime endTime)
		{
			if (endTime < startTime)
			{
				throw new ArgumentException("A hora de fim deve ser maior que a hora de in�cio!", "endTime");
			}

			_startTime = startTime;
			_endTime = endTime;
		}

		/// <summary>
		/// In�cio do per�odo trabalhado.
		/// </summary>
		public DateTime StartTime
		{
			get
			{
				return _startTime;
			}
		}

		/// <summary>
		/// Fim do per�odo trabalhado.
		/// </summary>
		public DateTime EndTime
		{
			get
			{
				return _endTime;
			}
		}
	}
}
