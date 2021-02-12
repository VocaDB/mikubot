using System.Linq;
using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins
{
	public class Op : MsgCommandModuleBase
	{
		public override int BotCommandParamCount
		{
			get { return 2; }
		}

		public override string CommandDescription
		{
			get { return "Gives ops to one or more users on a channel. The bot needs to be an operator first."; }
		}

		public override string UsageHelp
		{
			get { return "op <channel> <user1> [<user2>]..."; }
		}

		public override void HandleCommand(MsgCommand msg, IBotContext bot)
		{
			if (!CheckCall(msg, bot))
				return;

			var cmdParser = new CmdReader(msg.Text);
			cmdParser.ReadNext();

			var channel = msg.BotCommand.Params[0];
			var nicks = string.Join(" ", msg.BotCommand.Params.Skip(1));

			bot.Writer.Send(string.Format("mode {0} +o {1}", channel, nicks));
		}

		public override BotUserLevel MinUserLevel
		{
			get { return BotUserLevel.Admin; }
		}

		public override string Name
		{
			get { return "Op"; }
		}
	}
}
