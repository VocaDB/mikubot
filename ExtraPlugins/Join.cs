using System.Linq;
using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins {

	public class Join : MsgCommandModuleBase {

		public override int BotCommandParamCount {
			get { return 1; }
		}

		public override string CommandDescription {
			get { return "Makes the bot join a channel."; }
		}

		public override BotUserLevel MinUserLevel {
			get { return BotUserLevel.Manager; }
		}

		public override string Name {
			get { return "Join"; }
		}

		public override string UsageHelp {
			get { return "join <channel name> [<channel name 2>] ..."; }
		}

		public override void HandleCommand(MsgCommand cmd, IBotContext bot) {

			if (!CheckCall(cmd, bot))
				return;

			var channels = cmd.BotCommand.Params.Select(c => new IrcName(c));
			bot.ChannelManager.JoinAll(channels);

		}

	}
}
