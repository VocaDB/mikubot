using System;
using MikuBot.Commands;

namespace MikuBot.Modules {

	public class Unignore : BuiltinModule {

		public override string HelpText {
			get { return "Makes the bot unignore an user."; }
		}

		public override void HandleCommand(MsgCommand cmd, Bot bot) {

			if (!cmd.BotCommand.Is(Name) || !cmd.BotCommand.Params.HasParam(0))
				return;

			if (!CheckAccess(cmd, bot))
				return;

			var query = cmd.BotCommand.Params[0];
			var hostName = bot.UserManager.Users.FindUser(query);
			if (hostName == Hostmask.Empty)
				hostName = new Hostmask(query);

			var success = bot.IgnoredNickList.Unignore(hostName);

			var receiver = new Receiver(bot.Writer, cmd.ChannelOrSenderNick);

			if (success)
				receiver.Msg("User '" + hostName + "' is now unignored.");
			else
				receiver.Msg("User '" + hostName + "' was not ignored.");


		}

		public override BotUserLevel MinUserLevel {
			get { return BotUserLevel.Manager; }
		}

		public override string Name {
			get { return "UnIgnore"; }
		}

	}
}
