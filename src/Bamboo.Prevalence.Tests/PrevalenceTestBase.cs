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
