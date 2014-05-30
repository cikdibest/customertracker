using System.Web.Mvc;

namespace CustomerTracker.Web.Controllers
{
    public class CrudViewController : Controller
    {
        public ActionResult Department()
        {
            return View();
        }

        public ActionResult Customer()
        {
            return View();
        }

        public ActionResult RemoteMachine()
        {
            return View();
        }
    }
}