using System.Web.Mvc;
using MikuBot.Site.Helpers;

namespace MikuBot.Site.Controllers
{
	public class PixivController : Controller
	{
		//
		// GET: /Pixiv/

		public ActionResult Preview(string id)
		{
			var sampleUrl = PixivHelper.GetImageUrlEmbed(id);

			if (string.IsNullOrEmpty(sampleUrl))
				return Content(string.Empty);

			return RedirectPermanent(sampleUrl);
		}
	}
}
