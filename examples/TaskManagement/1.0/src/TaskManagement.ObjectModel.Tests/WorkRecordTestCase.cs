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
using NUnit.Framework;
using TaskManagement.ObjectModel;

namespace TaskManagement.ObjectModel.Tests
{
	/// <summary>
	/// Testes para a classe WorkRecord.
	/// </summary>
	[TestFixture]
	public class WorkRecordTestCase : Assertion
	{
		protected DateTime _startTime = new DateTime(2003, 6, 29, 14, 30, 0);
		
		protected DateTime _endTime = new DateTime(2003, 6, 29, 18, 56, 0);

		protected WorkRecord _record;

		[SetUp]
		public void SetUp()
		{
			_record = new WorkRecord(_startTime, _endTime);
		}

		[Test]
		public void TestConstruct()
		{
			AssertEquals("StartTime", _startTime, _record.StartTime);
			AssertEquals("EndTime", _endTime, _record.EndTime);
		}

		/// <summary>
		/// Assegura que WorkRecord n�o aceite argumentos
		/// inv�lidos para a hora de in�cio e fim.
		/// </summary>
		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestRejectInvalidArguments()
		{
			new WorkRecord(_endTime, _startTime);
		}
	}
}
