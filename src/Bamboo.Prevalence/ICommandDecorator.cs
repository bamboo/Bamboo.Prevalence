using System;

namespace Bamboo.Prevalence
{
	/// <summary>
	/// Decorates a command before it is saved to
	/// the command log. This interface can be
	/// implemented only by attributes that should
	/// be applied to a prevalent system class.
	/// </summary>
	/// <remarks>
	/// Before a command gets written to the command
	/// log the PrevalenceEngine class allows it to
	/// be decorated in order to preserve any context
	/// information that its execution might be sensitive
	/// to. An good example would be a command that is 
	/// dependent upon the principal associated to
	/// the running thread. If this command is to be
	/// successfully re-executed at system recovery time,
	/// the principal must be saved.<br />
	/// See <see cref="Bamboo.Prevalence.Attributes.PrincipalSensitiveAttribute"/>.
	/// </remarks>
	public interface ICommandDecorator
	{
		/// <summary>
		/// Decorates the command.
		/// </summary>
		/// <returns>a new command object or command</returns>
		ICommand Decorate(ICommand command);
	}
}
