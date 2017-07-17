using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins {

	public class Send : MsgCommandModuleBase {

		public override int BotCommandParamCount {
			get { return 2; }
		}

		public override string CommandDescription {
			get { return "Sends a message to a specific receiver."; }
		}

		public override string UsageHelp {
			get { return "send <receiver> <message text>"; }
		}

		public override void HandleCommand(MsgCommand msg, IBotContext bot) {

			if (!CheckCall(msg, bot))
				return;

			var cmdParser = new CmdReader(msg.Text);
			cmdParser.ReadNext();

			var channel = cmdParser.ReadNext();
			var text = cmdParser.ReadToEnd();

			bot.Writer.Msg(channel, text);

		}

		public override BotUserLevel MinUserLevel {
			get { return BotUserLevel.Manager; }
		}

		public override string Name {
			get { return "Send"; }
		}
	}

}
