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
