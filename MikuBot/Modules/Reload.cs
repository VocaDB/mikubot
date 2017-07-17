using MikuBot.Commands;

namespace MikuBot.Modules {

	public class Reload : BuiltinModule {

		public override void HandleCommand(MsgCommand cmd, Bot bot) {

			if (!cmd.BotCommand.Is(Name))
				return;

			if (!BotHelper.CheckAuthenticated(cmd, bot))
				return;

			var receiver = new Receiver(bot.Writer, cmd.ChannelOrSenderNick);

			bot.ModuleManager.LoadModules(bot, receiver, bot.Config.Modules);
			
		}

		public override string HelpText {
			get { return "Reloads all modules."; }
		}

		public override BotUserLevel MinUserLevel {
			get { return BotUserLevel.Admin; }
		}

		public override string Name {
			get { return "Reload"; }
		}
	}

}
