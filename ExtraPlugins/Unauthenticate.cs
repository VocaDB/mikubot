using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins {

	public class Unauthenticate : MsgCommandModuleBase {

		public override void HandleCommand(MsgCommand cmd, IBotContext bot) {

			if (!CheckCall(cmd, bot))
				return;

			var user = cmd.BotCommand.Params.HasParam(0) ? new Hostmask(cmd.BotCommand.Params[0]) : cmd.SenderHost;

			if (bot.Authenticator.Unauthenticate(user)) {
				bot.Writer.Notice(cmd.Sender.Nick, "User has been unauthenticated.");
			} else {
				bot.Writer.Notice(cmd.Sender.Nick, "User is not authenticated.");
			}

		}

		public override string CommandDescription {
			get { return "Unauthenticates (logs off) a user. If no hostmask is specified, the current user is unauthenticated."; }
		}

		public override string UsageHelp {
			get { return "Unauthenticate [<hostmask>]"; }
		}

		public override BotUserLevel MinUserLevel {
			get { return BotUserLevel.Manager; }
		}

		public override string Name {
			get { return "Unauthenticate"; }
		}

	}

}
