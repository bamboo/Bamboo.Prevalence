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
	public class Product
	{
		string _name;

		Decimal _price;

		int _priority;

		public Product(string name, Decimal price, int priority)
		{
			_name = name;
			_price = price;
			_priority = priority;
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}

		public Decimal Price
		{
			get
			{
				return _price;
			}
		}

		public int Priority
		{
			get
			{
				return _priority;
			}
		}

		public override string ToString()
		{
			return string.Format("{0}, {1}, {2}", _name, _price, _priority);
		}
	}

	/// <summary>
	/// Test cases for ObjectFieldComparer and friends.
	/// </summary>
	[TestFixture]
	public class ObjectComparersTests : Assertion
	{
		Product[] _products;

		Product _product1;
		Product _product2;
		Product _product3;
		Product _product4;

		/// <summary>
		/// 
		/// </summary>
		[SetUp]
		public void SetUp()
		{
			_products = new Product[]
				{
					_product1 = new Product("Foo", new Decimal(3.45), 10),
					_product2 = new Product("Bar", new Decimal(4.55), 5),					
					_product3 = new Product("Zeng", new Decimal(1.99), 30),
					_product4 = new Product("Zeng", new Decimal(0.55), 40)
				};
		}

		[Test]
		public void TestObjectPropertyComparer()
		{
			Array.Sort(_products, new ObjectPropertyComparer(typeof(Product), "Name"));
			AssertProducts(_product2, _product1, _product4, _product3);

			Array.Sort(_products, new ObjectPropertyComparer(typeof(Product), "Price"));
			AssertProducts(_product4, _product3, _product1, _product2);

			Array.Sort(_products, new ObjectPropertyComparer(typeof(Product), "Priority"));
			AssertProducts(_product2, _product1, _product3, _product4);
		}

		[Test]
		public void TestCompositeComparer()
		{
			CompositeComparer comparer = new CompositeComparer(
				new ObjectPropertyComparer(typeof(Product), "Name"),
				new ObjectPropertyComparer(typeof(Product), "Price")
				);		
			
			Array.Sort(_products, comparer);			
			AssertProducts(_product2, _product1, _product4, _product3);
		}

		void AssertProducts(params Product[] expectedProducts)
		{
			for (int i=0; i<expectedProducts.Length; ++i)
			{
				AssertEquals(expectedProducts[i], _products[i]);
			}
		}
	}
}
