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
using System.IO;
using NUnit.Framework;
using Bamboo.Prevalence;

namespace Bamboo.Prevalence.Tests
{
	/// <summary>
	/// Base test case for testing Bamboo.Prevalence.PrevalenceEngine
	/// </summary>
	[TestFixture]
	public abstract class AbstractPrevalenceEngineTest : PrevalenceTestBase
	{			
		protected abstract void Add(int amount, int expectedTotal);

		protected abstract void AssertTotal(int total);		
		
		[SetUp]
		public override void SetUp()
		{	
			base.SetUp();
			ClearPrevalenceBase();
			_engine = CreateEngine();			
		}	

		[Test]
		public void TestPrevalenceEngine()
		{
			for (int mode=0; mode<2; ++mode)
			{
				SetUp();
				
				for (int pass=0; pass<10; ++pass)
				{
					int factor = 200*pass;

					Add(10, 10 + factor);
					Add(30, 40 + factor);
					CrashRecover();
					AssertTotal(40 + factor);

					Add(60, 100 + factor);
					CrashRecover();
					AssertTotal(100 + factor);

					Add(50, 150 + factor);
					if (mode == 1)
					{
						Snapshot();
					}
					CrashRecover();
					CrashRecover();
					AssertTotal(150 + factor);			

					CrashRecover();					
					if (mode == 1)
					{
						Snapshot();
					}
					CrashRecover();
					AssertTotal(150 + factor);
					Add(50, 200 + factor);

					CrashRecover();
					AssertTotal(200 + factor);
				}
			}
		}		

		[Test]
		public void TestExceptionThrowingCommand()
		{
			Add(20, 20);

			try
			{
				Add(-10, 0);
				Assert("Add(-10) should throw an exception!", false);
			}
			catch (ArgumentOutOfRangeException)
			{
			}

			CrashRecover();
			AssertTotal(20);
		}

		[Test]
		public void TestRecoverFromEmptySnapshot()
		{
			Snapshot();
			CrashRecover();
			AssertTotal(0);
		}

		/// <summary>
		/// This test makes sure that is possible
		/// to use the same command twice without
		/// any side effects.
		/// </summary>
		[Test]
		public void TestExecutingTheSameCommandTwice()
		{
			AddCommand command = new AddCommand(10);
			AssertEquals("AddCommand 10", 10, ExecuteCommand(command));

			command.Amount = 20;
			AssertEquals("AddCommand 20", 30, ExecuteCommand(command));

			CrashRecover();
			AssertTotal(30);
		}

		[Test]
		public void TestPrevalenceEngineCurrent()
		{
			CrashRecover();

			ExecuteCommand(new TestCurrentCommandAndQuery(Engine));
			ExecuteQuery(new TestCurrentCommandAndQuery(Engine));
		}

		[Test]
		public void TestNonSerializableCommand()
		{
			CrashRecover();

			ExecuteCommand(new AddCommand(20));
			AssertTotal(20);
			
			try
			{
				ExecuteCommand(new NonSerializableCommand());
			}
			catch (System.Runtime.Serialization.SerializationException)
			{
			}

			ExecuteCommand(new AddCommand(5));
			CrashRecover();
			AssertTotal(25);
			ExecuteCommand(new AddCommand(10));
			AssertTotal(35);
			CrashRecover();
			AssertTotal(35);
		}

		[Test]
		public void TestPausedState()
		{
			CrashRecover();
			ExecuteCommand(new AddCommand(10));
			AssertTotal(10);

			_engine.Pause();
			Assert(_engine.IsPaused);

			try
			{
				ExecuteCommand(new AddCommand(5));
				Fail("PausedEngineException expected!");
			}
			catch (PausedEngineException)
			{
			}

			AssertTotal(10);

			_engine.Resume();
			Assert(!_engine.IsPaused);

			ExecuteCommand(new AddCommand(5));
			AssertTotal(15);
		}

		[Test]
		public void TestObjectMethodsAreNotCommands()
		{
			CrashRecover();
			_engine.Pause();
			
			Type type = _engine.PrevalentSystem.GetType();			
		}
	}
}

