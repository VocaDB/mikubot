using System;
using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins {

	public class CoinToss : MsgCommandModuleBase {

		public override void HandleCommand(MsgCommand msg, IBotContext bot) {

			if (!CheckCall(msg, bot))
				return;

			var reply = msg.Reply(bot.Writer);

			if (new Random().Next(100) == 1) {
				reply.Msg("Negis");
				return;
			}

			reply.Msg(new Random().Next(2) == 0 ? "Heads" : "Tails");

		}

		public override int CooldownChannelMs {
			get { return 3000; }
		}

		public override int CooldownUserMs {
			get { return 10000; }
		}

		public override string Name {
			get { return "CoinToss"; }
		}
	}

}
