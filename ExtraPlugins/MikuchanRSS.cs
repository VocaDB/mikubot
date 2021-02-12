using System;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using MikuBot.Commands;
using MikuBot.Helpers;
using MikuBot.Modules;
using Rss;

namespace MikuBot.ExtraPlugins
{
	/*
	public class MikuchanRSS : MsgCommandModuleBase {
		public override string Name {
			get { return "MikuRSS"; }
		}

		public override int CooldownChannelMs {
			get { return 10000; }
		}

		public override int CooldownUserMs {
			get { return 30000; }
		}

		public override string CommandDescription {
			get { return "Displays mikuchan.org RSS feed."; }
		}

		public override InitialModuleStatus InitialStatus {
			get { return InitialModuleStatus.Disabled; }
		}

		public override string UsageHelp {
			get { return "mikurss [<number of entries>]"; }
		}

		public override void HandleCommand(MsgCommand msg, IBotContext bot) {
			if (!CheckCall(msg, bot))
				return;

			var reply = new Receiver(bot.Writer, msg.ChannelOrSenderNick);
			var feed = RssFeed.Read("http://mikuchan.org/miku/rss.xml");

			if (feed.Exceptions.LastException != null) {
				reply.Msg("Unable to read feed: " + feed.Exceptions.LastException.Message);
				return;
			}

			if (feed.Channels.Count == 0) {
				reply.Msg("Feed contains no channels");
				return;
			}

			var channel = feed.Channels[0];

			var maxEntryCount = Math.Max(Math.Min(ParseHelper.ParseIntOrDefault(msg.BotCommand.Params.ParamOrEmpty(0), 3), 3), 1);
			var itemCount = Math.Min(maxEntryCount, channel.Items.Count);

			reply.Msg(string.Format("Displaying {0} item(s) out of {1} for feed {2} ({3})", itemCount, channel.Items.Count, channel.Title, feed.Url));

			foreach (var item in channel.Items.Cast<RssItem>().Take(maxEntryCount)) {
				var stripped = HtmlEntity.DeEntitize(item.Description);
				stripped = HtmlUtils.StripHTML(stripped);

				stripped = stripped.Replace("\n", " ");
				stripped = stripped.Replace("\r", " ");
				stripped = stripped.Replace("\t", string.Empty);

				var regex = new Regex(@"\[http://.+\]");
				stripped = regex.Replace(stripped, string.Empty);

				if (stripped.Length > 160)
					stripped = stripped.Substring(0, 160) + "...";

				reply.Msg(string.Format("{0}: {1}", item.Link, stripped));
			}
		}
	}*/
}
