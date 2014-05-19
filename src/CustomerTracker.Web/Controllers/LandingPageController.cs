using System.Web.Mvc;
using CustomerTracker.Web.Utilities;

namespace CustomerTracker.Web.Controllers
{
    [Authorize(Roles = "Personel,Admin")]
    public class LandingPageController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

      
    }
}
