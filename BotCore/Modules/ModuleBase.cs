using System;
using MikuBot.Commands;

namespace MikuBot.Modules
{
	public abstract class ModuleBase : IModule
	{
		protected bool CheckAccess(IrcCommand cmd, IBotContext bot)
		{
			return BotHelper.CheckAuthenticated(cmd, bot, MinUserLevel);
		}

		public virtual string HelpText
		{
			get { return string.Empty; }
		}

		public virtual InitialModuleStatus InitialStatus
		{
			get { return InitialModuleStatus.Enabled; }
		}

		public virtual BotUserLevel MinUserLevel
		{
			get { return BotUserLevel.Unidentified; }
		}

		public abstract string Name { get; }

		public virtual void OnConnected(IBotContext bot) { }

		public virtual void OnLoaded(IBotContext bot, IModuleFile moduleFile) { }
	}
}
