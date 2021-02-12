using System.Linq;
using MikuBot.Commands;
using MikuBot.ExtraPlugins.Helpers;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins
{
	public class Gimme : MsgCommandModuleBase
	{
		private class Rule
		{
			private readonly string action;
			private readonly bool tsun;
			private readonly string word;

			public Rule(string word, string action, bool tsun = true)
			{
				this.word = word;
				this.action = action;
				this.tsun = tsun;
			}

			public string Action
			{
				get { return action; }
			}

			public bool Ignore
			{
				get
				{
					return string.IsNullOrEmpty(Action);
				}
			}

			public bool Tsun
			{
				get { return tsun; }
			}

			public string Word
			{
				get { return word; }
			}
		}

		private readonly Rule[] rules = new[] {
			new Rule("negi", Resources.Strings.HandsANegi),
			new Rule("french bread", "gives {0} French bread"),
			new Rule("hug", "hugs {0}"),
			new Rule("kiss", "kisses {0}"),
			new Rule("its life", null),
			new Rule("head on a plate", null)
		};

		public override int BotCommandParamCount
		{
			get { return 1; }
		}

		public override int CooldownChannelMs
		{
			get { return 5000; }
		}

		public override int CooldownUserMs
		{
			get { return 60000; }
		}

		public override string CommandDescription
		{
			get { return "Shares a delicious negi with the user."; }
		}

		public override string Name
		{
			get { return "Gimme"; }
		}

		public override string UsageHelp
		{
			get { return "gimme negi"; }
		}

		public override void HandleCommand(MsgCommand chat, IBotContext bot)
		{
			if (!CheckCall(chat, bot))
				return;

			var par = chat.BotCommand.Params[0].ToLowerInvariant();
			var reply = chat.Reply(bot.Writer);

			var rule = rules.FirstOrDefault(r => par.Equals(r.Word));

			if (rule != null)
			{
				if (rule.Ignore)
					return;

				reply.Action(string.Format(rule.Action, chat.Sender.Nick));

				if (rule.Tsun)
					Tsundere.Tsun(reply);
			}
			else
			{
				//if (BotHelper.CheckAuthenticated(chat, bot, BotUserLevel.Identified)) {
				var commandWord = chat.BotCommand.CommandString;
				reply.Action(string.Format("gives {0} {1}", chat.Sender.Nick, commandWord));

				//}
			}
		}
	}
}
