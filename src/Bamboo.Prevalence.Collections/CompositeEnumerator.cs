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

namespace Bamboo.Prevalence.Collections
{
	/// <summary>
	/// Composite over IEnumerable or IEnumerator objects.
	/// </summary>
	public class CompositeEnumerator : IEnumerable, IEnumerator
	{
		IEnumerator[] _enumerators;
		IEnumerator _current;
		int _currentIndex = 0;

		public CompositeEnumerator(params IEnumerator[] enumerators)
		{
			if (null == enumerators)
			{
				throw new ArgumentNullException("enumerators");
			}

			if (0 == enumerators.Length)
			{
				throw new ArgumentException("At least one IEnumerator must be provided!", "enumerators");
			}

			_enumerators = enumerators;
			Reset();
		}

		public CompositeEnumerator(params IEnumerable[] enumerables)
		{				
			if (null == enumerables)
			{
				throw new ArgumentNullException("enumerables");
			}

			if (0 == enumerables.Length)
			{
				throw new ArgumentException("At least one IEnumerable object must be provided!", "enumerables");
			}

			_enumerators = new IEnumerator[enumerables.Length];
			for (int i=0; i<enumerables.Length; ++i)
			{
				_enumerators[i] = enumerables[i].GetEnumerator();
			}		
	
			Reset();
		}

		public IEnumerator GetEnumerator()
		{
			return this;
		}

		public void Reset()
		{
			foreach (IEnumerator e in _enumerators)
			{
				e.Reset();
			}
			_currentIndex = 0;
			_current = _enumerators[0];
		}

		public bool MoveNext()
		{		
			bool next = _current.MoveNext();
			while (!next)
			{
				++_currentIndex;
				if (_currentIndex < _enumerators.Length)
				{
					_current = _enumerators[_currentIndex];
					next = _current.MoveNext();
				}
				else
				{
					break;
				}
			}
			return next;
		}

		public object Current
		{
			get
			{
				return _current.Current;
			}
		}
	}
}
