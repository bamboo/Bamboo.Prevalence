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
using Bamboo.Prevalence.Tests.Model;
using NUnit.Framework;
using Bamboo.Prevalence;

namespace Bamboo.Prevalence.Tests
{
	/// <summary>
	/// Test case for testing Bamboo.Prevalence.PrevalenceEngine
	/// </summary>
	[TestFixture]
	public class PrevalenceEngineTest : AbstractPrevalenceEngineTest
	{			
		protected override void Add(int amount, int expectedTotal)
		{
			int actualTotal = (int)ExecuteCommand(new AddCommand(amount));
			AssertEquals("Add", expectedTotal, actualTotal);
		}		

		protected override void AssertTotal(int amount)
		{
			int total = (int)ExecuteQuery(new QueryTotal());
			AssertEquals("Total", amount, total);
		}

		protected override System.Type PrevalentSystemType
		{
			get
			{
				return typeof(AddingSystem);
			}
		}		
	}
}
