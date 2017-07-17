using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MikuBot.Commands;
using MikuBot.Helpers;
using MikuBot.Modules;
using Rss;
using log4net;

namespace MikuBot.ExtraPlugins {

	public class RedditChecker : GenericModuleBase {

		private const string channelToPost = "#Vocaloid";
		private static readonly TimeSpan checkFreq = TimeSpan.FromMinutes(5);
		private static readonly ILog log = LogManager.GetLogger(typeof(RedditChecker));
		private const string urlToCheck = "http://reddit.com/r/Vocaloid/.rss";

		private readonly HashSet<string> checkedItems = new HashSet<string>(new CaseInsensitiveStringComparer());
		private DateTime lastCheck = DateTime.Now;

		public override string Name {
			get { return "RedditRSS"; }
		}

		public override InitialModuleStatus InitialStatus {
			get { return InitialModuleStatus.Disabled; }
		}

		public override string HelpText {
			get { return "Checks Reddit RSS for new posts."; }
		}

		private void Check(Receiver receiver) {

			var feed = RssFeed.Read(urlToCheck);

			if (feed.Exceptions.LastException != null) {
				log.Warn("Unable to read feed", feed.Exceptions.LastException);
				return;
			}

			if (feed.Channels.Count == 0) {
				log.Warn("Feed contains no channels");
				return;
			}

			var channel = feed.Channels[0];

			var items = channel.Items.Cast<RssItem>().Take(3);

			var newItems = items.Where(i => !checkedItems.Contains(i.Link.ToString())).ToArray();

			foreach (var item in newItems) {

				checkedItems.Add(item.Link.ToString());

				if (receiver != null) {

					var shortLink = GetShortLink(item.Link.ToString());

					receiver.Msg(string.Format("New r/Vocaloid post: {0}{1}{0} - {2}", 
						Formatting.Bold, item.Title, shortLink));
				}

			}

			lastCheck = DateTime.Now;

		}

		private string GetShortLink(string link) {

			return link;

			/*var url = string.Format("http://api.waa.ai/?url={0}", link);

			var request = WebRequest.Create(url);

			try {
				using (var response = request.GetResponse())
				using (var reader = new StreamReader(response.GetResponseStream())) {
					return reader.ReadLine();
				}
			} catch (WebException x) {
				log.Warn("Unable to get shortened URL", x);
				return link;
			}*/

		}

		public override void HandleCommand(IrcCommand command, IBotContext bot) {

			bool check = false;

			if (command is MsgCommand) {

				var msg = (MsgCommand)command;

				if (msg.BotCommand.Is(Name) && command.Sender.UserLevel >= BotUserLevel.Manager) {

					if (msg.BotCommand.Params.ParamOrEmpty(0) == "clear") {
						checkedItems.Clear();
					} else {
						check = true;
					}

				}

			}

			if (DateTime.Now - lastCheck > checkFreq)
				check = true;

			if (check) {

				var receiver = new Receiver(bot.Writer, new IrcName(channelToPost));

				Task.Factory.StartNew(() => Check(receiver))	// Async version
					.ContinueWith(TaskHelper.HandleTaskException, TaskContinuationOptions.OnlyOnFaulted);

			}

		}

		public override void OnLoaded(IBotContext bot, IModuleFile moduleFile) {

			Check(null);

		}

	}
}
