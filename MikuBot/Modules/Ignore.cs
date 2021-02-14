using System;
using MikuBot.Commands;

namespace MikuBot.Modules
{
	public class Ignore : BuiltinModule
	{
		public override string HelpText
		{
			get { return "Makes the bot ignore an user."; }
		}

		public override void HandleCommand(MsgCommand cmd, Bot bot)
		{
			if (!cmd.BotCommand.Is(Name) || !cmd.BotCommand.Params.HasParam(0))
				return;

			if (!CheckAccess(cmd, bot))
				return;

			var query = cmd.BotCommand.Params[0];
			var hostName = bot.UserManager.Users.FindUser(query);
			if (hostName == Hostmask.Empty)
				hostName = new Hostmask(query);

			DateTime? until = null;

			if (cmd.BotCommand.Params.HasParam(1))
			{
				TimeSpan.TryParse(cmd.BotCommand.Params[1], out var span);
				until = DateTime.Now + span;
			}

			bot.IgnoredNickList.Ignore(hostName, until);
			var receiver = new Receiver(bot.Writer, cmd.ChannelOrSenderNick);

			if (until == null)
				receiver.Msg("User '" + hostName + "' is now ignored.");
			else
				receiver.Msg("User '" + hostName + "' is now ignored until " + until + ".");
		}

		public override BotUserLevel MinUserLevel
		{
			get { return BotUserLevel.Manager; }
		}

		public override string Name
		{
			get { return "Ignore"; }
		}
	}
}
