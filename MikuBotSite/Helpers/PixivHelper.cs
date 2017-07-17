using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace MikuBot.Site.Helpers {

	public static class PixivHelper {

		private static readonly Regex[] regexes = { 
			new Regex(@"www.pixiv.net/member_illust.php\?mode=medium\&illust_id=(\d+)"),
			new Regex(@"www.pixiv.net/member_illust.php\?mode=big\&illust_id=(\d+)"),
		};

		public static string GetIdFromUrl(string url) {

			var matcher = regexes.FirstOrDefault(r => r.IsMatch(url));

			if (matcher == null)
				return string.Empty;

			var match = matcher.Match(url);

			return match.Groups[1].Value;

		}

		public static string GetImageUrlFromLink(string linkUrl) {

			var id = GetIdFromUrl(linkUrl);

			if (string.IsNullOrEmpty(id))
				return string.Empty;

			return GetImageUrl(id);

		}

		public static string GetImageUrlEmbed(string id) {
			return string.Format("http://embed.pixiv.net/decorate.php?illust_id={0}", id);
		}

		public static string GetImageUrl(string id) {

			var requestUrl = string.Format("http://www.pixiv.net/member_illust.php?mode=medium&illust_id={0}", id);
			var request = (HttpWebRequest)WebRequest.Create(requestUrl);
			request.UserAgent = "MikuBot";
			var doc = new HtmlDocument();

			try {
				using (var response = request.GetResponse())
				using (var stream = response.GetResponseStream()) {
					try {
						doc.Load(stream);
					} catch (ArgumentException) {
						return string.Empty;
					}					
				}
			} catch (WebException) {
				return string.Empty;
			}

			var res = doc.DocumentNode.SelectSingleNode("//meta[@property='og:image']");

			if (res == null || res.Attributes["content"] == null) {
				return string.Empty;
			}

			var att = res.Attributes["content"];

			return att.Value;

		}

	}

}