using System;
using MikuBot.Commands;

namespace MikuBot.Modules
{
	public abstract class GenericModuleBase : ModuleBase, IGenericModule
	{
		public virtual void HandleCommand(IrcCommand command, IBotContext bot) { }

		public virtual void OnDisconnected(IBotContext bot) { }

		public virtual void OnMessageReceived(string line, IBotContext bot) { }

		public virtual void OnMessageSent(string line, IBotContext bot) { }
	}
}
