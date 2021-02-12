using MikuBot.Commands;

namespace MikuBot.Modules
{
	public abstract class BuiltinModule : ModuleBase, IBuiltinModule
	{
		public abstract void HandleCommand(MsgCommand command, Bot bot);
	}
}
