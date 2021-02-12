namespace MikuBot.Modules
{
	/// <summary>
	/// Base interface for modules.
	/// </summary>
	public interface IModule
	{
		string HelpText { get; }

		/// <summary>
		/// Module status when it's first loaded. Configuration setting for disabled modules overrides this.
		/// </summary>
		InitialModuleStatus InitialStatus { get; }

		/// <summary>
		/// Minimum user level required by this module.
		/// </summary>
		BotUserLevel MinUserLevel { get; }

		string Name { get; }

		/// <summary>
		/// Called when the bot has successfully connected to a server.
		/// </summary>
		/// <param name="bot">Bot. Cannot be null.</param>
		void OnConnected(IBotContext bot);

		/// <summary>
		/// Called when this module has been loaded.
		/// </summary>
		/// <param name="bot">Bot. Cannot be null.</param>
		/// <param name="moduleFile">Module file. Can be null if there is no module file object.</param>
		void OnLoaded(IBotContext bot, IModuleFile moduleFile);
	}
}
