using MikuBot.Commands;

namespace MikuBot.Modules {

	public class SendRaw : BuiltinModule {

		public override string HelpText {
			get { return "Sends raw data to the server."; }
		}

		public override void HandleCommand(MsgCommand cmd, Bot bot) {

			if (!cmd.BotCommand.Is(Name) || !cmd.BotCommand.Params.HasParam(0))
				return;

			if (!CheckAccess(cmd, bot))
				return;

			var text = cmd.BotCommand.CommandString;
			bot.Writer.Send(text);

		}

		public override BotUserLevel MinUserLevel {
			get { return BotUserLevel.Admin; }
		}

		public override string Name {
			get { return "Send_Raw"; }
		}

	}

}
