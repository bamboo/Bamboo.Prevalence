using System;
using System.Collections;

namespace Bamboo.Prevalence.Indexing.FullText
{	
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class Posting
	{
		IRecord _record;
		
		TermOccurrenceCollection _occurrences;

		public Posting(IRecord record)
		{				
			_record = record;
			_occurrences = new TermOccurrenceCollection();
		}

		internal TermOccurrenceCollection Occurrences
		{
			get
			{
				return _occurrences;
			}
		}
		
		public IRecord Record
		{
			get
			{
				return _record;
			}
		}		

		public override string ToString()
		{
			return "<" + _record + " => " + _occurrences + ">";
		}
	}
}
