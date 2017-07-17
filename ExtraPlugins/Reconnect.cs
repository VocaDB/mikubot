using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins {

	public class Reconnect : MsgCommandModuleBase {

		public override string CommandDescription {
			get { return "Disconnects and reconnects the bot."; }
		}

		public override string UsageHelp {
			get { return "reconnect"; }
		}

		public override void HandleCommand(MsgCommand msg, IBotContext bot) {

			if (!CheckCall(msg, bot))
				return;

			bot.Reconnect();

		}

		public override BotUserLevel MinUserLevel {
			get { return BotUserLevel.Admin; }
		}

		public override string Name {
			get { return "Reconnect"; }
		}

	}

}
