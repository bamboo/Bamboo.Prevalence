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
using System.Collections;

namespace Bamboo.Prevalence.Indexing.Records
{
	/// <summary>
	/// An IRecord implementation over a Hashtable.
	/// </summary>
	[Serializable]
	public class HashtableRecord : Bamboo.Prevalence.Indexing.IRecord
	{
		/// <summary>
		/// The hashtable.
		/// </summary>
		protected Hashtable _hashtable;

		/// <summary>
		/// Creates an empty record.
		/// </summary>
		public HashtableRecord()
		{			
			_hashtable = new Hashtable();
		}

		/// <summary>
		/// Sets/Gets a hashtable field.
		/// </summary>
		public object this[string name]
		{
			get
			{
				return _hashtable[name];
			}

			set
			{
				_hashtable[name] = value;
			}
		}

		/// <summary>
		/// Delegates to the internal hashtable.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return _hashtable.GetHashCode();
		}

		/// <summary>
		/// Delegates to the internal hashtable.
		/// </summary>
		/// <param name="rhs"></param>
		/// <returns></returns>
		public override bool Equals(object rhs)
		{
			HashtableRecord other = rhs as HashtableRecord;
			if (null == other)
			{
				return false;
			}
			return other._hashtable.Equals(_hashtable);
		}
	}
}
