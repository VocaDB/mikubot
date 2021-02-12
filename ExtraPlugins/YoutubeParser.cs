using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using HtmlAgilityPack;
using log4net;
using MikuBot.Commands;
using MikuBot.ExtraPlugins.Helpers;
using MikuBot.Helpers;
using MikuBot.LinkParsing;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins
{
	public class YoutubeParser : MsgCommandModuleBase
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(YoutubeParser));
		private string apiKey;

		private readonly RegexLinkMatcher[] linkMatchers = new[] {
			new RegexLinkMatcher("www.youtube.com/watch?v={0}", @"youtube.com/watch?\S*v=(\S{11})"),
			new RegexLinkMatcher("youtu.be/{0}", @"youtu.be/(\S{11})"),
		};

		private void GetVideoData(Receiver receiver, string id)
		{
			var youtubeService = new YouTubeService(new BaseClientService.Initializer
			{
				ApiKey = apiKey,
				ApplicationName = "MikuBot"
			});

			var request = youtubeService.Videos.List("snippet,statistics");
			request.Id = id;

			try
			{
				var video = request.Execute();

				if (!video.Items.Any())
				{
					receiver.Msg("Youtube (error): not found");
					return;
				}

				receiver.Msg(string.Format("Youtube: {0}", YoutubeUtils.Format(video.Items.First())));
			}
			catch (Exception x)
			{
				log.Warn("Youtube (error)", x);
				receiver.Msg("Youtube (error): " + x.Message);
			}
		}

		private void GetPageContent(Receiver receiver, string url, bool forced)
		{
			string videoTitle = null;
			var request = WebRequest.Create(url);
			WebResponse response;

			try
			{
				response = request.GetResponse();
			}
			catch (WebException x)
			{
				receiver.Msg("Youtube (error): " + x.Message);
				return;
			}

			try
			{
				var enc = response.Headers[HttpResponseHeader.ContentEncoding];

				using (var stream = response.GetResponseStream())
				{
					videoTitle = GetVideoData(stream, enc);
				}
			}
			finally
			{
				response.Close();
			}

			if (!string.IsNullOrEmpty(videoTitle))
			{
				receiver.Msg("Youtube: " + videoTitle);
			}
			else if (forced)
			{
				receiver.Msg("Youtube: no title found.");
			}
		}

		private string GetVideoTitle(HtmlDocument doc)
		{
			var titleElem = doc.DocumentNode.SelectSingleNode("//span[@id = 'eow-title']");
			string titleText = null;

			if (titleElem != null)
				titleText = titleElem.GetAttributeValue("title", null);
			else
			{
				var verifyElem = doc.DocumentNode.SelectSingleNode("//meta[@name = 'title']");

				if (verifyElem != null)
					titleText = verifyElem.GetAttributeValue("content", null);
			}

			return HtmlEntity.DeEntitize(titleText);
		}

		private string GetVideoData(Stream htmlStream, string encodingStr)
		{
			var encoding = (!string.IsNullOrEmpty(encodingStr) ? Encoding.GetEncoding(encodingStr) : Encoding.UTF8);

			var doc = new HtmlDocument();
			doc.Load(htmlStream, encoding);

			// Video title element (could use page title as well...)
			var titleText = GetVideoTitle(doc);

			if (string.IsNullOrEmpty(titleText))
				return null;

			var builder = new StringBuilder(titleText);

			var authorElem = doc.DocumentNode.SelectSingleNode("//a[@class = 'author']");
			var authorText = (authorElem != null ? authorElem.InnerText : null);

			if (!string.IsNullOrEmpty(authorText))
				builder.Append(" by " + authorText);

			var dateElem = doc.DocumentNode.SelectSingleNode("//span[@id = 'eow-date']");
			var dateText = (dateElem != null ? dateElem.InnerText : null);

			if (!string.IsNullOrEmpty(dateText))
				builder.Append(" at " + dateText);

			return builder.ToString();
		}

		public override void HandleCommand(MsgCommand cmd, IBotContext bot)
		{
			if (cmd.BotCommand.Is("NoLink"))
				return;

			var receiver = new Receiver(bot.Writer, cmd.ChannelOrSenderNick);
			string url;
			string id = null;
			bool forced = false;

			if (cmd.BotCommand.Is(Name) && cmd.BotCommand.Params.HasParam(0))
			{
				url = cmd.BotCommand.Params.ParamOrEmpty(0);
				url = PluginHelper.MakeLink(url, true);
				forced = true;
			}
			else
			{
				var possibleUrl = cmd.Text;
				var matcher = linkMatchers.FirstOrDefault(m => m.IsMatch(possibleUrl));

				if (matcher == null)
					return;

				url = PluginHelper.MakeLink(matcher.MakeLink(possibleUrl));
				id = matcher.GetId(url);
			}

			if (string.IsNullOrEmpty(id))
			{
				Task.Factory.StartNew(() => GetPageContent(receiver, url, forced))
					.ContinueWith(TaskHelper.HandleTaskException, TaskContinuationOptions.OnlyOnFaulted);
			}
			else
			{
				Task.Factory.StartNew(() => GetVideoData(receiver, id))
					.ContinueWith(TaskHelper.HandleTaskException, TaskContinuationOptions.OnlyOnFaulted);
			}
		}

		public override void OnLoaded(IBotContext bot, IModuleFile moduleFile)
		{
			apiKey = bot.Config.YoutubeApiKey;
		}

		public override string HelpText
		{
			get { return "Parses Youtube links. By prefixing the link with 'youtube' the link is parsed even if it's not automatically recognized as a Youtube link. By prefixing the link with 'nolink', all link parsing is skipped. This is useful if you don't want some link to be parsed."; }
		}

		public override bool IsPassive
		{
			get { return true; }
		}

		public override string Name
		{
			get { return "Youtube"; }
		}

		public override string UsageHelp
		{
			get { return "[<youtube>|<nolink>] <Youtube URL>"; }
		}
	}
}
