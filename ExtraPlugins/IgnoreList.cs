using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins
{
	public class IgnoreList : MsgCommandModuleBase
	{
		public override void HandleCommand(MsgCommand cmd, IBotContext bot)
		{
			if (!CheckCall(cmd, bot))
				return;

			var reply = new Receiver(bot.Writer, cmd.Sender.Nick);

			foreach (var user in bot.IgnoredNickList)
			{
				reply.Msg(user.ToString());
			}
		}

		public override string CommandDescription
		{
			get { return "Displays a list of ignored users."; }
		}

		public override string UsageHelp
		{
			get { return "IgnoreList"; }
		}

		public override BotUserLevel MinUserLevel
		{
			get { return BotUserLevel.Admin; }
		}

		public override string Name
		{
			get { return "IgnoreList"; }
		}
	}
}
