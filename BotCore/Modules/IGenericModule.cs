using MikuBot.Commands;

namespace MikuBot.Modules
{
	/// <summary>
	/// Generic module. Able to process any type of IRC message or event.
	/// </summary>
	public interface IGenericModule : IModule
	{
		void HandleCommand(IrcCommand command, IBotContext bot);

		void OnDisconnected(IBotContext bot);

		void OnMessageReceived(string line, IBotContext bot);

		void OnMessageSent(string line, IBotContext bot);
	}
}
