using System.Web.Mvc;

namespace rmxwebapi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "rmxwebservice";

            return View();
        }
    }
}
