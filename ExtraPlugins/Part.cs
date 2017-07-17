using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins {

	public class Part : MsgCommandModuleBase {

		public override string CommandDescription {
			get { return "Makes the bot leave a channel."; }
		}

		public override string UsageHelp {
			get { return "part [<channel>]"; }
		}

		public override void HandleCommand(MsgCommand msg, IBotContext bot) {

			if (!CheckCall(msg, bot))
				return;

			if (msg.BotCommand.Params.Count == 0 && !msg.ReceiverIsChannel) {
				bot.Writer.Msg(msg.ChannelOrSenderNick, "Channel must be specified");
				return;
			}

			var channel = msg.BotCommand.Params.Count > 0 ? new IrcName(msg.BotCommand.Params[0]) : msg.Receiver;

			bot.ChannelManager.Part(channel);

		}

		public override BotUserLevel MinUserLevel {
			get { return BotUserLevel.Manager; }
		}

		public override string Name {
			get { return "Part"; }
		}

	}

}
