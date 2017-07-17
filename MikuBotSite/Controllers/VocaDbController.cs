using System.Net;
using System.Web.Mvc;

namespace MikuBot.Site.Controllers {

	public class VocaDbController : Controller {

		private const string vocaDbPath = "http://vocadb.net";

		public ActionResult PreviewSong(int id) {

			string thumbUrl;

			using (var request = new WebClient()) {
				thumbUrl = request.DownloadString(string.Format("{0}/Song/ThumbUrl/{1}", vocaDbPath, id));
			}

			if (!string.IsNullOrEmpty(thumbUrl))
				return Redirect(thumbUrl);

			return Content("");

		}

	}
}