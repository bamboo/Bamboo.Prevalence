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

