using System;
using System.Globalization;
using System.Threading;
using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins {

	public class Culture : MsgCommandModuleBase {

		public override int BotCommandParamCount {
			get { return 1; }
		}

		public override string CommandDescription {
			get { return "Sets the language and culture used by the bot."; }
		}

		public override string UsageHelp {
			get { return "culture <four-letter culture id>"; }
		}

		public override void HandleCommand(MsgCommand msg, IBotContext bot) {

			if (!CheckCall(msg, bot))
				return;

			var receiver = new Receiver(bot.Writer, msg.Sender.Nick);
			var cultureId = msg.BotCommand.Params[0];

			try {
				Thread.CurrentThread.CurrentCulture 
					= Thread.CurrentThread.CurrentUICulture 
					= CultureInfo.DefaultThreadCurrentCulture 
					= CultureInfo.DefaultThreadCurrentUICulture
					= CultureInfo.GetCultureInfo(cultureId);
			} catch (ArgumentException) {
				receiver.Msg("Unable to set culture");
				return;
			}

			receiver.Msg("Culture was set to " + cultureId);

		}

		public override BotUserLevel MinUserLevel {
			get { return BotUserLevel.Manager; }
		}

		public override string Name {
			get { return "Culture"; }
		}
	}

}
