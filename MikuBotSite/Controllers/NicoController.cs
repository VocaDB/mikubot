using System;
using System.Web.Caching;
using System.Web.Mvc;
using NicoApi;

namespace MikuBot.Site.Controllers {

	public class NicoController : Controller {

		private string GetThumbUrl(string id) {

			var thumbFromCache = HttpContext.Cache[id];

			if (thumbFromCache != null)
				return (string)thumbFromCache;
			
			VideoDataResult data;
			try {
				data = VideoApiClient.GetVideoData(id, false);
			} catch (NicoApiException) {
				return null;
			}

			HttpContext.Cache.Add(id, data.ThumbUrl, null, Cache.NoAbsoluteExpiration, TimeSpan.FromDays(7), CacheItemPriority.Default, null);

			return data.ThumbUrl;

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