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
		/// Assegura que WorkRecord não aceite argumentos
		/// inválidos para a hora de início e fim.
		/// </summary>
		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestRejectInvalidArguments()
		{
			new WorkRecord(_endTime, _startTime);
		}
	}
}
