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

namespace ObjectOperations
{
	class ProductType
	{
		string _code;

		string _label;

		public ProductType(string code, string label)
		{
			_code = code;
			_label = label;
		}

		public string Code
		{
			get
			{
				return _code;
			}
		}

		public string Label
		{
			get
			{
				return _label;
			}
		}
	}

	class InverseProductNameComparer : IComparer
	{
		public static IComparer Default = new InverseProductNameComparer();

		public int Compare(object lhs, object rhs)
		{
			Product lhsProduct = (Product)lhs;
			Product rhsProduct = (Product)rhs;
			return rhsProduct.Name.CompareTo(lhsProduct.Name);
		}
	}

	class Product
	{
		Guid _id;

		string _name;

		ProductType _type;

		public Product(string name, ProductType type)
		{
			_id = Guid.NewGuid();
			_name = name;
			_type = type;
		}

		public Guid ID
		{
			get
			{
				return _id;
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}

		public ProductType ProductType
		{
			get
			{
				return _type;
			}
		}
	}

	class ProductCatalog
	{
		Hashtable _products;

		ArrayList _productTypes;

		public ProductCatalog()
		{
			_products = new Hashtable();
			_productTypes = new ArrayList();
		}

		public long ProductCount
		{
			get
			{
				return _products.Count;
			}
		}

		public Product this[Guid id]
		{
			get
			{
				return (Product)_products[id];
			}
		}		

		public Product[] Products
		{
			get
			{
				Product[] products = new Product[_products.Count];
				_products.Values.CopyTo(products, 0);
				return products;
			}
		}

		public void AddProductType(ProductType type)
		{
			_productTypes.Add(type);
		}

		public void AddProduct(Product product)
		{
			_products[product.ID] = product;
		}

		public ArrayList QueryProductsByType(ProductType type)
		{
			ArrayList result = new ArrayList();
			foreach (Product p in _products.Values)
			{
				if (p.ProductType == type)
				{
					result.Add(p);
				}
			}
			return result;
		}
	}

	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Application
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			int outerLoop = 100;
			int innerLoop = 1000;

			if (2 == args.Length)
			{
				outerLoop = int.Parse(args[0]);
				innerLoop = int.Parse(args[1]);
			}
			else
			{
				Console.WriteLine("ObjectOperations [OuterLoop InnerLoop]");
			}

			Random random = new Random();

			ProductType[] types = new ProductType[]
				{
					new ProductType("dvds", "DVDs"),
					new ProductType("cds", "CDs"),
					new ProductType("lps", "LPs"),
					new ProductType("actionfigures", "Action Figures")
				};

			ProductCatalog catalog = new ProductCatalog();
			foreach (ProductType pt in types)
			{
				catalog.AddProductType(pt);
			}

			TimeSpan total = TimeSpan.Zero;
			for (int i=0; i<outerLoop; ++i)
			{
				DateTime start = DateTime.Now;

				int j=0;
				for (; j<innerLoop; ++j)
				{
					ProductType pt = types[random.Next(0, types.Length)];
					Product product = new Product(string.Format("product {0}, {1}", i, j), pt);
					catalog.AddProduct(product);
				}

				DateTime stop = DateTime.Now;
				TimeSpan elapsed = stop - start;
				total += elapsed;
				Console.WriteLine("It takes {0}ms to add {1} objects after the collection already has {2}.", elapsed.TotalMilliseconds, j, catalog.ProductCount - j);
			}
			Console.WriteLine("It takes {0}ms to construct and add {1} objects to a hashtable.", total.TotalMilliseconds, catalog.ProductCount);
			
			foreach (ProductType pt in types)
			{
				DateTime start = DateTime.Now;
				ArrayList found = catalog.QueryProductsByType(pt);
				TimeSpan elapsed = DateTime.Now - start;
				Console.WriteLine("It takes {0}ms to select {1} {2} from the collection of {3} products.", elapsed.TotalMilliseconds, found.Count, pt.Label, catalog.ProductCount);

				start = DateTime.Now;
				Guid productID = ((Product)found[found.Count-1]).ID;
				Product p = catalog[productID];
				elapsed = DateTime.Now - start;
				Console.WriteLine("It takes {0}ms to access a product by its ID.", elapsed.TotalMilliseconds);

				start = DateTime.Now;
				foreach (Product existing in found)
				{
					if (productID == existing.ID)
					{
						break;
					}
				}
				elapsed = DateTime.Now - start;
				Console.WriteLine("It takes {0}ms to perform {1} Guid comparisons", elapsed.TotalMilliseconds, found.Count);

				// Test the worst sorting case
				start = DateTime.Now;
				found.Sort(InverseProductNameComparer.Default);
				elapsed = DateTime.Now - start;
				Console.WriteLine("It takes {0}ms to sort an ArrayList with {1} elements using a static bound comparer.", elapsed.TotalMilliseconds, found.Count);
				
				// worst sorting case again...
				start = DateTime.Now;
				found.Sort(new Bamboo.Prevalence.Collections.ObjectPropertyComparer(typeof(Product), "Name"));
				elapsed = DateTime.Now - start;
				Console.WriteLine("It takes {0}ms to sort an ArrayList with {1} elements using a dynamic bound comparer.", elapsed.TotalMilliseconds, found.Count);
			}

			TimeXPathQuery(catalog);
			
			System.GC.Collect();

			Console.WriteLine("This process is using {0} bytes to hold {1} products.", Environment.WorkingSet, catalog.ProductCount);
		}		

		static void TimeXPathQuery(ProductCatalog catalog)
		{
			Bamboo.Prevalence.XPath.XPathObjectNavigator navigator = new Bamboo.Prevalence.XPath.XPathObjectNavigator(catalog);
			DateTime start = DateTime.Now;
			object[] objects = navigator.SelectObjects("Products/Product[ProductType/Code='dvds']");
			TimeSpan elapsed = DateTime.Now - start;
			Console.WriteLine("It takes {0}ms to select {1} products using an expensive XPath query", elapsed.TotalMilliseconds, objects.Length);
		}
	}
}
