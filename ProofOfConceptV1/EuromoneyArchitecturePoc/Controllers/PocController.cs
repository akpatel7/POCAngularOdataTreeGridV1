using System.Web.Mvc;

namespace EuromoneyArchitecturePoc.Controllers
{
    [Authorize]
    public class PocController : Controller
    {
         [AllowAnonymous]
        public ActionResult OdataExamples()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult AngularPerformanceTests()
        {
            return View();
        }

        public ActionResult DownloadCode()
        {
            return View();
        }

    }
}