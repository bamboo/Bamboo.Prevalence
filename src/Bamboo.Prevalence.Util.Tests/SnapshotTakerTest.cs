#region License
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
#endregion

using System;
using System.IO;
using System.Threading;
using NUnit.Framework;
using Bamboo.Prevalence;
using Bamboo.Prevalence.Tests;
using Bamboo.Prevalence.Util;

namespace Bamboo.Prevalence.Util.Tests
{
	/// <summary>
	/// Test cases for Bamboo.Prevalence.Util.SnapshotTaker.
	/// </summary>
	[TestFixture]
	public class SnapshotTakerTest : PrevalenceTestBase
	{			
		protected override System.Type PrevalentSystemType
		{
			get
			{
				return typeof(TransparentAddingSystem);
			}
		}

		IAddingSystem PrevalentSystem
		{
			get
			{
				return (IAddingSystem)_engine.PrevalentSystem;
			}
		}

		void Add(int amount)
		{
			PrevalentSystem.Add(amount);
		}		

		void AssertTotal(int expected)
		{
			AssertEquals(expected, PrevalentSystem.Total);
		}

		[SetUp]
		public override void SetUp()
		{
			base.SetUp();
			ClearPrevalenceBase();
			CrashRecover();
		}
		
		[Test]
		public void TestSnapshotTaker()
		{
			// create some logs...			
			Add(30); 
			_engine.HandsOffOutputLog();
			Add(20);

			TimeSpan period = TimeSpan.FromSeconds(2);
			SnapshotTaker taker = new SnapshotTaker(_engine, period);		

			// Let's wait for a snapshot to be taken
			Thread.Sleep(period + TimeSpan.FromSeconds(1));			
			AssertEquals(1, _engine.PrevalenceBase.GetFiles("*.snapshot").Length);

			// more log files...
			Add(20);

			// a second snapshot...
			Thread.Sleep(period);

			// ok, we're done.
			taker.Dispose();

			// Let's make sure no more snapshots will be
			// taken...
			Thread.Sleep(period + TimeSpan.FromSeconds(1));

			AssertEquals(2, _engine.PrevalenceBase.GetFiles("*.snapshot").Length);

			// sanity check
			CrashRecover();
			AssertTotal(70);			
		}

		[Test]
		public void TestCleanUpAllFilesPolicy()
		{
			// some log files...
			Add(20); // 1st log file
			CrashRecover();
			Add(30); // 2nd log file			
			_engine.TakeSnapshot(); // 1st snapshot
			Add(10); // 3rd log file
			_engine.TakeSnapshot(); // 2nd snapshot

			TimeSpan period = TimeSpan.FromMilliseconds(500);
			SnapshotTaker taker = new SnapshotTaker(_engine, period, CleanUpAllFilesPolicy.Default);
			Thread.Sleep(TimeSpan.FromMilliseconds(600));
			taker.Dispose();

			FileInfo[] files = _engine.PrevalenceBase.GetFiles("*.*");
			AssertEquals(1, files.Length);
			AssertEquals(".snapshot", files[0].Extension);

			// sanity check
			CrashRecover();
			AssertTotal(60);
		}
	}
}
