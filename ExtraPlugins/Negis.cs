using System.Collections.Specialized;
using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins
{
	public class Negis : MsgCommandModuleBase
	{
		private readonly StringCollection responses = new StringCollection {
			"mmm...negis...",
			"mmm...negis...",
			"I love negis!",
			"negis are awesome",
			"more negis",
			"want a negi?",
			"did you mention negis?",
		};

		public override int CooldownChannelMs
		{
			get { return 30000; }
		}

		public override string HelpText
		{
			get { return "Negis!"; }
		}

		public override bool IsPassive
		{
			get { return true; }
		}

		public override void HandleCommand(MsgCommand chat, IBotContext bot)
		{
			var txt = chat.Text.ToLower();

			if (txt.Contains("negis") && (!responses.Contains(txt) || chat.HighlightMe))
			{
				if (!CheckCooldowns(chat, bot, false))
					return;

				var output = BotHelper.ChooseRandom(responses);
				bot.Writer.Msg(chat.ChannelOrSenderNick, output);
			}
		}

		public override string Name
		{
			get { return "Negis"; }
		}
	}
}
