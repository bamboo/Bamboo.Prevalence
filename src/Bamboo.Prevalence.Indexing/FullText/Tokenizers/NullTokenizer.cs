using System;
using Bamboo.Prevalence.Indexing.FullText;

namespace Bamboo.Prevalence.Indexing.FullText.Tokenizers
{
	/// <summary>
	/// Summary description for NullTokenizer.
	/// </summary>
	public class NullTokenizer : ITokenizer
	{
		/// <summary>
		/// The one and only NullTokenizer instance.
		/// </summary>
		public static ITokenizer Instance = new NullTokenizer();

		private NullTokenizer()
		{
		}

		#region Implementation of ITokenizer
		/// <summary>
		/// Always returns null since this tokenizer
		/// must be the last in the chain.
		/// </summary>
		public ITokenizer Previous
		{
			get
			{
				return null;
			}

			set
			{
				throw new NotSupportedException("ITokenizer chaining not supported by NullTokenizer!");
			}
		}

		/// <summary>
		/// See <see cref="Bamboo.Prevalence.Indexing.FullText.ITokenizer.NextToken"/> for
		/// details.
		/// </summary>
		/// <returns></returns>
		public Bamboo.Prevalence.Indexing.FullText.Token NextToken()
		{
			return null;
		}

		/// <summary>
		/// Returns this.
		/// </summary>
		/// <param name="tail">must always be null</param>
		/// <returns>a clone</returns>
		public ITokenizer Clone(ITokenizer tail)
		{
			return this;
		}
		#endregion
	}
}
