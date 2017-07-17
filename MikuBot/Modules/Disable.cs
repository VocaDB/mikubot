using MikuBot.Commands;

namespace MikuBot.Modules {

	public class Disable : BuiltinModule {

		public override string HelpText {
			get { return "Disables a plugin module (built-in modules cannot be disabled)."; }
		}

		public override void HandleCommand(MsgCommand cmd, Bot bot) {

			if (!cmd.BotCommand.Is(Name) || !cmd.BotCommand.Params.HasParam(0))
				return;

			if (!CheckAccess(cmd, bot))
				return;

			var moduleName = cmd.BotCommand.Params[0];
			var success = bot.ModuleManager.DisablePluginModule(moduleName);
			var receiver = new Receiver(bot.Writer, cmd.ChannelOrSenderNick);

			if (success) {
				receiver.Msg("Module '" + moduleName + "' has been disabled");
			} else {
				receiver.Msg("Module '" + moduleName + "' not found or already disabled");
			}

		}

		public override BotUserLevel MinUserLevel {
			get { return BotUserLevel.Manager; }
		}

		public override string Name {
			get { return "Disable"; }
		}

	}

}
