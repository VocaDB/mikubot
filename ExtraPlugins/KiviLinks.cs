using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins
{
	public class KiviLinks : MsgCommandModuleBase
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
				return "Displays kivilinks location.";
			}
		}

		public override string UsageHelp
		{
			get
			{
				return "kivilinks";
			}
		}

		public override void HandleCommand(MsgCommand msg, IBotContext bot)
		{
			if (!CheckCall(msg, bot))
				return;

			string statLocation = string.Format("http://vocaloid.eu/mikubot/Channel/Links/{0}?nick=kyllakivi",
				msg.ChannelOrSenderNick.ToString().Substring(1));

			bot.Writer.Msg(msg.ChannelOrSenderNick, string.Format("kivilinks can be found at {0}", statLocation));
		}

		public override string Name
		{
			get { return "kivilinks"; }
		}
	}
}
