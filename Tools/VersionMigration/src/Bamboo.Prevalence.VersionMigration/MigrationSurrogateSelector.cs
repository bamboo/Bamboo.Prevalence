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
