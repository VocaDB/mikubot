using MikuBot.Commands;

namespace MikuBot.Modules {

	public class Quit : BuiltinModule {

		public override void HandleCommand(MsgCommand cmd, Bot bot) {

			if (!cmd.BotCommand.Is(Name))
				return;

			if (!BotHelper.CheckAuthenticated(cmd, bot))
				return;

			bot.Quit();

		}

		public override string HelpText {
			get { return "Shuts down the bot"; }
		}

		public override BotUserLevel MinUserLevel {
			get { return BotUserLevel.Admin; }
		}

		public override string Name {
			get { return "Quit"; }
		}

	}

}
