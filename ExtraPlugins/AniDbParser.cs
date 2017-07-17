using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using log4net;
using MikuBot.Commands;
using MikuBot.Helpers;
using MikuBot.Modules;
using ParseHelper = MikuBot.ExtraPlugins.Helpers.ParseHelper;

namespace MikuBot.ExtraPlugins {

	public class AniDbParser : MsgCommandModuleBase {

		private readonly ILog log = LogManager.GetLogger(typeof (AniDbParser));

		private void GetPageContent(Receiver receiver, string url) {

			string videoTitle = null;
			var request = WebRequest.Create(url);
			//request.Headers.Add("accept-encoding", Encoding.UTF8.HeaderName);
			WebResponse response;

			try {
				response = request.GetResponse();
			} catch (WebException x) {
				receiver.Msg("AniDB (error): " + x.Message);
				return;
			}

			var enc = response.Headers[HttpResponseHeader.ContentEncoding];

			try {
				using (var stream = response.GetResponseStream()) {
					videoTitle = GetVideoTitle(ParseHelper.GetStream(stream, enc), enc);
				}
			} finally {
				response.Close();
			}

			if (!string.IsNullOrEmpty(videoTitle)) {

				receiver.Msg("AniDB: " + videoTitle);

			}

		}

		private string GetVideoTitle(Stream htmlStream, string encStr) {

			var encoding = ParseHelper.GetEncoding(encStr);

			var doc = new HtmlDocument();
			doc.Load(htmlStream, encoding);

			// Video title element (could use page title as well but this is more reliable)
			var titleElem = doc.DocumentNode.SelectSingleNode("//div[@id = 'layout-main']/h1");

			//log.Debug("titleElem: " + titleElem);

			var titleText = (titleElem != null ? titleElem.InnerText : null);

			//log.Debug("titleText:");

			return (titleText != null ? HtmlEntity.DeEntitize(titleText) : null);

		}

		public override string HelpText {
			get { return "Parses AniDB.net links"; }
		}

		public override bool IsPassive {
			get { return true; }
		}

		public override string Name {
			get { return "AniDB"; }
		}

		public override void HandleCommand(MsgCommand cmd, IBotContext bot) {

			const string aniDbLink = "http://anidb.net/perl-bin/animedb.pl";

			var text = cmd.Text.ToLowerInvariant();

			if (!text.Contains(aniDbLink))
				return;

			var idRegex = new Regex(@"[a-z0-9\-\.\:\/\%\?\&\=\+]+");	// Valid URL
			var linkPos = text.IndexOf(aniDbLink);
			var urlMatch = idRegex.Match(cmd.Text, linkPos);

			if (!urlMatch.Success)
				return;

			var receiver = new Receiver(bot.Writer, cmd.ChannelOrSenderNick);
			var url = urlMatch.Value;

			log.Info("Parsing AniDB link " + url);

			//GetPageContent(receiver, url);							// Synchronized version

			Task.Factory.StartNew(() => GetPageContent(receiver, url))	// Async version
				.ContinueWith(TaskHelper.HandleTaskException, TaskContinuationOptions.OnlyOnFaulted);	

		}

	}

}
