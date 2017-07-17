using System.Collections.Specialized;
using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins {
	public class Sake : MsgCommandModuleBase {

		private readonly StringCollection responses = new StringCollection {
			"zzzzzz",
			"whaddaya mean I can't have more sake?",
			"cheers!",
			"here please!",
			"come here pretty... *waves at MikuB0t*",
		};

		public override int CooldownChannelMs {
			get { return 30000; }
		}

		public override string HelpText {
			get { return "Sake!"; }
		}

		public override InitialModuleStatus InitialStatus {
			get { return InitialModuleStatus.Disabled; }
		}

		public override bool IsPassive {
			get { return true; }
		}

		public override void HandleCommand(MsgCommand chat, IBotContext bot) {

			var txt = chat.Text.ToLower();

			if (txt.Contains("sake") && (!responses.Contains(txt) || chat.HighlightMe)) {

				if (!CheckCooldowns(chat, bot, false))
					return;

				var output = BotHelper.ChooseRandom(responses);
				bot.Writer.Msg(chat.ChannelOrSenderNick, output);

			}

		}

		public override string Name {
			get { return "Sake"; }
		}

	}
}
