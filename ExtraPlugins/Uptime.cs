using System;
using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins
{
	public class Uptime : MsgCommandModuleBase
	{
		public override int CooldownChannelMs
		{
			get { return 10000; }
		}

		public override int CooldownUserMs
		{
			get { return 30000; }
		}

		public override string CommandDescription
		{
			get
			{
				return "Prints bot uptime.";
			}
		}

		public override string UsageHelp
		{
			get
			{
				return "uptime";
			}
		}

		public override void HandleCommand(MsgCommand msg, IBotContext bot)
		{
			if (!CheckCall(msg, bot))
				return;

			var startupTime = bot.StartupTime;
			var runningTime = DateTime.Now - startupTime;

			bot.Writer.Msg(msg.ChannelOrSenderNick, string.Format("The bot has been running for {0} hour(s) and {1} minute(s).", (int)runningTime.TotalHours, runningTime.Minutes));
		}

		public override string Name
		{
			get { return "Uptime"; }
		}
	}
}
