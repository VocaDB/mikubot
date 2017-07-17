using System;
using System.Linq;
using MikuBot.Commands;

namespace MikuBot.Modules {

	public class Authenticate : BuiltinModule {

		public override void HandleCommand(MsgCommand cmd, Bot bot) {

			var receiver = new Receiver(bot.Writer, cmd.Sender.Nick);

			if (!cmd.BotCommand.Params.HasParam(0)) {
				receiver.Notice("Your user level is '" + cmd.Sender.UserLevel + "'");
				return;
			}

			if (!cmd.BotCommand.Is(Name, BotCommandMethod.Private))
				return;

			var key = cmd.BotCommand.Params[0];

			if (bot.Authenticator.Authenticate(cmd.SenderHost, key)) {
				receiver.Msg("You have been authenticated");
			} else {
				receiver.Msg("Key not recognized");
			}

		}

		public override string HelpText {
			get { return "Authenticates the user, allowing restricted commands. This command must be sent in PM!"; }
		}

		public override string Name {
			get { return "Authenticate"; }
		}

	}

}
