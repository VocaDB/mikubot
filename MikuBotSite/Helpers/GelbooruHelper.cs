using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace MikuBot.Site.Helpers
{
	public static class GelbooruHelper
	{
		private static readonly Regex regex = new Regex(@"gelbooru\.com/index.php\?page=post\&s=view\&id=(\d+)");

		public static string GetDanbooruId(string url)
		{
			var match = regex.Match(url);

			if (!match.Success)
				return string.Empty;

			return match.Groups[1].Value;
		}

		public static string GetDanbooruImageUrlFromLink(string linkUrl)
		{
			var id = GetDanbooruId(linkUrl);

			if (string.IsNullOrEmpty(id))
				return string.Empty;

			return GetDanbooruImageUrl(id);
		}

		public static string GetDanbooruImageUrl(string id)
		{
			var apiUrl = string.Format("http://www.gelbooru.com/index.php?page=dapi&s=post&q=index&id={0}", id);

			var request = (HttpWebRequest)WebRequest.Create(apiUrl);
			request.UserAgent = "MikuBot";
			XDocument doc;

			try
			{
				using (var response = request.GetResponse())
				using (var stream = response.GetResponseStream())
				{
					try
					{
						doc = XDocument.Load(stream);
					}
					catch (XmlException)
					{
						return string.Empty;
					}
				}
			}
			catch (WebException)
			{
				return string.Empty;
			}

			var res = doc.Element("posts");

			if (res == null || res.Element("post") == null)
			{
				return string.Empty;
			}

			var post = res.Element("post");

			var att = post.Attribute("preview_url");

			if (att == null)
			{
				return string.Empty;
			}

			return string.Format(att.Value);
		}
	}
}