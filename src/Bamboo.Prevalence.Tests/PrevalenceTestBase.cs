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

namespace Bamboo.Prevalence.Tests
{
	/// <summary>
	/// Summary description for PrevalenceTestBase.
	/// </summary>
	public abstract class PrevalenceTestBase : Assertion
	{
		protected string _PrevalenceBase;

		protected PrevalenceEngine _engine;	

		protected string PrevalenceBase
		{
			get
			{
				if (null == _PrevalenceBase)
				{
					//_PrevalenceBase = new Uri(new Uri(GetType().Assembly.CodeBase), "prevalence").LocalPath;
					_PrevalenceBase = Path.Combine(Path.GetTempPath(), GetType().FullName);					
				}
				return _PrevalenceBase;
			}
		}
		
		protected void ClearPrevalenceBase()
		{
			if (System.IO.Directory.Exists(PrevalenceBase))
			{
				foreach (string path in System.IO.Directory.GetFiles(PrevalenceBase))
				{
					System.IO.File.Delete(path);
				}
			}
		}

		protected void Snapshot()
		{
			_engine.TakeSnapshot();
		}

		protected void CrashRecover()
		{
			
			// The new engine automatically
			// recovers from crash by loading
			// its previous state		
			HandsOffOutputLog();
			_engine = CreateEngine();
		}

		protected Bamboo.Prevalence.PrevalenceEngine Engine
		{
			get
			{
				return _engine;
			}

			set
			{
				_engine = value;
			}
		}		

		protected virtual PrevalenceEngine CreateEngine()
		{			
			return PrevalenceActivator.CreateEngine(PrevalentSystemType, PrevalenceBase);
		}

		protected void HandsOffOutputLog()
		{
			if (null != _engine)
			{
				_engine.HandsOffOutputLog();
			}
		}

		protected object ExecuteCommand(Bamboo.Prevalence.ICommand command)
		{			
			return _engine.ExecuteCommand(command);
		}

		protected object ExecuteQuery(Bamboo.Prevalence.IQuery query)
		{
			return _engine.ExecuteQuery(query);
		}

		protected abstract System.Type PrevalentSystemType
		{
			get;
		}

		[SetUp]
		public virtual void SetUp()
		{			
			Bamboo.Prevalence.Configuration.PrevalenceSettings.FlushAfterCommand = false; // let's speed things up a little			
		}

		[TearDown]
		public virtual void TearDown()
		{	
			HandsOffOutputLog();	
		}
	}
}
