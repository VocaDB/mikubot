using System.Collections.Generic;
using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins {

	public class Stats : MsgCommandModuleBase {

		private readonly Dictionary<IrcName, string> statLocations = new Dictionary<IrcName, string> {
			{ new IrcName("#vocadb"), "http://mikustats.vocaloid.eu" }, 
			{ new IrcName("#yama"), "http://yamastats.vocaloid.eu" }
		};

		public override int CooldownChannelMs {
			get { return 10000; }
		}

		public override int CooldownUserMs {
			get { return 30000; }
		}

		public override string CommandDescription {
			get { 
				return "Displays stats location. Channel name is optional. If it's not specified, stats will be displayed for the current channel, if available.";
			}
		}

		public override string  UsageHelp {
			get { 
				return "stats [<channel name>]";
			}
		}

		public override void HandleCommand(MsgCommand msg, IBotContext bot) {

			if (!CheckCall(msg, bot))
				return;

			string statLocation = null;

			if (msg.BotCommand.Params.HasParam(0)) {
				statLocations.TryGetValue(new IrcName(msg.BotCommand.Params[0]), out statLocation);
			}

			if (statLocation == null && !statLocations.TryGetValue(msg.Receiver, out statLocation))
				statLocation = "http://stats.vocaloid.eu";

			bot.Writer.Msg(msg.ChannelOrSenderNick, "Stats can be found at " + statLocation);

		}

		public override string Name {
			get { return "Stats"; }
		}
	}

}
