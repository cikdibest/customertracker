using System;
using System.Web.Mvc;

namespace CustomerTracker.Web.Angular.Controllers
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