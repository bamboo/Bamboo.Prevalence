using System;

namespace TaskManagement.ObjectModel
{
	/// <summary>
	/// Um registro de horas trabalhadas.
	/// </summary>
	[Serializable]
	public class WorkRecord
	{		
		protected DateTime _startTime;

		protected DateTime _endTime;

		/// <summary>
		/// Cria um novo registro para o período indicado.
		/// </summary>
		/// <param name="startTime">início do período</param>
		/// <param name="endTime">fim do período</param>
		public WorkRecord(DateTime startTime, DateTime endTime)
		{
			if (endTime < startTime)
			{
				throw new ArgumentException("A hora de fim deve ser maior que a hora de início!", "endTime");
			}

			_startTime = startTime;
			_endTime = endTime;
		}

		/// <summary>
		/// Início do período trabalhado.
		/// </summary>
		public DateTime StartTime
		{
			get
			{
				return _startTime;
			}
		}

		/// <summary>
		/// Fim do período trabalhado.
		/// </summary>
		public DateTime EndTime
		{
			get
			{
				return _endTime;
			}
		}
	}
}
