using System;

namespace Bamboo.Prevalence.Indexing
{
	/// <summary>
	/// A record that can be indexed by its fields.
	/// </summary>
	public interface IRecord
	{
		/// <summary>
		/// Field acessor. Returns the named field.
		/// </summary>		
		/// <exception cref="ArgumentException">if the specified named field does not exist</exception>
		object this[string name]
		{
			get;
		}
	}
}
