using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins
{
	public class LutkustIneen : MsgCommandModuleBase
	{
		public override int CooldownChannelMs
		{
			get { return 20000; }
		}

		public override string CommandDescription
		{
			get { return "Vitun lutkust ineen!"; }
		}

		public override InitialModuleStatus InitialStatus
		{
			get { return InitialModuleStatus.NotLoaded; }
		}

		public override bool IsPassive
		{
			get { return true; }
		}

		public override string Name
		{
			get { return "Lutkust_ineen"; }
		}

		public override string UsageHelp
		{
			get { return "lutkust ineen"; }
		}

		public override void HandleCommand(MsgCommand msg, IBotContext bot)
		{
			if (msg.Text.ToLowerInvariant().Contains("lutkust ineen"))
			{
				if (!CheckCooldowns(msg, bot, false))
					return;

				bot.Writer.Msg(msg.ChannelOrSenderNick, "Vitun lutkust ineen! (automaatti)");
			}
		}
	}
}
