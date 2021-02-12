using System;
using MikuBot.Commands;
using MikuBot.ExtraPlugins.Helpers;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins
{
	public class Seen : MsgCommandModuleBase
	{
		private UserActivityMonitor userActivityMonitor;

		private string VerbName(string messageType)
		{
			switch (messageType.ToUpperInvariant())
			{
				case JoinCommand.MessageName:
					return "joined";
				case PartMessage.MessageName:
					return "parted";
				case MsgCommand.MessageName:
					return "messaged";
				case QuitMessage.MessageName:
					return "quit";
				default:
					return "was last active";
			}
		}

		public override int BotCommandParamCount
		{
			get { return 1; }
		}

		public override int CooldownChannelMs
		{
			get { return 1000; }
		}

		public override int CooldownUserMs
		{
			get { return 10000; }
		}

		public override string CommandDescription
		{
			get
			{
				return "Checks when an user was last active. Depends on UserActivityRecording module.";
			}
		}

		public override string Name
		{
			get { return "Seen"; }
		}

		public override string UsageHelp
		{
			get
			{
				return "seen <username> [<channel>]";
			}
		}

		public override void HandleCommand(MsgCommand msg, IBotContext bot)
		{
			if (!CheckCall(msg, bot))
				return;

			var nick = new IrcName(msg.BotCommand.Params[0]);
			var channel = (msg.BotCommand.Params.HasParam(1) ? new IrcName(msg.BotCommand.Params[1]) : msg.ChannelOrSenderNick);
			var receiver = msg.Reply(bot.Writer);

			if (msg.Sender.Nick == nick)
			{
				receiver.Msg("Why are you asking about yourself?");
				return;
			}

			if (bot.OwnNick == nick)
			{
				receiver.Msg("Yes, I'm right here.");
				return;
			}

			var entry = userActivityMonitor.Find(channel, nick);

			if (entry == null)
				receiver.Msg("Don't know when '" + nick + "' was last active.");
			else
			{
				var ago = DateTime.Now - entry.Time;
				if (entry.PrivMsgTime == null || entry.MessageType.Equals(MsgCommand.MessageName, StringComparison.InvariantCultureIgnoreCase))
					receiver.Msg(string.Format("{0} {1} {2} ago.", nick, VerbName(entry.MessageType), BotHelper.FormatTimeSpan(ago)));
				else
				{
					var msgAgo = DateTime.Now - entry.PrivMsgTime;
					receiver.Msg(string.Format("{0} messaged {1} ago and {2} {3} ago.", nick,
						BotHelper.FormatTimeSpan(msgAgo.Value),
						VerbName(entry.MessageType),
						BotHelper.FormatTimeSpan(ago)));
				}
			}
		}

		public override void OnLoaded(IBotContext bot, IModuleFile moduleFile)
		{
			ParamIs.NotNull(() => moduleFile);

			var extraPluginsModuleFile = (ExtraPluginsModuleFile)moduleFile;
			userActivityMonitor = extraPluginsModuleFile.UserActivityMonitor;
		}
	}
}
