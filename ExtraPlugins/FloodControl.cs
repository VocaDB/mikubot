using System;
using System.Collections.Generic;
using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins
{
	public class FloodControl : MsgCommandModuleBase
	{
		private class PostList
		{
			private readonly TimeSpan postExpirationTime = TimeSpan.FromSeconds(20);
			private readonly int postIgnoreCount = 10;

			private readonly Dictionary<Hostmask, List<DateTime>> postTimes =
				new Dictionary<Hostmask, List<DateTime>>(new HostmaskHostnameEqualityComparer());

			private bool IsExpired(DateTime time)
			{
				return (time + postExpirationTime < DateTime.Now);
			}

			public PostList(IConfig config)
			{
				ParamIs.NotNull(() => config);

				postExpirationTime = TimeSpan.FromSeconds(config.FloodPostExpirationTimeSeconds);
				postIgnoreCount = config.FloodPostIgnoreCount;
			}

			public bool WillBeIgnored(Hostmask hostmask)
			{
				if (!postTimes.ContainsKey(hostmask))
				{
					postTimes.Add(hostmask, new List<DateTime>());
				}

				var entry = postTimes[hostmask];
				entry.RemoveAll(IsExpired);

				if (entry.Count > postIgnoreCount)
					return true;
				else
				{
					entry.Add(DateTime.Now);
					return false;
				}
			}
		}

		private TimeSpan defaultIgnoreTime = TimeSpan.FromMinutes(2);
		private PostList postList;

		public override string CommandDescription
		{
			get { return "Automatically ignores an user who is flooding the bot. Doesn't apply to bot admins."; }
		}

		public override void HandleCommand(MsgCommand msg, IBotContext bot)
		{
			if (!msg.HasBotCommand || msg.Sender.IsAuthenticated)
				return;

			if (postList.WillBeIgnored(msg.SenderHost))
			{
				bot.IgnoredNickList.Ignore(msg.SenderHost, DateTime.Now + defaultIgnoreTime);

				bot.Writer.Msg(msg.ChannelOrSenderNick,
					string.Format("{0}: stop harassing me for a while. I'm going to have to ignore you for {1} minutes.",
					msg.Sender.Nick, defaultIgnoreTime.Minutes));
			}
		}

		public override void OnLoaded(IBotContext bot, IModuleFile moduleFile)
		{
			postList = new PostList(bot.Config);

			defaultIgnoreTime = TimeSpan.FromMinutes(bot.Config.FloodPostIgnoreTimeMinutes);
		}

		public override bool IsPassive
		{
			get { return true; }
		}

		public override BotUserLevel MinUserLevel
		{
			get { return BotUserLevel.Unidentified; }
		}

		public override string Name
		{
			get { return "FloodControl"; }
		}
	}
}
