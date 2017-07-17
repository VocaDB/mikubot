using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikuBot.Modules;
using MikuBot.Commands;

namespace MikuBot.ExtraPlugins {

	public class Links : MsgCommandModuleBase {

		public override int CooldownChannelMs {
			get { return 10000; }
		}

		public override int CooldownUserMs {
			get { return 30000; }
		}

		public override string CommandDescription {
			get {
				return "Displays links location.";
			}
		}

		public override string UsageHelp {
			get {
				return "links";
			}
		}

		public override void HandleCommand(MsgCommand msg, IBotContext bot) {

			if (!CheckCall(msg, bot))
				return;

			if (!msg.ReceiverIsChannel) {
				bot.Writer.Msg(msg.ChannelOrSenderNick, "Has to be sent on a channel");
				return;
			}

			var statLocation = "http://vocaloid.eu/mikubot/Channel/Links/" + msg.ChannelOrSenderNick.ToString().Substring(1);

			bot.Writer.Msg(msg.ChannelOrSenderNick, "Links list can be found at " + statLocation);

		}

		public override string Name {
			get { return "Links"; }
		}
	}

}
