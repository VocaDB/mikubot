using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins {

	public class Act : MsgCommandModuleBase {

		public override int BotCommandParamCount {
			get { return 2; }
		}

		public override string CommandDescription {
			get { return "Sends an action to a specific receiver."; }
		}

		public override string UsageHelp {
			get { return "act <receiver> <message text>"; }
		}

		public override void HandleCommand(MsgCommand msg, IBotContext bot) {

			if (!CheckCall(msg, bot))
				return;

			var cmdParser = new CmdReader(msg.Text);
			cmdParser.ReadNext();

			var channel = cmdParser.ReadNext();
			var text = cmdParser.ReadToEnd();

			bot.Writer.Action(channel, text);

		}

		public override BotUserLevel MinUserLevel {
			get { return BotUserLevel.Manager; }
		}

		public override string Name {
			get { return "Act"; }
		}
	}

}
