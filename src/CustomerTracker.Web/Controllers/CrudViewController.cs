using System.Web.Mvc;

namespace CustomerTracker.Web.Controllers
{
    [Authorize(Roles = "Admin,Personel")]
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