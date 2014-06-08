using System;
using System.Web.Mvc;
using CustomerTracker.Web.App_Start;

namespace CustomerTracker.Web.Controllers
{
    [Authorize(Roles = "Admin,Personel")]
    public class CrudViewController : Controller
    {
        public ActionResult Department()
        {
            throw new Exception("das");
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

        public ActionResult DataMaster()
        {
            return View();
        }

        public ActionResult Product()
        {
            return View();
        }

        public ActionResult Solution()
        {
            return View();
        }
        
        public ActionResult Users()
        {
            return View();
        }
    }
}