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
using System.Text;

namespace Bamboo.Prevalence.Indexing.FullText
{
	/// <summary>
	/// A collection of TermOccurrence objects.
	/// </summary>
	[Serializable]
	public class TermOccurrenceCollection : CollectionBase
	{
		/// <summary>
		/// Creates an empty collection.
		/// </summary>
		public TermOccurrenceCollection()
		{
		}

		/// <summary>
		/// Adds the information related to the new
		/// occurrence of the term in the field and
		/// position passed as argument. If a TermOccurrence
		/// object for the specified field
		/// is already in the collection, the new position
		/// information is simply added to the existing
		/// TermOccurrence object. Otherwise a new TermOccurrence
		/// object will be created and added to the
		/// collection.
		/// </summary>
		/// <param name="field">field where the term
		/// was found</param>
		/// <param name="position">
		/// position in the field where the term was found</param>
		public void Add(IndexedField field, int position)
		{
			foreach (TermOccurrence to in InnerList)
			{
				if (to.Field == field)
				{
					to.Add(position);
					return;
				}
			}
			InnerList.Add(new TermOccurrence(field, position));
		}

		/// <summary>
		/// Builds a readable representation of this object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			builder.Append("[");
			foreach (TermOccurrence to in InnerList)
			{
				builder.Append(to.ToString());
				builder.Append(", ");
			}
			builder.Append("]");
			return builder.ToString();
		}
	}
}
