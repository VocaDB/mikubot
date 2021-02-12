using MikuBot.Modules;
using MikuBot.Commands;

namespace MikuBot.ExtraPlugins
{
	public class NickServ : GenericModuleBase
	{
		private void Identify(IBotContext bot)
		{
			var pass = bot.Config.NickServPass;

			if (!string.IsNullOrWhiteSpace(pass))
				bot.Writer.Msg("NickServ", "IDENTIFY " + pass);
		}

		public override string HelpText
		{
			get { return "Identifies the bot with NickServ."; }
		}

		public override void OnConnected(IBotContext bot)
		{
			Identify(bot);
		}

		public override void OnLoaded(IBotContext bot, IModuleFile moduleFile)
		{
			if (bot.IsConnected)
				Identify(bot);
		}

		public override void HandleCommand(IrcCommand command, IBotContext bot)
		{
			if (!(command is MsgCommand))
				return;

			var msg = (MsgCommand)command;

			if (!msg.BotCommand.Is(Name) || !CheckAccess(command, bot))
				return;

			Identify(bot);
		}

		public override BotUserLevel MinUserLevel
		{
			get { return BotUserLevel.Manager; }
		}

		public override string Name
		{
			get { return "NickServ"; }
		}
	}
}
