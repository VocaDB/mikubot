using System;
using System.Configuration;
using System.Linq;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using MikuBot.LinkParsing;

namespace MikuBot.Site.Helpers {

	public static class YoutubeHelper {

		private static readonly RegexLinkMatcher[] matchers = {
			new RegexLinkMatcher("youtu.be/{0}", @"youtu.be/(\S{11})"),
			new RegexLinkMatcher("www.youtube.com/watch?v={0}", @"youtube.com/watch?\S*v=(\S{11})"),
		};

		public static string GetIdBy(string youtubeUrl) {

			var matcher = matchers.FirstOrDefault(m => m.IsMatch(youtubeUrl));
			if (matcher == null)
				return null;

			return matcher.GetId(youtubeUrl);

		}

		public static string GetThumbUrl(string id) {

			var apiKey = ConfigurationManager.AppSettings["YoutubeApiKey"];

			var youtubeService = new YouTubeService(new BaseClientService.Initializer {
				ApiKey = apiKey,
				ApplicationName = "MikuBot"
			});			

			var request = youtubeService.Videos.List("snippet");
			request.Id = id;

			string thumbUrl;

			try {
				var video = request.Execute();
				thumbUrl = video.Items.Any() && video.Items[0].Snippet.Thumbnails.Default != null ? video.Items[0].Snippet.Thumbnails.Default.Url : string.Empty;
			} catch (Exception) {
				return string.Empty;
			}

			return thumbUrl;

		}
 
	}
}