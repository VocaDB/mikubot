using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins {

	public class KeepPrimaryNick : GenericModuleBase {

		public override string HelpText {
			get { return "Attempts to keep the bot's primary nick."; }
		}

		public override void HandleCommand(IrcCommand command, IBotContext bot) {

			if (!(command is PingCommand) || bot.NickManager.CurrentIsPrimary)
				return;

			bot.NickManager.Set(bot.NickManager.Primary);

		}

		public override string Name {
			get { return "KeepPrimaryNick"; }
		}

	}

}
