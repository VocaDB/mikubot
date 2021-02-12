using System.Web.Mvc;
using MikuBot.Site.Helpers;

namespace MikuBot.Site.Controllers
{
	public class GelbooruController : Controller
	{
		public ActionResult Preview(string id)
		{
			var sampleUrl = GelbooruHelper.GetDanbooruImageUrl(id);

			if (string.IsNullOrEmpty(sampleUrl))
				return Content(string.Empty);

			return Redirect(sampleUrl);
		}
	}
}