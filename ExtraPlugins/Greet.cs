using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins
{
	public class Greet : GenericModuleBase
	{
		public override string HelpText
		{
			get { return "Posts an automatic greeting when joining a channel."; }
		}

		public override InitialModuleStatus InitialStatus
		{
			get { return InitialModuleStatus.Disabled; }
		}

		public override BotUserLevel MinUserLevel
		{
			get { return BotUserLevel.Unidentified; }
		}

		public override string Name
		{
			get { return "Greet"; }
		}

		public override void HandleCommand(IrcCommand command, IBotContext bot)
		{
			if (!(command is JoinCommand))
				return;

			var cmd = (JoinCommand)command;

			if (cmd.Sender.IsSelf)
			{
				bot.Writer.Msg(cmd.Channel, "♪ Miku Miku~ ♪");
			}
			else
			{
				//bot.Writer.Msg(cmd.Channel, "hi " + cmd.Sender.Nick + "! Welcome to " + cmd.Channel);				
			}
		}
	}
}
