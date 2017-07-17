using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikuBot.Modules;
using MikuBot.Commands;
using HtmlAgilityPack;
using System.Net;
using log4net;
using System.IO;
using MikuBot.ExtraPlugins.Helpers;
using System.Threading.Tasks;
using MikuBot.Helpers;

namespace MikuBot.ExtraPlugins {

	/*
	public class MikuDBChecker : GenericModuleBase {

		private class MikuDbAlbum {

			public MikuDbAlbum(string name, string postLink, string dlLink) {

				Name = name;
				DlLink = dlLink;
				PostLink = postLink;

			}

			public string DlLink { get; private set; }

			public string Name { get; private set; }

			public string PostLink { get; private set; }

		}

		private const string channelToPost = "#mikuchan";
		private static readonly TimeSpan checkFreq = TimeSpan.FromMinutes(5);
		private static readonly ILog log = LogManager.GetLogger(typeof(MikuDBChecker));
		private const string urlToCheck = "http://mikudb.com/vocaloid-albums/";

		private readonly HashSet<string> checkedAlbums = new HashSet<string>(new CaseInsensitiveStringComparer());
		private DateTime lastCheck = DateTime.Now;


		private void Check(Receiver receiver) {

			var request = WebRequest.Create(urlToCheck);
			request.Timeout = 5000;
			WebResponse response;

			try {
				response = request.GetResponse();
			} catch (WebException x) {
				log.Error("Unable to download albums list", x);
				return;
			}

			var enc = response.Headers[HttpResponseHeader.ContentEncoding];
			MikuDbAlbum[] albums;

			try {
				using (var stream = response.GetResponseStream()) {
					albums = GetAlbums(stream, enc);
				}
			} finally {
				response.Close();
			}

			if (albums.Any()) {

				var newAlbums = albums.Where(a => !checkedAlbums.Contains(a.Name)).ToArray();

				foreach (var album in newAlbums) {

					checkedAlbums.Add(album.Name);

					if (receiver != null)
						receiver.Msg(string.Format("New album on MikuDB: {0}{1}{0} - {2} - {3}", Formatting.Bold, album.Name, album.PostLink, album.DlLink));

				}

			}

			lastCheck = DateTime.Now;

		}

		private MikuDbAlbum[] GetAlbums(Stream htmlStream, string encStr) {

			var encoding = Helpers.ParseHelper.GetEncoding(encStr);

			var doc = new HtmlDocument();
			doc.Load(htmlStream, encoding);

			var albums = new List<MikuDbAlbum>();
			var posts = doc.DocumentNode.SelectNodes("//div[@id = 'content']/div[contains(@class, 'post')]");

			if (posts == null)
				return albums.ToArray();

			foreach (var post in posts) {

				var postTitleElem = post.SelectSingleNode("div[@class = 'posttop']/h2[@class = 'posttitle']");

				if (postTitleElem == null)
					continue;

				var postLink = postTitleElem.Element("a");

				if (postLink == null)
					continue;

				var postUrl = postLink.Attributes["href"].Value;

				var postTitle = postTitleElem.InnerText;

				var postContentElem = post.SelectSingleNode("div[2]//td[2]");

				if (postContentElem == null)
					continue;

				var links = postContentElem.SelectNodes(".//a");

				if (links == null || !links.Any())
					continue;

				string url = null;
				var mf = links.FirstOrDefault(a => a.Attributes["href"].Value.Contains("www.mediafire.com"));

				if (mf != null)
					url = mf.Attributes["href"].Value;
				else
					url = links.First().Attributes["href"].Value;

				albums.Add(new MikuDbAlbum(postTitle, postUrl, url));

			}

			return albums.ToArray();

		}

		public override string HelpText {
			get { return "Checks MikuDB for new albums."; }
		}

		public override void HandleCommand(IrcCommand command, IBotContext bot) {

			bool check = false;

			if (command is MsgCommand) {

				var msg = (MsgCommand)command;

				if (msg.BotCommand.Is(Name) && command.Sender.UserLevel >= BotUserLevel.Manager) {

					if (msg.BotCommand.Params.ParamOrEmpty(0) == "del" && msg.BotCommand.Params.HasParam(1)) {
						var cmdReader = new CmdReader(msg.BotCommand.CommandString);
						cmdReader.ReadNext();
						var entryName = cmdReader.ReadToEnd();
						var priv = new Receiver(bot.Writer, command.Sender.Nick);
						var s = checkedAlbums.Remove(entryName);
						if (s)
							priv.Msg("Deleted " + entryName);
						else
							priv.Msg("Not found: " + entryName);

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

		public override BotUserLevel MinUserLevel {
			get { return BotUserLevel.Manager; }
		}

		public override InitialModuleStatus InitialStatus {
			get {
				return InitialModuleStatus.NotLoaded;
			}
		}

		public override string Name {
			get { return "MikuDBChecker"; }
		}

	}*/

}
