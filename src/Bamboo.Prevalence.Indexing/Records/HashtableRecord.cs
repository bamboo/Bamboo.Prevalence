using System;
using System.Collections;

namespace Bamboo.Prevalence.Indexing.Records
{
	/// <summary>
	/// An IRecord implementation over a Hashtable.
	/// </summary>
	[Serializable]
	public class HashtableRecord : Bamboo.Prevalence.Indexing.IRecord
	{
		protected Hashtable _hashtable;

		public HashtableRecord()
		{			
			_hashtable = new Hashtable();
		}

		public object this[string name]
		{
			get
			{
				return _hashtable[name];
			}

			set
			{
				_hashtable[name] = value;
			}
		}

		public override int GetHashCode()
		{
			return _hashtable.GetHashCode();
		}

		public override bool Equals(object rhs)
		{
			HashtableRecord other = rhs as HashtableRecord;
			if (null == other)
			{
				return false;
			}
			return other._hashtable.Equals(_hashtable);
		}
	}
}
