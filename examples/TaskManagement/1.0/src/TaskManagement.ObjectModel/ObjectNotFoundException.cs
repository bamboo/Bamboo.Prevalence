using System;

namespace TaskManagement.ObjectModel
{
	/// <summary>
	/// Um objeto necessário para a operação não foi encontrado.
	/// </summary>
	public class ObjectNotFoundException : ApplicationException
	{
		public ObjectNotFoundException(string message) : base(message)
		{
		}
	}
}
