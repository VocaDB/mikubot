using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikuBot.Modules;
using MikuBot.Commands;

namespace MikuBot.ExtraPlugins {

	public class UserList : MsgCommandModuleBase {

		public override void HandleCommand(MsgCommand cmd, IBotContext bot) {

			if (!CheckCall(cmd, bot))
				return;

			var reply = new Receiver(bot.Writer, cmd.Sender.Nick);

			foreach (var user in bot.Authenticator.AuthClients) {
				reply.Msg(user.Key + ": " + user.Value.UserLevel);
			}

		}

		public override string CommandDescription {
			get { return "Displays a list of currently authenticated users."; }
		}

		public override string UsageHelp {
			get { return "UserList"; }
		}

		public override BotUserLevel MinUserLevel {
			get { return BotUserLevel.Admin; }
		}

		public override string Name {
			get { return "UserList"; }
		}

	}

}
