using System.Collections.Generic;
using System.Web.Mvc;
using CustomerTracker.Common.Helpers;
using CustomerTracker.Web.Angular.Models.Enums;

namespace CustomerTracker.Web.Angular.Controllers
{
    [Authorize(Roles = "Admin,Personel")]
    public class HomeController : Controller
    { 
         

        public ActionResult Index()
        {
            Dictionary<int, string> searchTypes = EnumHelper.ToDictionary<SearchType>();
  
            ViewBag.SearchTypes = searchTypes;

            return View();
        }

        //public JsonResult Search(string searchCriteria, int searchTypeId)
        //{ 
        //    IPagedList<SearchResultModel> resultModels;

        //    switch ((SearchType)searchTypeId)
        //    {
        //        case SearchType.Customer:
        //            resultModels = _searchEngine.SearchCustomers(searchCriteria, 0, "Id", false);
        //            break;
        //        case SearchType.Communication:
        //            resultModels = _searchEngine.SearchCommunications(searchCriteria, 0, "Id", false);
        //            break; 
        //        default:
        //            throw new ArgumentOutOfRangeException();
        //            break;

        //    }

        //    return Json(resultModels, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult RemoteMachineMonitoring()
        {
            return View();
        }
    }

    //public class NinjectControllerFactory : DefaultControllerFactory
    //{
    //    protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
    //    { 
    //        return controllerType == null
    //               ? null
    //               : (IController)NinjectWebCommon.GetKernel.Get(controllerType);

    //        //return base.GetControllerInstance(requestContext, controllerType);
    //    }
    //}
}
