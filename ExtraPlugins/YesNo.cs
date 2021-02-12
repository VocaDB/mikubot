using System;
using System.Collections.Specialized;
using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins
{
	public class YesNo : MsgCommandModuleBase
	{
		private readonly StringCollection positive = new StringCollection {
			"Yes", "Definitely", "Affirmative", "Most certainly", "Yeah, sure"
		};

		private readonly StringCollection negative = new StringCollection {
			"No", "Nope", "Definitely not", "Negative", "I don't think so"
		};


		public override void HandleCommand(MsgCommand msg, IBotContext bot)
		{
			if (!CheckCall(msg, bot))
				return;

			var reply = msg.Reply(bot.Writer);

			if (msg.ParamCollection.Count < 1 || !msg.BotCommand.CommandString.EndsWith("?"))
			{
				reply.Msg("Please ask a question!");
				return;
			}

			reply.Msg(new Random().Next(2) == 0 ? BotHelper.ChooseRandom(positive) : BotHelper.ChooseRandom(negative));
		}

		public override string CommandDescription
		{
			get { return "Responds to a question with either a positive or negative answer."; }
		}

		public override int CooldownChannelMs
		{
			get { return 3000; }
		}

		public override int CooldownUserMs
		{
			get { return 10000; }
		}

		public override string Name
		{
			get { return "YesNo"; }
		}

		public override string UsageHelp
		{
			get { return "yesno <question>"; }
		}
	}
}
