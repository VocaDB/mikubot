using MikuBot.Modules;
using MikuBot.Commands;
using System.Threading;

namespace MikuBot.ExtraPlugins
{
	public class AutoRejoinOnKick : GenericModuleBase
	{
		public override string HelpText
		{
			get { return "Automatically rejoins the channel when kicked."; }
		}

		public override void HandleCommand(IrcCommand command, IBotContext bot)
		{
			if (!(command is KickCommand))
				return;

			var cmd = (KickCommand)command;

			if (!cmd.KickedUserIsSelf)
				return;

			Thread.Sleep(500);
			bot.ChannelManager.Autojoin();
		}

		public override InitialModuleStatus InitialStatus
		{
			get { return InitialModuleStatus.Disabled; }
		}

		public override string Name
		{
			get { return "AutoRejoinOnKick"; }
		}
	}
}
