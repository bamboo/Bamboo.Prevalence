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
