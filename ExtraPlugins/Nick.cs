using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins
{
	public class Nick : MsgCommandModuleBase
	{
		public override string CommandDescription
		{
			get { return "Changes the bot's nick."; }
		}

		public override string UsageHelp
		{
			get { return "nick [<nickname>]"; }
		}

		public override void HandleCommand(MsgCommand msg, IBotContext bot)
		{
			if (!CheckCall(msg, bot))
				return;

			if (msg.BotCommand.Params.HasParam(0))
			{
				var nick = new IrcName(msg.BotCommand.Params[0]);
				bot.NickManager.Set(nick);
			}
			else
			{
				bot.NickManager.Next();
			}
		}

		public override BotUserLevel MinUserLevel
		{
			get { return BotUserLevel.Manager; }
		}

		public override string Name
		{
			get { return "Nick"; }
		}
	}
}
