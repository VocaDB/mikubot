using System.Configuration;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace MikuBot.Site.Helpers {

	public static class DanbooruHelper {

		private static readonly Regex regex = new Regex(@"danbooru.donmai.us/posts/(\d+)");

		public static string GetDanbooruId(string url) {

			var match = regex.Match(url);

			if (!match.Success)
				return string.Empty;

			return match.Groups[1].Value;

		}

		public static string GetDanbooruImageUrlFromLink(string linkUrl) {

			var id = GetDanbooruId(linkUrl);

			if (string.IsNullOrEmpty(id))
				return string.Empty;

			return GetDanbooruImageUrl(id);

		}

		public static string GetDanbooruImageUrl(string id) {

			var username = ConfigurationManager.AppSettings["DanbooruUserName"];
			var passhash = ConfigurationManager.AppSettings["DanbooruPassHash"];
			var apiUrl = string.Format("http://danbooru.donmai.us/post/index.xml?login={0}&password_hash={1}&tags=id%3A{2}", username, passhash, id);
			//var apiUrl = string.Format("http://danbooru.donmai.us/post/index.xml?tags=id%3A{0}", id);

			var request = (HttpWebRequest)WebRequest.Create(apiUrl);
			request.UserAgent = "MikuBot";
			XDocument doc;

			try {
				using (var response = request.GetResponse())
				using (var stream = response.GetResponseStream()) {
					try {
						doc = XDocument.Load(stream);
					} catch (XmlException) {
						return string.Empty;
					}					
				}
			} catch (WebException) {
				return string.Empty;
			}

			var res = doc.Element("posts");

			if (res == null || res.Element("post") == null) {
				return string.Empty;
			}

			var post = res.Element("post");

			var att = post.Attribute("preview_url");

			if (att == null) {
				return string.Empty;
			}

			return string.Format("http://danbooru.donmai.us{0}", att.Value);

			/*var att = post.Attribute("file_url") ?? post.Attribute("sample_url");

			if (att == null) {
				return string.Empty;
			}

			return att.Value;*/

		}

	}
}