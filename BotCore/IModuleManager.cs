using System.Collections.Generic;
using MikuBot.Modules;

namespace MikuBot {

	public interface IModuleManager {

		IEnumerable<IModule> AllModules { get; }

		IEnumerable<IModule> BuiltinModules { get; }

		IEnumerable<IGenericModule> GenericModules { get; }

		IEnumerable<IMsgCommandModule> MsgCommandModules { get; }

		IModule FindModule(string name);

		bool IsModuleEnabled(IModule module);

	}

}
