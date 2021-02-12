using MikuBot.Commands;

namespace MikuBot.Modules
{
	/// <summary>
	/// Module for processing <see cref="MsgCommand"/> messages.
	/// </summary>
	public interface IMsgCommandModule : IModule
	{
		/// <summary>
		/// Whether this message module is passive. Passive modules cannot be called directly. 
		/// Generic modules are passive by default, but message modules are not.
		/// </summary>
		bool IsPassive { get; }

		void HandleCommand(MsgCommand command, IBotContext bot);
	}
}
