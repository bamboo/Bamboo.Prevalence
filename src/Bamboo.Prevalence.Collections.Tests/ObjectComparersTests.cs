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
