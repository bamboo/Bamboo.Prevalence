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
using System.Reflection;
using System.Runtime.Serialization;
using Bamboo.Prevalence.VersionMigration.Initializers;

namespace Bamboo.Prevalence.VersionMigration
{
	/// <summary>
	/// MigrationSurrogateSelector.
	/// </summary>
	internal class MigrationSurrogateSelector : ISurrogateSelector, ISerializationSurrogate
	{
		private MigrationContext _context;

		public MigrationSurrogateSelector(MigrationContext context)
		{
			_context = context;
		}

		#region Implementation of ISurrogateSelector
		public System.Runtime.Serialization.ISurrogateSelector GetNextSelector()
		{
			return null;
		}

		public System.Runtime.Serialization.ISerializationSurrogate GetSurrogate(System.Type type, System.Runtime.Serialization.StreamingContext context, out System.Runtime.Serialization.ISurrogateSelector selector)
		{
			selector = this;

			if (_context.HasInitializers(type))
			{				
				return this;
			}			

			return null;
		}

		public void ChainSelector(System.Runtime.Serialization.ISurrogateSelector selector)
		{
			throw new NotSupportedException();
		}
		#endregion

		#region Implementation of ISerializationSurrogate
		public void GetObjectData(object obj, System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{				
			throw new NotSupportedException();
		}

		public object SetObjectData(object obj, System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context, System.Runtime.Serialization.ISurrogateSelector selector)
		{
			_context.EnterObject(obj, info);

			Type type = obj.GetType();
			TypeMapping mapping = _context.GetTypeMapping(type);

			IObjectInitializer initializer = mapping.Initializer;
			if (null != initializer)
			{
				initializer.InitializeObject(_context);
			}
			else
			{
				DefaultObjectInitializer.Default.InitializeObject(_context);
			}

			_context.LeaveObject();

			return null;
		}
		#endregion
	}
}
