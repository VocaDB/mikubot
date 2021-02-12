using System.Collections.Specialized;
using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins
{
	public class Bread : MsgCommandModuleBase
	{
		private readonly StringCollection responses = new StringCollection {
			"mmm...French bread...",
			"mmm...French bread...",
			"I love French bread!",
			"French bread is awesome",
			"more French bread",
			"want French bread?",
			"did you mention French bread?",
		};

		public override int CooldownChannelMs
		{
			get { return 20000; }
		}

		public override string HelpText
		{
			get { return "French bread!"; }
		}

		public override bool IsPassive
		{
			get { return true; }
		}

		public override void HandleCommand(MsgCommand chat, IBotContext bot)
		{
			var txt = chat.Text.ToLower();

			if (txt.Contains("french bread") && (!responses.Contains(txt) || chat.HighlightMe))
			{
				if (!CheckCooldowns(chat, bot, false))
					return;

				var output = BotHelper.ChooseRandom(responses);
				bot.Writer.Msg(chat.ChannelOrSenderNick, output);
			}
		}

		public override string Name
		{
			get { return "FrenchBread"; }
		}
	}
}
