using System;

namespace Bamboo.Prevalence.Implementation
{
	/// <summary>
	/// Command surrogate that allows the execution of a 
	/// command as if in a specific moment in time.
	/// </summary>
	[Serializable]
	internal class ClockRecoveryCommand : Bamboo.Prevalence.ICommand
	{
		private Bamboo.Prevalence.ICommand _command;

		private System.DateTime _dateTime;

		public ClockRecoveryCommand(Bamboo.Prevalence.ICommand command, System.DateTime dateTime)
		{
			_command = command;
			_dateTime = dateTime;
		}

		object Bamboo.Prevalence.ICommand.Execute(Bamboo.Prevalence.IPrevalentSystem system)
		{
			system.Clock.Recover(_dateTime);
			return _command.Execute(system);
		}
	}
}
