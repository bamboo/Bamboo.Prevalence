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
			_currentIndex = 0;
			_current = _enumerators[0];
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
	
			_currentIndex = 0;
			_current = _enumerators[0];
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
			bool hasNext = _current.MoveNext();
			while (!hasNext)
			{
				++_currentIndex;
				if (_currentIndex < _enumerators.Length)
				{
					_current = _enumerators[_currentIndex];
					hasNext = _current.MoveNext();
				}
				else
				{
					break;
				}
			}
			return hasNext;
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
