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
using System.Threading;
using NUnit.Framework;
using Bamboo.Prevalence;

namespace Bamboo.Prevalence.Tests
{
	/// <summary>
	/// Tests the multithreaded behavior of PrevalenceEngine
	/// </summary>
	[TestFixture]
	public class MultiThreadedTests : AbstractAddingSystemTest
	{		
		protected override PrevalenceEngine CreateEngine()
		{
			return PrevalenceActivator.CreateEngine(PrevalentSystemType, PrevalenceBase, true);
		}

		[Test]
		public void TestMultiThreadedWrites()
		{
			ClearPrevalenceBase();
			CrashRecover();

			AssertTotal(0);

			Thread[] threads = new Thread[20];
			for (int i = 0; i<threads.Length; ++i)
			{
				threads[i] = new Thread(new ThreadStart(ExecuteAddCommand));
			}

			Start(threads);
			Join(threads);			

			CrashRecover();

			// 20 threads adding the value 10, 10 times
			AssertTotal(20*10*20);
		}

		private void ExecuteAddCommand()
		{	
			for (int i = 0; i<20; ++i)
			{
				ExecuteCommand(new AddCommand(10));
				Thread.Sleep(100);
			}
		}

		private void Join(Thread[] threads)
		{
			foreach (Thread t in threads)
			{
				t.Join();
			}
		}

		private void Start(Thread[] threads)
		{
			foreach (Thread t in threads)
			{
				t.Start();			
			}
		}
	}
}
