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

// Thanks to Jesse Ezell for coming up with the idea.

using System;
using System.Runtime.Serialization.Formatters.Binary;
using Bamboo.Prevalence.Implementation;

namespace Bamboo.Prevalence
{	
	internal class TransparentPrevalenceEngine : PrevalenceEngine
	{
		private object _transparentProxy;

		public TransparentPrevalenceEngine(System.Type systemType, string prevalenceBase, BinaryFormatter formatter, PrevalenceEngine.ExceptionDuringRecoveryHandler handler) :
			base(systemType, prevalenceBase, formatter, handler)
		{			
			if (!(_system is MarshalByRefObject))
			{
				throw new ArgumentException("Prevalent system type must extend MarshalByRefObject to be used with TransparentPrevalenceEngine!", "systemType");
			}
			_transparentProxy = new PrevalentSystemProxy(this, (MarshalByRefObject)_system).GetTransparentProxy();
		}

		public TransparentPrevalenceEngine(System.Type systemType, string prevalenceBase, BinaryFormatter formatter) :
			this(systemType, prevalenceBase, formatter, null)
		{
		}

		public override object PrevalentSystem
		{
			get
			{
				return _transparentProxy;
			}
		}
	}
}
