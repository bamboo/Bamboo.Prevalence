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
using System.Globalization;
using NUnit.Framework;
using Bamboo.Prevalence.VersionMigration;

namespace Bamboo.Prevalence.VersionMigration.Tests
{
	/// <summary>
	/// Test cases for MigrationPlan.
	/// </summary>
	[TestFixture]
	public class MigrationPlanTests : Assertion
	{		
		[Test]
		public void TestMigrationPlanLoad()
		{
			MigrationPlan plan = MigrationPlan.Load("test-plan-1.xml");
			AssertNotNull("MigrationPlan.Load", plan);

			AssertNotNull("plan.Culture", plan.Culture);
			AssertEquals("plan.Culture", CultureInfo.CreateSpecificCulture("pt-br"), plan.Culture);
			
			TypeMappingCollection mappings = plan.TypeMappings;
			AssertNotNull("plan.TypeMappings", mappings);

			// 3 mappings counting the aliases...
			AssertEquals("mappings.Count", 3, mappings.Count);

			TypeMapping mapping = mappings["SamplePrevalentSystem.Title"];
			AssertNotNull("mapping", mapping);
			AssertEquals(string.Empty, mapping.AssemblyName);
			AssertEquals("mapping.FieldMappings.Count", 3, mapping.FieldMappings.Count);

			mapping = mappings["SamplePrevalentSystem.LibrarySystem"];
			AssertNotNull("mapping", mapping);
			AssertEquals(string.Empty, mapping.AssemblyName);

			AssertEquals(mapping, mappings["SamplePrevalentSystem.Library"]);
			
			AssertEquals(2, plan.Scripts.Count);
			
			Script s = plan.Scripts[0];
			AssertEquals("context", s.TargetObject);
			AssertEquals("AfterDeserialization", s.TargetEvent);
			AssertEquals("// Nothing here...", s.Code);
			AssertEquals("vb", s.Language);
			
			s = plan.Scripts[1];
			AssertEquals("c# must be the default language", "c#", s.Language);
			AssertEquals(1, s.Imports.Count);
			AssertEquals("SamplePrevalentSystem", s.Imports[0].Namespace);
		}
	}
}
