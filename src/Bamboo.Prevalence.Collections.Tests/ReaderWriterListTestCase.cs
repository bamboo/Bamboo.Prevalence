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
using NUnit.Framework;
using Bamboo.Prevalence.Collections;

namespace Bamboo.Prevalence.Collections.Tests
{
	class Customer
	{
		string _name;

		public Customer(string name)
		{
			_name = name;
		}

		public string Name
		{
			get
			{
				return _name;
			}

			set
			{
				_name = value;
			}
		}

		public override bool Equals(object other)
		{
			Customer customer = other as Customer;
			if (null == customer)
			{
				return false;
			}
			return customer.Name.Equals(_name);
		}
	}

	/// <summary>
	/// Test cases for the ReaderWriterList class.
	/// </summary>
	[TestFixture]
	public class ReaderWriterListTestCase : Assertion
	{
		ReaderWriterList _list;

		Customer _customer1;

		Customer _customer2;

		[SetUp]
		public void SetUp()
		{
			_customer1 = new Customer("bamboo");
			_customer2 = new Customer("prevalence");
			_list = new ReaderWriterList();
			_list.Add(_customer1);
			_list.Add(_customer2);
		}		

		[Test]
		public void TestConstruct()
		{
			AssertEquals(0, new ReaderWriterList().Count);
		}
		
		[Test]
		public void TestReverse()
		{
			List reversed = _list.Reverse();
			AssertEquals(2, reversed.Count);
			AssertSame(_customer2, reversed[0]);
			AssertSame(_customer1, reversed[1]);
		}

		[Test]
		public void TestPopAny()
		{
			AssertEquals(2, _list.Count);

			object o1 = _list.PopAny();
			AssertEquals(1, _list.Count);

			object o2 = _list.PopAny();
			if (o1 == _customer1)
			{
				AssertSame(o2, _customer2);
			}
			else
			{
				AssertSame(o1, _customer2);
				AssertSame(o2, _customer1);
			}
			AssertEquals(0, _list.Count);
		}

		[Test]
		public void TestAdd()
		{			
			AssertEquals(2, _list.Count);
			AssertSame(_customer1, _list[0]);
			AssertSame(_customer2, _list[1]);
		}

		[Test]
		public void TestContains()
		{
			Assert(!_list.Contains(null));
			Assert(!_list.Contains(new Customer("foo")));
			Assert(_list.Contains(_customer1));
			Assert(_list.Contains(_customer2));
		}

		object ToUpperName(object obj)
		{
			Customer customer = (Customer)obj;
			return new Customer(customer.Name.ToUpper());
		}

		[Test]
		public void TestMap()
		{
			_list.Map(new Mapping(ToUpperName));
			Assert(_customer1 != _list[0]);			
			AssertEquals(_customer1.Name.ToUpper(), ((Customer)_list[0]).Name);
			Assert(_customer2 != _list[1]);
			AssertEquals(_customer2.Name.ToUpper(), ((Customer)_list[1]).Name);
		}

		void MakeUpperName(object obj)
		{
			Customer customer = (Customer)obj;
			customer.Name = customer.Name.ToUpper();
		}

		[Test]
		public void TestApply()
		{
			_list.Apply(new Action(MakeUpperName));
			AssertSame(_customer1, _list[0]);
			AssertSame(_customer2, _list[1]);
			AssertEquals("BAMBOO", _customer1.Name);
			AssertEquals("PREVALENCE", _customer2.Name);
		}

		[Test]
		public void TestMapEnumerable()
		{
			ReaderWriterList list = new ReaderWriterList();
			list.Add(new Customer("bamboo"));
			list.Add(new Customer("prevalence"));
			list.Map(_list);
			AssertSame(_customer1, list[0]);
			AssertSame(_customer2, list[1]);
		}

		[Test]
		public void TestCollect()
		{
			object[] items = _list.Collect(new Predicate(NameStartsWithX));
			AssertNotNull(items);
			AssertEquals(0, items.Length);

			items = _list.Collect(new Predicate(NameStartsWithP));
			AssertNotNull(items);
			AssertEquals(1, items.Length);
			AssertSame(_customer2, items[0]);		
		}

		[Test]
		public void TestFind()
		{
			AssertNull(_list.Find(new Predicate(NameStartsWithX)));
			AssertSame(_customer2, _list.Find(new Predicate(NameStartsWithP)));
		}

		[Test]
		public void TestAny()
		{
			Assert(!_list.Any(new Predicate(NameStartsWithX)));
			Assert(_list.Any(new Predicate(NameStartsWithP)));
			_list.Add(new Customer("XXX"));
			Assert(_list.Any(new Predicate(NameStartsWithX)));
		}
		
		[Test]
		public void TestGetRange()
		{
			object[] range = _list.GetRange(0, 3);
			AssertEquals(2, range.Length);
			AssertSame(_customer1, range[0]);
			AssertSame(_customer2, range[1]);
			
			range = _list.GetRange(1, 1);
			AssertEquals(1, range.Length);
			AssertSame(_customer2, range[0]);
		}

		bool NameStartsWithP(object customer)
		{
			return ((Customer)customer).Name.ToLower().StartsWith("p");
		}

		bool NameStartsWithX(object customer)
		{
			return ((Customer)customer).Name.ToLower().StartsWith("x");
		}
	}
}
