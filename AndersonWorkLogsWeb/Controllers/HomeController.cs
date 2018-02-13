using System.Web.Mvc;

namespace AndersonWorkLogsWeb.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Summary()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}