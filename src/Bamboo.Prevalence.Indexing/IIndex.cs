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

namespace Bamboo.Prevalence.Indexing
{
	/// <summary>
	/// A index.
	/// </summary>
	public interface IIndex
	{
		/// <summary>
		/// Adds a new record to the index.
		/// </summary>
		/// <param name="record">the record</param>
		void Add(IRecord record);

		/// <summary>
		/// Removes a record from the index.
		/// </summary>
		/// <param name="record">existing record that
		/// must be removed</param>
		/// <remarks>for the sake of efficiency,
		/// reference comparison should always be preferred
		/// over object.Equals</remarks>
		void Remove(IRecord record);

		/// <summary>
		/// Reindexes a existing record. Reindexing
		/// must always be explicitly started by
		/// the application.
		/// </summary>
		/// <param name="record">an existing record that
		/// was externally changed and thus should
		/// have its index updated</param>
		/// <remarks>for the sake of efficiency,
		/// reference comparison should always be preferred
		/// over object.Equals</remarks>
		void Update(IRecord record);

		/// <summary>
		/// Executes the search represented by the
		/// expression passed as argument.<br />
		/// 
		/// If the index does not know how to 
		/// execute the search it should call
		/// <see cref="ISearchExpression.Evaluate"/>. Because
		/// of that, <see cref="ISearchExpression.Evaluate"/>
		/// must never call this method.
		/// </summary>
		/// <param name="expression">search expression</param>
		SearchResult Search(ISearchExpression expression);
	}
}
