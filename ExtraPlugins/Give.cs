using System.Linq;
using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins
{
	public class Give : MsgCommandModuleBase
	{
		private readonly string[] reflectList = new[] {
			"stab", "concussion", "laceration", "injury", "bash", "knife", "bruise"
		};

		public override int BotCommandParamCount
		{
			get { return 2; }
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
			get { return "Give a present!"; }
		}

		/*
		public override BotUserLevel MinUserLevel {
			get { return BotUserLevel.Identified; }
		}*/

		public override string Name
		{
			get { return "Give"; }
		}

		public override string UsageHelp
		{
			get { return "give <whom> <something>"; }
		}

		public override void HandleCommand(MsgCommand chat, IBotContext bot)
		{
			if (!CheckCall(chat, bot))
				return;

			var cmdParser = new CmdReader(chat.Text);
			cmdParser.ReadNext();

			var target = cmdParser.ReadNext();
			var commandWord = cmdParser.ReadToEnd();

			if (reflectList.Any(commandWord.Contains))
				target = chat.Sender.Nick.ToString();

			var reply = chat.Reply(bot.Writer);
			reply.Action(string.Format("gives {0} {1}", target, commandWord));
		}
	}
}
