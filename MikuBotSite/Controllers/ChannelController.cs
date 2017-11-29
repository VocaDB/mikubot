using System.Web.Mvc;
using MikuBot.DbModel.Services;
using MikuBot.Site.Models.Channel;

namespace MikuBot.Site.Controllers
{
    public class ChannelController : Controller
    {

		private CommonServices Service {
			get { return MvcApplication.Services; }
		}

		private void ClearCache() {
			
			MvcApplication.SessionFactory.EvictQueries();

		}

        //
        // GET: /Link/

        public ActionResult Links(string id, string nick, int? page, bool clearCache = false, int entriesPerPage = 200)
        {

			if (clearCache)
				ClearCache();

			var pageIndex = (page - 1) ?? 0;
			var links = Service.GetRecords(new IrcName("#" + id), nick, pageIndex * entriesPerPage, entriesPerPage);
			var model = new ChannelLinks(id, nick, links, pageIndex, entriesPerPage);

            return View(model);
        }

    }
}
