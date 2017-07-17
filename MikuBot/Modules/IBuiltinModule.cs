using MikuBot.Commands;

namespace MikuBot.Modules {

	public interface IBuiltinModule : IModule {

		void HandleCommand(MsgCommand command, Bot bot);

	}

}
