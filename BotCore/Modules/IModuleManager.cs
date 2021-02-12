using System.Collections.Generic;

namespace MikuBot.Modules
{
	public interface IModuleManager
	{
		IEnumerable<IModule> AllModules { get; }

		IEnumerable<IModule> BuiltinModules { get; }

		IEnumerable<IGenericModule> GenericModules { get; }

		IEnumerable<IMsgCommandModule> MsgCommandModules { get; }

		/// <summary>
		/// Activates a module.
		/// The module will be activated regardless of whether it's enabled or not.
		/// </summary>
		/// <param name="bot">Bot context. Cannot be null.</param>
		/// <param name="module">Module to be called. Cannot be null.</param>
		void CallModule(IBotContext bot, IModule module);

		/// <summary>
		/// Finds a module by name.
		/// </summary>
		/// <param name="name">Module name (case insensitive).</param>
		/// <returns>Module matching the name, or null if no module was found.</returns>
		IModule FindModule(string name);

		/// <summary>
		/// Finds a module by type.
		/// </summary>
		/// <returns>Module matching the type, or null if no module was found.</returns>
		T FindModule<T>() where T : IModule;

		/// <summary>
		/// Checks whether a module is enabled.
		/// Modules that aren't enabled won't be activated automatically. They can be activated manually by calling <see cref="CallModule"/>.
		/// </summary>
		/// <param name="module">Module to be checked. Cannot be null.</param>
		/// <returns>True if the module is enabled. Otherwise false.</returns>
		bool IsModuleEnabled(IModule module);
	}
}
