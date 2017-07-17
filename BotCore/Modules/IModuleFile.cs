namespace MikuBot.Modules {

	public interface IModuleFile {

		/// <summary>
		/// Called when the module file loading has started, before any modules have been loaded.
		/// </summary>
		/// <param name="bot">Bot. Cannot be null.</param>
		void OnLoading(IBotContext bot);

		/// <summary>
		/// Called when the module file has been loaded. All modules are loaded at this point.
		/// </summary>
		/// <param name="bot">Bot. Cannot be null.</param>
		void OnLoaded(IBotContext bot);

		/// <summary>
		/// Called when a module has been loaded.
		/// </summary>
		/// <param name="module">Module which was loaded. Cannot be null.</param>
		/// <param name="bot">Bot. Cannot be null.</param>
		void OnModuleLoaded(IModule module, IBotContext bot);

	}

}
