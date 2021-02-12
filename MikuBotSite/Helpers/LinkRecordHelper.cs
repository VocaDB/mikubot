using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MikuBot.DbModel.DataContracts;

namespace MikuBot.Site.Helpers
{
	public static class LinkRecordHelper
	{
		private static readonly string[] pictureExt = { ".jpg", ".jpeg", ".png", ".gif" };
		private static readonly Regex tinypicRegex = new Regex(@"http://\w+\.tinypic\.com/\w+\.\w+");

		public static string GetImageUrl(UrlHelper urlHelper, LinkRecordContract record)
		{
			var pictureUrl = TrimPictureUrl(record.Url);

			if (IsPictureUrl(pictureUrl))
				return pictureUrl;

			//return DanbooruHelper.GetDanbooruImageUrlFromLink(record.Url);

			var danbooruId = DanbooruHelper.GetDanbooruId(record.Url);

			if (!string.IsNullOrEmpty(danbooruId))
				return urlHelper.Action("Preview", "Danbooru", new { id = danbooruId });

			var gelbooruId = GelbooruHelper.GetDanbooruId(record.Url);

			if (!string.IsNullOrEmpty(gelbooruId))
				return urlHelper.Action("Preview", "Gelbooru", new { id = gelbooruId });

			var pixivId = PixivHelper.GetIdFromUrl(record.Url);

			if (!string.IsNullOrEmpty(pixivId))
				return urlHelper.Action("Preview", "Pixiv", new { id = pixivId });

			var vocaDbPreviewUrl = VocaDbHelper.GetVocaDbPreviewUrl(urlHelper, record.Url);

			if (!string.IsNullOrEmpty(vocaDbPreviewUrl))
				return vocaDbPreviewUrl;

			var nicoId = NicoApi.NicoUrlHelper.GetIdByUrl(record.Url);

			if (!string.IsNullOrEmpty(nicoId))
				return urlHelper.Action("Thumbnail", "Nico", new { id = nicoId });

			var youtubeId = YoutubeHelper.GetIdBy(record.Url);

			if (!string.IsNullOrEmpty(youtubeId))
				return urlHelper.Action("Thumbnail", "Youtube", new { id = youtubeId });

			return null;
		}

		public static MvcHtmlString GetParsedHtmlString(string line)
		{
			var encoded = HttpUtility.HtmlEncode(line).Replace("&amp;", "&");
			var processed = WebHelper.ReplaceUrisWithLinks(encoded);

			return new MvcHtmlString(processed);
		}

		private static string TrimPictureUrl(string url)
		{
			if (string.IsNullOrEmpty(url))
				return url;

			// Trim :large from end for Twitter
			if (url.EndsWith(":large"))
			{
				return url.Substring(0, url.Length - 6);
			}

			return url;
		}

		public static bool IsPictureUrl(string url)
		{
			if (string.IsNullOrEmpty(url))
				return false;

			if (pictureExt.Any(e => e.Equals(Path.GetExtension(url), StringComparison.InvariantCultureIgnoreCase)))
				return true;

			if (tinypicRegex.IsMatch(url))
				return true;

			return false;
		}
	}
}