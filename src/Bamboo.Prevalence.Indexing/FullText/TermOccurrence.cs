using System;
using System.Text;

namespace Bamboo.Prevalence.Indexing.FullText
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class TermOccurrence
	{
		IndexedField _field;
		
		int[] _positions;
		
		public TermOccurrence(IndexedField field, int position)
		{
			_field = field;
			_positions = new int[] { position };
		}

		public IndexedField Field
		{
			get
			{
				return _field;
			}
		}
	
		public int[] Positions
		{
			get
			{
				return _positions;
			}
		}

		internal void Add(int position)
		{
			int[] newPositions = new int[_positions.Length + 1];
			Array.Copy(_positions, newPositions, _positions.Length);
			newPositions[_positions.Length] = position;
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			builder.Append("<");
			builder.Append(_field.ToString());
			builder.Append(" => ");
			builder.Append("[");
			for (int i=0; i<_positions.Length; ++i)
			{
				builder.Append(_positions[i]);
				builder.Append(", ");
			}
			builder.Append("]");
			return builder.ToString();
		}
	}
}
