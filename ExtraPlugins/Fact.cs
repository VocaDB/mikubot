using System.Collections.Specialized;
using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins
{
	public class Fact : MsgCommandModuleBase
	{
		private readonly StringCollection facts = new StringCollection {
			"Miku's favorite food is Japanese green onions (negis)",
			"Teto's favorite food is French Bread"
		};

		public override int CooldownChannelMs
		{
			get { return 1000; }
		}

		public override int CooldownUserMs
		{
			get { return 30000; }
		}

		public override string CommandDescription
		{
			get
			{
				return "Get your Vocaloid/UTAU facts.";
			}
		}

		public override string Name
		{
			get { return "Fact"; }
		}

		public override void HandleCommand(MsgCommand msg, IBotContext bot)
		{
			if (!CheckCall(msg, bot))
				return;

			var fact = BotHelper.ChooseRandom(facts);

			bot.Writer.Msg(msg.ChannelOrSenderNick, fact);
		}
	}
}
