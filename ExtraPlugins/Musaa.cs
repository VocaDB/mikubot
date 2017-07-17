using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins {
	public class Musaa : MsgCommandModuleBase {

		public override int CooldownChannelMs {
			get { return 10000; }
		}

		public override string CommandDescription {
			get { return "Lauri_P!"; }
		}

		public override bool IsPassive {
			get { return false; }
		}

		public override string Name {
			get { return "Musaa"; }
		}

		public override string UsageHelp {
			get { return "Musaa"; }
		}

		public override void HandleCommand(MsgCommand msg, IBotContext bot) {

			if (!CheckCall(msg, bot))
				return;

			msg.Reply(bot.Writer).Msg("Lauri_P");

		}

	}

}
