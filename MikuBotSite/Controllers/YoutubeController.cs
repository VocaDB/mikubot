using System;
using System.Web.Caching;
using System.Web.Mvc;
using MikuBot.Site.Helpers;

namespace MikuBot.Site.Controllers {

	public class YoutubeController : Controller {

		private string GetThumbUrl(string id) {

			var thumbFromCache = HttpContext.Cache[id];

			if (thumbFromCache != null)
				return (string)thumbFromCache;

			var thumbUrl = YoutubeHelper.GetThumbUrl(id);

			HttpContext.Cache.Add(id, thumbUrl, null, Cache.NoAbsoluteExpiration, TimeSpan.FromDays(7), CacheItemPriority.BelowNormal, null);

			return thumbUrl;

		}

		public ActionResult Thumbnail(string id) {

			var thumbUrl = GetThumbUrl(id);

			if (!string.IsNullOrEmpty(thumbUrl)) {
				return RedirectPermanent(thumbUrl);
			}

			return new EmptyResult();

		}
	}

}